using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CSharpMastery.Topics.Topic01;

/// <summary>
/// Practical demonstrations for Topic 01: Introduction to C# and .NET Runtime
/// </summary>
public static class IntroDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🌐 TOPIC 01: THE .NET RUNTIME & CLR ARCHITECTURE");
        Console.WriteLine("=======================================================");

        // 1. Environment and Runtime Inspection
        Console.WriteLine($"[1] Platform & Runtime Information:");
        Console.WriteLine($"    • .NET Core CLR Version:   {Environment.Version}");
        Console.WriteLine($"    • OS Platform:             {RuntimeInformation.OSDescription}");
        Console.WriteLine($"    • OS Architecture:         {RuntimeInformation.OSArchitecture}");
        Console.WriteLine($"    • Process Architecture:    {RuntimeInformation.ProcessArchitecture}");
        Console.WriteLine($"    • Framework Identity:      {RuntimeInformation.FrameworkDescription}");
        Console.WriteLine($"    • Logical Processor Cores: {Environment.ProcessorCount}");
        Console.WriteLine($"    • System Page Size:        {Environment.SystemPageSize} bytes");

        // 2. Garbage Collection & Memory Configuration
        Console.WriteLine($"\n[2] CLR Memory Subsystem:");
        Console.WriteLine($"    • Server GC Mode Enabled:  {System.Runtime.GCSettings.IsServerGC}");
        Console.WriteLine($"    • Latency Mode:            {System.Runtime.GCSettings.LatencyMode}");
        Console.WriteLine($"    • Supported GC Generations:{GC.MaxGeneration + 1} (Gen 0, Gen 1, Gen 2)");

        // 3. Assembly Reflection & Metadata Tokens
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        Console.WriteLine($"\n[3] Assembly Metadata & IL Container:");
        Console.WriteLine($"    • Full Assembly Name:      {currentAssembly.FullName}");
        Console.WriteLine($"    • Entry Point Method:      {currentAssembly.EntryPoint?.Name ?? "Top-Level Statements"}");
        Console.WriteLine($"    • Compiled Target Arch:    {RuntimeInformation.ProcessArchitecture}");

        // 4. JIT Optimization Status
        bool isJitOptimized = !currentAssembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false).Length.Equals(0);
        Console.WriteLine($"    • JIT Debuggable/Tracking: {isJitOptimized}");
    }
}
