using System;
using System.Collections.Generic;

namespace AspNetCoreBasics.DependencyInjection;

// ============================================================================
// Service Interfaces and Lifetime Contract Demonstrations
// ============================================================================

public interface ITransientOperation
{
    Guid OperationId { get; }
}

public interface IScopedOperation : IDisposable
{
    Guid OperationId { get; }
    bool IsDisposed { get; }
}

public interface ISingletonOperation
{
    Guid OperationId { get; }
    DateTime CreatedAtUtc { get; }
}

public class OperationService : ITransientOperation, IScopedOperation, ISingletonOperation
{
    public Guid OperationId { get; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

// ============================================================================
// Captive Dependency Demonstration Classes
// ============================================================================

public interface IOrderDatabaseContext : IDisposable
{
    Guid ContextId { get; }
    void SaveOrder(string orderId);
    bool IsDisposed { get; }
}

public class OrderDatabaseContext : IOrderDatabaseContext
{
    public Guid ContextId { get; } = Guid.NewGuid();
    public bool IsDisposed { get; private set; }

    public void SaveOrder(string orderId)
    {
        if (IsDisposed)
            throw new ObjectDisposedException(nameof(OrderDatabaseContext), "Cannot access a disposed DbContext!");

        Console.WriteLine($"      [Database] Order '{orderId}' written via DbContext ({ContextId.ToString()[..8]}).");
    }

    public void Dispose()
    {
        IsDisposed = true;
    }
}

// ⚠️ Anti-pattern: Captive Dependency (Singleton directly captures a Scoped dependency)
public class BrokenSingletonWorker
{
    private readonly IOrderDatabaseContext _captiveDbContext;

    public BrokenSingletonWorker(IOrderDatabaseContext captiveDbContext)
    {
        _captiveDbContext = captiveDbContext;
    }

    public void DoWork(string orderId)
    {
        Console.WriteLine($"      [BrokenSingletonWorker] Attempting to write with captured DbContext...");
        _captiveDbContext.SaveOrder(orderId);
    }
}

// ✅ Correct Pattern: Singleton uses IServiceScopeFactory to safely scope execution
public class HealthySingletonWorker
{
    private readonly ISimulatedScopeFactory _scopeFactory;

    public HealthySingletonWorker(ISimulatedScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void DoWork(string orderId)
    {
        Console.WriteLine($"      [HealthySingletonWorker] Creating safe scope to resolve DbContext...");
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ResolveScoped<IOrderDatabaseContext>();
        db.SaveOrder(orderId);
        Console.WriteLine($"      [HealthySingletonWorker] Scope ended, DbContext disposed cleanly.");
    }
}

// ============================================================================
// Lightweight DI Container Engine Simulation
// ============================================================================

public interface ISimulatedScope : IDisposable
{
    T ResolveScoped<T>() where T : class;
    T ResolveTransient<T>() where T : class;
    T ResolveSingleton<T>() where T : class;
}

public interface ISimulatedScopeFactory
{
    ISimulatedScope CreateScope();
}

public class SimulatedDiContainer : ISimulatedScopeFactory
{
    private readonly Dictionary<Type, Func<object>> _transientFactories = new();
    private readonly Dictionary<Type, Func<object>> _scopedFactories = new();
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly Dictionary<Type, Func<object>> _singletonFactories = new();

    public void RegisterTransient<TInterface, TImpl>() where TImpl : TInterface, new()
    {
        _transientFactories[typeof(TInterface)] = () => new TImpl();
    }

    public void RegisterScoped<TInterface, TImpl>() where TImpl : TInterface, new()
    {
        _scopedFactories[typeof(TInterface)] = () => new TImpl();
    }

    public void RegisterSingleton<TInterface, TImpl>() where TImpl : TInterface, new()
    {
        _singletonFactories[typeof(TInterface)] = () => new TImpl();
    }

    public T ResolveSingleton<T>() where T : class
    {
        var type = typeof(T);
        if (!_singletons.TryGetValue(type, out var instance))
        {
            if (_singletonFactories.TryGetValue(type, out var factory))
            {
                instance = factory();
                _singletons[type] = instance;
            }
            else
            {
                throw new InvalidOperationException($"No registration found for singleton {type.Name}");
            }
        }
        return (T)instance;
    }

    public ISimulatedScope CreateScope()
    {
        return new SimulatedScope(this);
    }

    private class SimulatedScope : ISimulatedScope
    {
        private readonly SimulatedDiContainer _root;
        private readonly Dictionary<Type, object> _scopedInstances = new();
        private readonly List<IDisposable> _disposables = new();
        private bool _isDisposed;

        public SimulatedScope(SimulatedDiContainer root)
        {
            _root = root;
        }

        public T ResolveTransient<T>() where T : class
        {
            EnsureNotDisposed();
            var type = typeof(T);
            if (!_root._transientFactories.TryGetValue(type, out var factory))
                throw new InvalidOperationException($"No transient registration for {type.Name}");

            var instance = (T)factory();
            if (instance is IDisposable d)
                _disposables.Add(d);

            return instance;
        }

        public T ResolveScoped<T>() where T : class
        {
            EnsureNotDisposed();
            var type = typeof(T);
            if (!_scopedInstances.TryGetValue(type, out var instance))
            {
                if (!_root._scopedFactories.TryGetValue(type, out var factory))
                    throw new InvalidOperationException($"No scoped registration for {type.Name}");

                instance = factory();
                _scopedInstances[type] = instance;

                if (instance is IDisposable d)
                    _disposables.Add(d);
            }
            return (T)instance;
        }

        public T ResolveSingleton<T>() where T : class
        {
            EnsureNotDisposed();
            return _root.ResolveSingleton<T>();
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
            _scopedInstances.Clear();
            _disposables.Clear();
        }

        private void EnsureNotDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(SimulatedScope), "Cannot resolve services from a disposed scope!");
        }
    }
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class DependencyInjectionDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 💉 MODULE 06: DEPENDENCY INJECTION & SERVICE LIFETIMES");
        Console.WriteLine("=======================================================\n");

        var container = new SimulatedDiContainer();

        // 1. Service Registrations
        Console.WriteLine("[1] Registering Services with DI Container...");
        container.RegisterTransient<ITransientOperation, OperationService>();
        container.RegisterScoped<IScopedOperation, OperationService>();
        container.RegisterSingleton<ISingletonOperation, OperationService>();
        container.RegisterScoped<IOrderDatabaseContext, OrderDatabaseContext>();

        Console.WriteLine("    ✓ Transient:  ITransientOperation => OperationService (New instance every call)");
        Console.WriteLine("    ✓ Scoped:     IScopedOperation    => OperationService (One per HTTP Request Scope)");
        Console.WriteLine("    ✓ Singleton:  ISingletonOperation => OperationService (Single app-wide instance)\n");

        // 2. Simulating HTTP Request A (Scope 1)
        Console.WriteLine("[2] Simulating HTTP Request A (Incoming Thread 1)...");
        IScopedOperation scope1ScopedOp1;
        IScopedOperation scope1ScopedOp2;
        ITransientOperation scope1TransientOp1;
        ITransientOperation scope1TransientOp2;
        ISingletonOperation singletonOpA;

        using (var scopeA = container.CreateScope())
        {
            scope1TransientOp1 = scopeA.ResolveTransient<ITransientOperation>();
            scope1TransientOp2 = scopeA.ResolveTransient<ITransientOperation>();
            scope1ScopedOp1 = scopeA.ResolveScoped<IScopedOperation>();
            scope1ScopedOp2 = scopeA.ResolveScoped<IScopedOperation>();
            singletonOpA = scopeA.ResolveSingleton<ISingletonOperation>();

            Console.WriteLine($"    • Transient #1 ID: {scope1TransientOp1.OperationId.ToString()[..8]}");
            Console.WriteLine($"    • Transient #2 ID: {scope1TransientOp2.OperationId.ToString()[..8]} (Different from #1)");
            Console.WriteLine($"    • Scoped #1 ID:    {scope1ScopedOp1.OperationId.ToString()[..8]}");
            Console.WriteLine($"    • Scoped #2 ID:    {scope1ScopedOp2.OperationId.ToString()[..8]} (MATCHES #1 within same scope!)");
            Console.WriteLine($"    • Singleton ID:    {singletonOpA.OperationId.ToString()[..8]}");
        }
        Console.WriteLine($"    ✓ Scope A ended: Scoped Instance Disposed = {scope1ScopedOp1.IsDisposed}\n");

        // 3. Simulating HTTP Request B (Scope 2)
        Console.WriteLine("[3] Simulating HTTP Request B (Incoming Thread 2)...");
        using (var scopeB = container.CreateScope())
        {
            var scope2Transient = scopeB.ResolveTransient<ITransientOperation>();
            var scope2Scoped = scopeB.ResolveScoped<IScopedOperation>();
            var singletonOpB = scopeB.ResolveSingleton<ISingletonOperation>();

            Console.WriteLine($"    • Transient ID:    {scope2Transient.OperationId.ToString()[..8]} (Brand new)");
            Console.WriteLine($"    • Scoped ID:       {scope2Scoped.OperationId.ToString()[..8]} (Unique to Scope B!)");
            Console.WriteLine($"    • Singleton ID:    {singletonOpB.OperationId.ToString()[..8]} (EXACT SAME as Request A!)");
            Console.WriteLine($"    • Same Singleton?  {ReferenceEquals(singletonOpA, singletonOpB)}");
        }
        Console.WriteLine();

        // 4. Demonstrating the Captive Dependency Trap & Safe Solution
        Console.WriteLine("[4] Captive Dependency Trap vs. IServiceScopeFactory Pattern:");
        Console.WriteLine("    Scenario: A Singleton Worker needs to access a Scoped DbContext.");

        var healthyWorker = new HealthySingletonWorker(container);
        healthyWorker.DoWork("ORD-1001");
        healthyWorker.DoWork("ORD-1002");
        Console.WriteLine("    ✓ Both database operations executed with independent, safely disposed scopes.");
    }
}
