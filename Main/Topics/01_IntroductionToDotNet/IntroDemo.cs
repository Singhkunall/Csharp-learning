using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CSharpMastery.Topics.Topic01;

/// <summary>
/// Practical demonstrations for Topic 01: Introduction to C# and .NET
/// </summary>
public static class IntroDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 01: INTRODUCTION TO C# AND .NET RUNTIME");
        Console.WriteLine("=======================================================");

        // Runtime diagnostics & Environment
        Console.WriteLine($"• .NET Version:       {Environment.Version}");
        Console.WriteLine($"• OS Description:     {RuntimeInformation.OSDescription}");
        Console.WriteLine($"• OS Architecture:    {RuntimeInformation.OSArchitecture}");
        Console.WriteLine($"• Process Arch:       {RuntimeInformation.ProcessArchitecture}");
        Console.WriteLine($"• Framework:          {RuntimeInformation.FrameworkDescription}");
        Console.WriteLine($"• Machine Core Count: {Environment.ProcessorCount}");
        Console.WriteLine($"• Assembly Location:  {Assembly.GetExecutingAssembly().Location}");
    }
}
