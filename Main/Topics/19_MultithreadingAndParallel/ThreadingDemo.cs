using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpMastery.Topics.Topic19;

/// <summary>
/// Practical demonstrations for Topic 19: Multithreading & Synchronization
/// </summary>
public static class ThreadingDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 19: MULTITHREADING, LOCKS & PARALLEL LOOPS");
        Console.WriteLine("=======================================================");

        // 1. Thread-safe Atomic increment via Interlocked
        int atomicCounter = 0;
        Parallel.For(0, 1000, _ =>
        {
            Interlocked.Increment(ref atomicCounter);
        });
        Console.WriteLine($"Interlocked parallel sum (Expected 1000): {atomicCounter}");

        // 2. Thread-safe ConcurrentDictionary
        var threadSafeCache = new ConcurrentDictionary<string, int>();
        Parallel.For(0, 5, i =>
        {
            threadSafeCache.TryAdd($"Key_{i}", i * 10);
        });
        Console.WriteLine($"ConcurrentDictionary entry count: {threadSafeCache.Count}");

        // 3. SemaphoreSlim for rate limiting
        using var semaphore = new SemaphoreSlim(2, 2); // Allows max 2 concurrent threads
        Console.WriteLine("Semaphore configured for max 2 concurrent accessors.");
    }
}
