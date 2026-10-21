using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpMastery.Topics.Topic18;

/// <summary>
/// Practical demonstrations for Topic 18: Asynchronous Programming (async / await)
/// </summary>
public static class AsyncDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" ⚡ TOPIC 18: ASYNCHRONOUS PROGRAMMING (async / await)");
        Console.WriteLine("=======================================================");

        // 1. Thread-Yielding Proof: Inspecting Thread IDs across await
        Console.WriteLine("[1] Thread Yielding Verification (Inspecting Thread IDs):");
        int threadBefore = Environment.CurrentManagedThreadId;
        Console.WriteLine($"    • Thread ID BEFORE await: {threadBefore} (Initiating 150ms I/O delay)");
        
        await Task.Delay(150); // Yields thread back to ThreadPool
        
        int threadAfter = Environment.CurrentManagedThreadId;
        Console.WriteLine($"    • Thread ID AFTER await : {threadAfter} (Resumed on available thread from pool!)");

        // 2. Concurrency: Task.WhenAll vs. Task.WhenAny
        Console.WriteLine("\n[2] Fan-Out / Fan-In Concurrency (Task.WhenAll & WhenAny):");
        var sw = Stopwatch.StartNew();

        var taskA = SimulateNetworkFetchAsync("Service Alpha (Fast)", 100);
        var taskB = SimulateNetworkFetchAsync("Service Beta (Slow)", 250);
        var taskC = SimulateNetworkFetchAsync("Service Gamma (Medium)", 150);

        // WhenAny: Returns as soon as the first task finishes
        Task<string> firstCompleted = await Task.WhenAny(taskA, taskB, taskC);
        Console.WriteLine($"    • Task.WhenAny fastest responder: '{await firstCompleted}' in {sw.ElapsedMilliseconds} ms");

        // WhenAll: Waits for all remaining tasks
        string[] allPayloads = await Task.WhenAll(taskA, taskB, taskC);
        Console.WriteLine($"    • Task.WhenAll all completed in {sw.ElapsedMilliseconds} ms (Ran concurrently, not serially!)");

        // 3. Cooperative Cancellation with CancellationToken
        Console.WriteLine("\n[3] Cooperative Cancellation (CancellationTokenSource):");
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(80); // Cancels after 80ms

        try
        {
            Console.WriteLine("    • Initiating long-running transfer (timeout at 80ms, operation requires 300ms)...");
            await Task.Delay(300, cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("    • Intercepted OperationCanceledException: Token gracefully halted worker thread!");
        }

        // 4. Zero-Allocation Optimization with ValueTask<T>
        Console.WriteLine("\n[4] Zero-Allocation Cached Optimization via ValueTask<T>:");
        Console.WriteLine($"    • Call 1 (Fresh):  {await QueryUserCachedAsync(101)}");
        Console.WriteLine($"    • Call 2 (Cached): {await QueryUserCachedAsync(101)} (Returned synchronously with 0 heap allocation!)");

        // 5. Async Streams (IAsyncEnumerable<T> & await foreach)
        Console.WriteLine("\n[5] Async Streams (IAsyncEnumerable<T> and 'await foreach'):");
        Console.Write("    • Streaming real-time sensor packets: ");
        await foreach (var packet in StreamSensorTelemetryAsync())
        {
            Console.Write($"[{packet}] ");
        }
        Console.WriteLine();
    }

    private static async Task<string> SimulateNetworkFetchAsync(string name, int delayMs)
    {
        await Task.Delay(delayMs);
        return $"{name} -> Payload OK";
    }

    // In-memory cache simulating fast synchronous path
    private static readonly Dictionary<int, string> _memoryCache = new();

    private static ValueTask<string> QueryUserCachedAsync(int userId)
    {
        // FAST PATH: Return immediately from cache without allocating Task on heap!
        if (_memoryCache.TryGetValue(userId, out string? cached))
        {
            return new ValueTask<string>($"[CACHED] {cached}");
        }

        // SLOW ASYNC PATH: Delegate to Task
        return new ValueTask<string>(FetchUserFromDatabaseAsync(userId));
    }

    private static async Task<string> FetchUserFromDatabaseAsync(int userId)
    {
        await Task.Delay(50); // Simulate DB query
        string user = $"User_{userId}_Profile";
        _memoryCache[userId] = user;
        return $"[FETCHED] {user}";
    }

    // Real-time asynchronous streaming
    private static async IAsyncEnumerable<string> StreamSensorTelemetryAsync()
    {
        for (int i = 1; i <= 3; i++)
        {
            await Task.Delay(60); // Asynchronously waiting for next hardware packet
            yield return $"Packet_{i} (Temp {20 + i}°C)";
        }
    }
}
