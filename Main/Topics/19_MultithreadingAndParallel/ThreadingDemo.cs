using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpMastery.Topics.Topic19;

/// <summary>
/// Practical demonstrations for Topic 19: Multithreading, Parallelism & Synchronization
/// </summary>
public static class ThreadingDemo
{
    private static readonly object _stoveLock = new();
    private static int _unsafeCounter = 0;
    private static int _lockedCounter = 0;
    private static int _atomicCounter = 0;

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🧵 TOPIC 19: MULTITHREADING, SYNCHRONIZATION & LOCKS");
        Console.WriteLine("=======================================================");

        // 1. Race Condition Demonstration: Unsafe vs. Locked vs. Interlocked
        Console.WriteLine("[1] The Corrupted Soup Pot (Demonstrating a Race Condition):");
        _unsafeCounter = 0;
        _lockedCounter = 0;
        _atomicCounter = 0;

        const int iterations = 50_000;

        // Unsynchronized parallel increment (Corrupts data via race conditions)
        Parallel.For(0, iterations, _ =>
        {
            _unsafeCounter++; // Non-atomic read-modify-write!
        });
        Console.WriteLine($"    • Unsynchronized Counter: {_unsafeCounter} (LOST UPDATES! Expected {iterations})");

        // Synchronized via 'lock' (Monitor)
        Parallel.For(0, iterations, _ =>
        {
            lock (_stoveLock)
            {
                _lockedCounter++;
            }
        });
        Console.WriteLine($"    • Locked Counter (lock)   : {_lockedCounter} (Exact match: {iterations})");

        // Synchronized via hardware atomic 'Interlocked'
        Parallel.For(0, iterations, _ =>
        {
            Interlocked.Increment(ref _atomicCounter);
        });
        Console.WriteLine($"    • Hardware Interlocked    : {_atomicCounter} (Exact match: {iterations} - Zero lock overhead!)");

        // 2. Deadlock Prevention using Monitor.TryEnter with Timeout
        Console.WriteLine("\n[2] Deadlock Prevention via Monitor.TryEnter Timeout:");
        object resourceA = new();
        bool acquiredLock = false;

        try
        {
            // Attempt to acquire lock, but give up after 100 milliseconds if held:
            Monitor.TryEnter(resourceA, TimeSpan.FromMilliseconds(100), ref acquiredLock);
            if (acquiredLock)
            {
                Console.WriteLine("    • Successfully acquired lock on Resource A safely within timeout.");
            }
            else
            {
                Console.WriteLine("    • Could not acquire lock within 100ms. Aborting to prevent deadlock!");
            }
        }
        finally
        {
            if (acquiredLock) Monitor.Exit(resourceA);
        }

        // 3. SemaphoreSlim (Throttled Pool of Concurrent Accessors)
        Console.WriteLine("\n[3] SemaphoreSlim (Throttling Max 2 Active Workers at Once):");
        using var limiter = new SemaphoreSlim(initialCount: 2, maxCount: 2);
        var activeWorkers = new List<Task>();

        for (int i = 1; i <= 4; i++)
        {
            int workerId = i;
            activeWorkers.Add(Task.Run(async () =>
            {
                Console.WriteLine($"      -> Worker {workerId} waiting for permit...");
                await limiter.WaitAsync(); // Waits for an open slot
                try
                {
                    Console.WriteLine($"         [ACTIVE] Worker {workerId} entered gate.");
                    await Task.Delay(80); // Simulating work
                }
                finally
                {
                    Console.WriteLine($"         [EXIT] Worker {workerId} leaving gate.");
                    limiter.Release(); // Releases slot for next waiting worker
                }
            }));
        }

        Task.WaitAll(activeWorkers.ToArray());

        // 4. Thread-Safe Concurrent Collections (ConcurrentDictionary & ConcurrentQueue)
        Console.WriteLine("\n[4] Concurrent Collections (ConcurrentDictionary & ConcurrentQueue):");
        var sharedCache = new ConcurrentDictionary<string, int>();
        var sharedQueue = new ConcurrentQueue<string>();

        Parallel.For(0, 100, i =>
        {
            sharedCache.AddOrUpdate($"Key_{i % 5}", 1, (k, existing) => existing + 1);
            sharedQueue.Enqueue($"Item_{i}");
        });

        Console.WriteLine($"    • ConcurrentDictionary Key_0 Aggregated Tally: {sharedCache["Key_0"]}");
        Console.WriteLine($"    • ConcurrentQueue Total Enqueued Items: {sharedQueue.Count}");
    }
}
