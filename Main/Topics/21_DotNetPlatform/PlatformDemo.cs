using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic21;

/// <summary>
/// Practical demonstrations for Topic 21: .NET Platform & Dependency Injection
/// </summary>
public static class PlatformDemo
{
    // Interface contracts
    public interface ITelemetryService
    {
        Guid OperationId { get; }
        void LogEvent(string eventName);
    }

    public interface INotificationGateway
    {
        void DispatchAlert(string recipient, string alert);
    }

    // Concrete implementation tracking lifetime via unique Guid
    public class DiagnosticsTelemetryService : ITelemetryService
    {
        public Guid OperationId { get; } = Guid.NewGuid();

        public void LogEvent(string eventName)
        {
            Console.WriteLine($"    • [Telemetry {OperationId.ToString()[..8]}] Logged: {eventName}");
        }
    }

    public class SmtpNotificationGateway : INotificationGateway
    {
        private readonly ITelemetryService _telemetry;

        // Constructor Injection: The dependency is handed in from outside
        public SmtpNotificationGateway(ITelemetryService telemetry)
        {
            _telemetry = telemetry;
        }

        public void DispatchAlert(string recipient, string alert)
        {
            _telemetry.LogEvent($"Dispatching SMTP payload to {recipient}");
            Console.WriteLine($"      -> SMTP Message sent to {recipient}: \"{alert}\"");
        }
    }

    // A lightweight service locator / mock IoC container to illustrate lifetime mechanics
    public class SimpleServiceContainer
    {
        private readonly Dictionary<Type, Func<object>> _transientFactories = new();
        private readonly Dictionary<Type, object> _singletons = new();

        public void RegisterTransient<TInterface, TImplementation>() where TImplementation : TInterface, new()
        {
            _transientFactories[typeof(TInterface)] = () => new TImplementation();
        }

        public void RegisterSingleton<TInterface>(TInterface instance) where TInterface : notnull
        {
            _singletons[typeof(TInterface)] = instance;
        }

        public T Resolve<T>()
        {
            if (_singletons.TryGetValue(typeof(T), out object? singleton))
            {
                return (T)singleton;
            }

            if (_transientFactories.TryGetValue(typeof(T), out var factory))
            {
                return (T)factory();
            }

            throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered!");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🏗 TOPIC 21: .NET PLATFORM & INVERSION OF CONTROL (DI)");
        Console.WriteLine("=======================================================");

        // 1. Constructor Injection in Action
        Console.WriteLine("[1] Constructor Injection (Decoupled Architecture):");
        ITelemetryService telemetry = new DiagnosticsTelemetryService();
        var gateway = new SmtpNotificationGateway(telemetry);
        gateway.DispatchAlert("admin@enterprise.com", "Server CPU Threshold exceeded 90%");

        // 2. The 3 Lifetimes Demonstration: Transient vs. Singleton
        Console.WriteLine("\n[2] DI Lifetime Semantics Proof (Transient vs. Singleton):");
        var container = new SimpleServiceContainer();

        // Register Transient: New Guid every resolve
        container.RegisterTransient<ITelemetryService, DiagnosticsTelemetryService>();
        
        var transientA = container.Resolve<ITelemetryService>();
        var transientB = container.Resolve<ITelemetryService>();
        Console.WriteLine($"    • Transient Instance A Guid: {transientA.OperationId}");
        Console.WriteLine($"    • Transient Instance B Guid: {transientB.OperationId} (Distinct instance created!)");

        // Register Singleton: Identical Guid every resolve
        var sharedTelemetry = new DiagnosticsTelemetryService();
        container.RegisterSingleton<ITelemetryService>(sharedTelemetry);

        var singletonA = container.Resolve<ITelemetryService>();
        var singletonB = container.Resolve<ITelemetryService>();
        Console.WriteLine($"    • Singleton Instance A Guid: {singletonA.OperationId}");
        Console.WriteLine($"    • Singleton Instance B Guid: {singletonB.OperationId} (Shared identical instance!)");
        Console.WriteLine($"    • References match exactly : {ReferenceEquals(singletonA, singletonB)}");
    }
}
