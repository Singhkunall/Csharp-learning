using System;
using System.Threading.Tasks;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 11: Asynchronous Programming with async, await, and Task
/// Theory: Frees up the calling thread (e.g. UI or Web request thread) during I/O operations.
/// </summary>
public static class AsyncAwaitDemo
{
    public static async Task RunAsync()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 11. ASYNCHRONOUS PROGRAMMING (async / await)");
        Console.WriteLine("==========================================");

        Console.WriteLine("[Thread Info] Starting asynchronous simulation...");

        // Simulate non-blocking I/O operations running concurrently
        var task1 = FetchDataAsync("Database Records", 500);
        var task2 = FetchDataAsync("External API Payload", 300);

        // Wait for both to complete
        string[] results = await Task.WhenAll(task1, task2);

        foreach (var result in results)
        {
            Console.WriteLine($"Result: {result}");
        }

        Console.WriteLine("[Thread Info] All async operations completed successfully.");
    }

    private static async Task<string> FetchDataAsync(string sourceName, int delayMs)
    {
        Console.WriteLine($"-> Initiating fetch from '{sourceName}' (simulated {delayMs}ms delay)...");
        await Task.Delay(delayMs); // Non-blocking asynchronous delay
        return $"[Loaded] {sourceName} after {delayMs}ms";
    }
}
