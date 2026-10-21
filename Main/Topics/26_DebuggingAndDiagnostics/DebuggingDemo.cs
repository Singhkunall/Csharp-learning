using System;
using System.Diagnostics;

namespace CSharpMastery.Topics.Topic26;

/// <summary>
/// Practical demonstrations for Topic 26: Debugging & Diagnostics
/// </summary>
public static class DebuggingDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 26: DEBUGGING, DIAGNOSTICS & STOPWATCH TIMING");
        Console.WriteLine("=======================================================");

        // 1. High-precision performance measurement via Stopwatch
        var sw = Stopwatch.StartNew();

        int accumulator = 0;
        for (int i = 0; i < 100_000; i++)
        {
            accumulator += (i % 2 == 0) ? 1 : -1;
        }

        sw.Stop();
        Console.WriteLine($"Executed 100,000 iterations in {sw.Elapsed.TotalMilliseconds:F3} ms. Result = {accumulator}");

        // 2. Programmatic StackTrace inspection
        var stackTrace = new StackTrace(true);
        Console.WriteLine($"Current Call Stack Frame Count: {stackTrace.FrameCount}");
        Console.WriteLine($"Executing Method: {stackTrace.GetFrame(0)?.GetMethod()?.Name}");
    }
}
