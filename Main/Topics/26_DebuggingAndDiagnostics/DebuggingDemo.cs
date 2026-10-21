using System;
using System.Diagnostics;
using System.Threading;

namespace CSharpMastery.Topics.Topic26;

/// <summary>
/// Practical demonstrations for Topic 26: Debugging, Diagnostics & Profiling
/// </summary>
public static class DebuggingDemo
{
    // Simulating a memory leak via a static GC Root
    private static readonly System.Collections.Generic.List<byte[]> _leakedHeapMemory = new();

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🩺 TOPIC 26: DEBUGGING, DIAGNOSTICS & TELEMETRY");
        Console.WriteLine("=======================================================");

        // 1. High-Precision Timing with System.Diagnostics.Stopwatch
        Console.WriteLine("[1] Microsecond Performance Profiling (Stopwatch):");
        var sw = Stopwatch.StartNew();

        int sum = 0;
        for (int i = 0; i < 250_000; i++)
        {
            sum += i;
        }

        sw.Stop();
        Console.WriteLine($"    • Completed 250,000 iterations in {sw.Elapsed.TotalMilliseconds:F3} ms (Ticks: {sw.ElapsedTicks:N0})");

        // 2. Programmatic Call Stack Inspection
        Console.WriteLine("\n[2] Programmatic Call Stack Inspection (StackFrame Breadcrumbs):");
        SimulateNestedWorkflowLevel1();

        // 3. Diagnosing Memory Leaks: The Static Root Trap
        Console.WriteLine("\n[3] Memory Leak Diagnosis (The Static GC Root Trap):");
        long heapBeforeLeak = GC.GetTotalMemory(forceFullCollection: false);

        // Pinning memory in a static collection (Cannot be reclaimed by GC!)
        for (int i = 0; i < 5; i++)
        {
            _leakedHeapMemory.Add(new byte[100_000]); // 100 KB chunks pinned
        }

        long heapAfterLeak = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"    • Heap Memory Before: {heapBeforeLeak:N0} bytes");
        Console.WriteLine($"    • Heap Memory After : {heapAfterLeak:N0} bytes (+{heapAfterLeak - heapBeforeLeak:N0} bytes pinned by static root!)");

        // Cleaning the static root allows GC reclamation:
        _leakedHeapMemory.Clear();
        Console.WriteLine("    • Static reference cleared; memory released for next GC collection sweep.");

        // 4. Production CLI Profiler Tool Commands
        Console.WriteLine("\n[4] Production Diagnostic CLI Commands:");
        Console.WriteLine("    • dotnet-counters monitor --process-id <PID> (Live GC & CPU dashboard)");
        Console.WriteLine("    • dotnet-trace collect --process-id <PID>    (CPU sampling profiling)");
        Console.WriteLine("    • dotnet-dump collect --process-id <PID>     (Full memory crash dump)");
    }

    private static void SimulateNestedWorkflowLevel1() => SimulateNestedWorkflowLevel2();

    private static void SimulateNestedWorkflowLevel2()
    {
        var trace = new StackTrace(fNeedFileInfo: true);
        Console.WriteLine($"    • Active Stack Frame Count: {trace.FrameCount}");
        
        for (int i = 0; i < Math.Min(3, trace.FrameCount); i++)
        {
            StackFrame? frame = trace.GetFrame(i);
            Console.WriteLine($"      Frame #{i}: {frame?.GetMethod()?.DeclaringType?.Name}.{frame?.GetMethod()?.Name}()");
        }
    }
}
