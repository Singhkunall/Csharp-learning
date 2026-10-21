using System;

namespace CSharpMastery.Topics.Topic02;

/// <summary>
/// Practical demonstrations for Topic 02: Language Fundamentals
/// </summary>
public static class FundamentalsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 02: C# LANGUAGE FUNDAMENTALS");
        Console.WriteLine("=======================================================");

        // 1. Implicit typing with var
        var language = "C# 12";
        var versionNumber = 12.0;
        Console.WriteLine($"Inferred Types: language is {language.GetType().Name}, version is {versionNumber.GetType().Name}");

        // 2. Constants
        const double Pi = 3.1415926535;
        Console.WriteLine($"Constant Pi: {Pi}");

        // 3. Type Conversion & Casting
        int originalInt = 42;
        double implicitDouble = originalInt; // Implicit widening conversion
        double piApproximation = 3.9999;
        int explicitTruncated = (int)piApproximation; // Explicit narrowing conversion (truncates to 3)

        Console.WriteLine($"Implicit Cast (int -> double): {implicitDouble}");
        Console.WriteLine($"Explicit Cast (double -> int): {explicitTruncated} (truncated, not rounded)");

        // 4. Safe Parsing with int.TryParse
        string rawText = "1024";
        if (int.TryParse(rawText, out int parsedNumber))
        {
            Console.WriteLine($"Safe parse succeeded: {parsedNumber * 2}");
        }

        // 5. Value Types vs Reference Types Behavior
        int valA = 10;
        int valB = valA;
        valB = 20; // Does NOT change valA
        Console.WriteLine($"Value Type Copy: valA = {valA}, valB = {valB}");

        int[] arrA = { 1, 2, 3 };
        int[] arrB = arrA;
        arrB[0] = 999; // Modifies arrA[0] as both point to the same array on the Heap
        Console.WriteLine($"Reference Type Copy: arrA[0] = {arrA[0]}, arrB[0] = {arrB[0]} (Shared memory)");
    }
}
