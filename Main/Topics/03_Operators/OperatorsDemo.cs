using System;

namespace CSharpMastery.Topics.Topic03;

/// <summary>
/// Practical demonstrations for Topic 03: Operators
/// </summary>
public static class OperatorsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 03: OPERATORS & TYPE TESTING");
        Console.WriteLine("=======================================================");

        // 1. Arithmetic & Precedence
        int x = 10, y = 3;
        Console.WriteLine($"Arithmetic: 10 / 3 = {x / y}, 10 % 3 = {x % y}");

        // 2. Short-Circuiting Logical Operators
        bool result = (x > 5) || (y++ > 10); // Left side is true -> right side is never evaluated
        Console.WriteLine($"Short-circuit test: result = {result}, y remained = {y}");

        // 3. Bitwise Operators
        int bitA = 0b0101; // 5
        int bitB = 0b0011; // 3
        Console.WriteLine($"Bitwise AND (5 & 3): {bitA & bitB}"); // 0001 = 1
        Console.WriteLine($"Bitwise XOR (5 ^ 3): {bitA ^ bitB}"); // 0110 = 6

        // 4. Null-Coalescing (?? and ??=)
        string? optionalName = null;
        string displayName = optionalName ?? "Default Guest";
        Console.WriteLine($"?? Coalesced Name: {displayName}");

        optionalName ??= "Kunal"; // Assigns only because it was null
        Console.WriteLine($"??= Assigned Name: {optionalName}");

        // 5. is, as, and typeof
        object objData = "Hello .NET 10";
        if (objData is string text) // Pattern matching 'is' with declaration
        {
            Console.WriteLine($"'is' matched: string of length {text.Length}");
        }

        object numBox = 123;
        string? asResult = numBox as string; // Safe cast -> yields null
        Console.WriteLine($"'as' safe cast to string: {(asResult == null ? "null (no exception thrown)" : asResult)}");
        Console.WriteLine($"typeof(int) is: {typeof(int).FullName}");
    }
}
