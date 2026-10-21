using System;
using CSharpLearning;

namespace CSharpMastery.Topics.Topic25;

/// <summary>
/// Practical demonstrations for Topic 25: Automated Testing with xUnit
/// </summary>
public static class UnitTestingDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🧪 TOPIC 25: AUTOMATED TESTING WITH XUNIT");
        Console.WriteLine("=======================================================");

        var calculator = new Calculator();

        // 1. Simulating the AAA (Arrange, Act, Assert) Pattern
        Console.WriteLine("[1] The AAA (Arrange - Act - Assert) Execution Pipeline:");
        
        // Arrange
        int a = 15, b = 25;
        int expectedSum = 40;
        Console.WriteLine($"    • 1. Arrange: Input A = {a}, Input B = {b}, Expected = {expectedSum}");

        // Act
        int actualResult = calculator.Add(a, b);
        Console.WriteLine($"    • 2. Act    : calculator.Add({a}, {b}) executed -> Returned {actualResult}");

        // Assert
        bool passed = (actualResult == expectedSum);
        Console.WriteLine($"    • 3. Assert : (actual == expected) -> {(passed ? "✅ GREEN (PASSED)" : "❌ RED (FAILED)")}");

        // 2. Exception Boundary Testing
        Console.WriteLine("\n[2] Exception Assertion (Assert.Throws<TException>):");
        try
        {
            calculator.Divide(10, 0);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("    • Assert.Throws intercepted DivideByZeroException: Boundary condition validated safe!");
        }

        // 3. Command Line Testing Instructions
        Console.WriteLine("\n[3] Automated xUnit CLI Suite:");
        Console.WriteLine("    Your solution includes a dedicated automated test project: 'main.Tests'.");
        Console.WriteLine("    To execute all unit tests with full telemetry:");
        Console.WriteLine("      $ dotnet test");
        Console.WriteLine("      $ dotnet test --logger \"console;verbosity=normal\"");
    }
}
