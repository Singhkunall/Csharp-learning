using System;

namespace CSharpMastery.Topics.Topic05;

/// <summary>
/// Practical demonstrations for Topic 05: Methods
/// </summary>
public static class MethodsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 05: METHODS & PARAMETER MODIFIERS");
        Console.WriteLine("=======================================================");

        // 1. ref and out parameter demonstration
        int original = 50;
        ModifyWithRef(ref original);
        Console.WriteLine($"After ref modification: original = {original}");

        DivideWithOut(27, 4, out int quotient, out int remainder);
        Console.WriteLine($"27 / 4 via out: Quotient = {quotient}, Remainder = {remainder}");

        // 2. in modifier (Read-only reference)
        int readOnlyTarget = 100;
        InspectWithIn(in readOnlyTarget);

        // 3. params keyword
        int sumTotal = CalculateSum(10, 20, 30, 40, 50);
        Console.WriteLine($"params Sum: {sumTotal}");

        // 4. Local function with recursion
        int fact5 = Factorial(5);
        Console.WriteLine($"Factorial of 5: {fact5}");

        static int Factorial(int n)
        {
            return n <= 1 ? 1 : n * Factorial(n - 1);
        }
    }

    private static void ModifyWithRef(ref int value) => value += 100;

    private static void DivideWithOut(int dividend, int divisor, out int quotient, out int remainder)
    {
        quotient = dividend / divisor;
        remainder = dividend % divisor;
    }

    private static void InspectWithIn(in int value)
    {
        // value += 1; // Error: Cannot assign to variable because it is a readonly variable
        Console.WriteLine($"Inspected readonly value: {value}");
    }

    private static int CalculateSum(params int[] numbers)
    {
        int total = 0;
        foreach (var n in numbers) total += n;
        return total;
    }
}
