using System;
using CSharpLearning;

namespace CSharpMastery.Topics.Topic25;

/// <summary>
/// Practical demonstrations for Topic 25: Unit Testing Concepts
/// </summary>
public static class UnitTestingDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 25: UNIT TESTING CONCEPTS & EXECUTION");
        Console.WriteLine("=======================================================");

        var calc = new Calculator();
        int sum = calc.Add(10, 25);
        Console.WriteLine($"Calculator.Add(10, 25) = {sum} (Target of xUnit test suite)");
        Console.WriteLine("\nRun the full xUnit test suite using the .NET CLI:");
        Console.WriteLine("  $ dotnet test");
        Console.WriteLine("  $ dotnet test --logger \"console;verbosity=normal\"");
    }
}
