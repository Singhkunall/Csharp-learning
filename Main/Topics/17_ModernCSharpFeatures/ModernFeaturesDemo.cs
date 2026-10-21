using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic17;

// Extension Method Definition
public static class StringExtensions
{
    public static string ToTitleCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        return char.ToUpper(input[0]) + input[1..].ToLower();
    }
}

/// <summary>
/// Practical demonstrations for Topic 17: Modern C# Features
/// </summary>
public static class ModernFeaturesDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 17: MODERN C# FEATURES (C# 10, 11, 12)");
        Console.WriteLine("=======================================================");

        // 1. C# 12 Collection Expressions
        int[] numbers = [10, 20, 30, 40];
        List<string> frameworks = ["ASP.NET", "EF Core", "MAUI"];
        Console.WriteLine($"Collection Expression count: {frameworks.Count}");

        // 2. Extension Method Call
        string rawWord = "kUnAl";
        Console.WriteLine($"Extension method result: '{rawWord.ToTitleCase()}'");

        // 3. nameof, typeof, default
        int defaultVal = default;
        Console.WriteLine($"nameof operator: {nameof(numbers)}, default(int): {defaultVal}");

        // 4. yield return iterator
        Console.Write("Yield return Fibonacci generator: ");
        foreach (var num in GenerateFibonacci(7))
        {
            Console.Write($"{num} ");
        }
        Console.WriteLine();
    }

    private static IEnumerable<int> GenerateFibonacci(int count)
    {
        int a = 0, b = 1;
        for (int i = 0; i < count; i++)
        {
            yield return a;
            int temp = a + b;
            a = b;
            b = temp;
        }
    }
}
