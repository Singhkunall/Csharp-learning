using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpMastery.Topics.Topic18;

/// <summary>
/// Practical demonstrations for Topic 18: Asynchronous Programming
/// </summary>
public static class AsyncDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 18: ASYNCHRONOUS PROGRAMMING (async / await)");
        Console.WriteLine("=======================================================");

        // 1. Concurrent task execution with Task.WhenAll
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId}] Starting concurrent downloads...");
        var downloadA = DownloadPayloadAsync("Service Alpha", 200);
        var downloadB = DownloadPayloadAsync("Service Beta", 150);

        string[] results = await Task.WhenAll(downloadA, downloadB);
        foreach (var res in results)
        {
            Console.WriteLine($"-> Received: {res}");
        }

        // 2. Cancellation Token usage
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(50); // Will trigger cancellation after 50ms

        try
        {
            Console.WriteLine("Starting cancellable task (timeout 50ms, required 200ms)...");
            await Task.Delay(200, cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("CancellationToken correctly stopped the long-running operation!");
        }
    }

    private static async Task<string> DownloadPayloadAsync(string serviceName, int delayMs)
    {
        await Task.Delay(delayMs); // Non-blocking async delay
        return $"{serviceName} payload ({delayMs}ms)";
    }
}
