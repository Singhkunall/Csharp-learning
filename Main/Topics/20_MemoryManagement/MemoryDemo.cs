using System;
using System.Diagnostics;

namespace CSharpMastery.Topics.Topic20;

/// <summary>
/// Practical demonstrations for Topic 20: Memory Management, GC & Span<T>
/// </summary>
public static class MemoryDemo
{
    // Implementation of the Full Standard Dispose Pattern
    public class ManagedResourceGateway : IDisposable
    {
        private bool _isDisposed = false;
        private IntPtr _simulatedNativeHandle = new IntPtr(0x1024);

        public void PerformWork()
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(ManagedResourceGateway));
            Console.WriteLine("    • [ManagedResourceGateway] Executing operation on active unmanaged handle.");
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            // Critical optimization: removes object from GC Finalizer Queue!
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                // Clean up managed objects here (e.g. database connections)
            }

            // Clean up unmanaged OS handles
            _simulatedNativeHandle = IntPtr.Zero;
            _isDisposed = true;
            Console.WriteLine("    • [Dispose Pattern] Native handles freed, GC.SuppressFinalize executed.");
        }

        // Finalizer fallback (Only executes if developer forgot to call Dispose)
        ~ManagedResourceGateway()
        {
            Dispose(disposing: false);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🧠 TOPIC 20: MEMORY ARCHITECTURE, SPAN<T> & THE GC");
        Console.WriteLine("=======================================================");

        // 1. CLR Garbage Collection Generations
        Console.WriteLine("[1] Garbage Collection Generations & Memory Diagnostics:");
        long memoryBefore = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"    • Total Allocated Heap Memory: {memoryBefore:N0} bytes");

        var tempObj = new object();
        Console.WriteLine($"    • Newly allocated object lives in: Generation {GC.GetGeneration(tempObj)}");
        Console.WriteLine($"    • Lifetime Gen 0 Collections Count: {GC.CollectionCount(0)}");
        Console.WriteLine($"    • Lifetime Gen 1 Collections Count: {GC.CollectionCount(1)}");
        Console.WriteLine($"    • Lifetime Gen 2 Collections Count: {GC.CollectionCount(2)}");

        // 2. Zero-Allocation High-Speed Slicing with ReadOnlySpan<char>
        Console.WriteLine("\n[2] Zero-Allocation Slicing via ReadOnlySpan<char>:");
        string logHeader = "TIMESTAMP=2026-10-21;SEVERITY=CRITICAL;PAYLOAD_ID=98234;";

        // No Substring allocations! Sliding a pointer window directly over memory:
        ReadOnlySpan<char> span = logHeader.AsSpan();

        int severityStart = span.IndexOf("SEVERITY=") + 9;
        int severityEnd = span[severityStart..].IndexOf(';');
        ReadOnlySpan<char> severitySpan = span.Slice(severityStart, severityEnd);

        Console.WriteLine($"    • Extracted Severity via Span: '{severitySpan}' (Zero Heap Allocations!)");

        // 3. Boxing & Unboxing Memory Semantics
        Console.WriteLine("\n[3] Boxing and Unboxing Overhead:");
        int valueTypeInt = 42;          // 4 bytes directly on the Stack
        
        // BOXING: Allocates a new 24-byte object on the Heap to wrap the 42:
        object boxedObject = valueTypeInt; 
        
        // UNBOXING: Copies the 42 back from the Heap to the Stack:
        int unboxedInt = (int)boxedObject;

        Console.WriteLine($"    • Original Value: {valueTypeInt} (Stack) -> Boxed to Heap -> Unboxed to: {unboxedInt} (Stack)");

        // 4. The Full Standard Dispose Pattern
        Console.WriteLine("\n[4] Deterministic Resource Cleanup via IDisposable & using:");
        using (var gateway = new ManagedResourceGateway())
        {
            gateway.PerformWork();
        } // gateway.Dispose() automatically called here!

        // 5. WeakReference<T> (Caching without Preventing Garbage Collection)
        Console.WriteLine("\n[5] WeakReference<T> (Cache that yields to memory pressure):");
        var heavyReport = new byte[1024]; // Simulated report
        var weakCache = new WeakReference<byte[]>(heavyReport);

        if (weakCache.TryGetTarget(out byte[]? cachedTarget))
        {
            Console.WriteLine($"    • WeakReference target alive and retrieved: {cachedTarget.Length} bytes.");
        }

        // Release strong reference:
        heavyReport = null!;
        // The object is now eligible for GC collection if memory pressure rises!
        Console.WriteLine("    • Strong reference dropped; object ready for memory reclamation by GC.");
    }
}
