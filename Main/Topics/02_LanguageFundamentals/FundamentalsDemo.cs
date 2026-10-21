using System;
using System.Globalization;

namespace CSharpMastery.Topics.Topic02;

/// <summary>
/// Practical demonstrations for Topic 02: Language Fundamentals
/// </summary>
public static class FundamentalsDemo
{
    // A sample class to demonstrate Heap allocation and reference copying
    private class BankVault
    {
        public int GoldBars { get; set; }
    }

    /// <summary>
    /// Executes the practical learning module for Language Fundamentals.
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🧱 TOPIC 02: C# LANGUAGE FUNDAMENTALS IN ACTION");
        Console.WriteLine("=======================================================");

        // 1. Inferred Types with var (Compile-Time Strict Typing)
        var message = "Welcome to Modern .NET";
        var precision = 3.14159265;
        Console.WriteLine($"[1] Type Inference via 'var':");
        Console.WriteLine($"    • message is strictly typed as:   {message.GetType().FullName}");
        Console.WriteLine($"    • precision is strictly typed as: {precision.GetType().FullName}");

        // 2. The Photocopier vs The Vault: Value Types vs Reference Types
        Console.WriteLine($"\n[2] Memory Semantics (Value vs Reference):");
        
        // Value Type (Stack Copy)
        int originalNumber = 100;
        int copiedNumber = originalNumber;
        copiedNumber = 999;
        Console.WriteLine($"    • Value Type (Independent Stack copies):");
        Console.WriteLine($"      Original = {originalNumber}, Copy altered = {copiedNumber}");

        // Reference Type (Heap Reference / Shared Pointer)
        var vaultA = new BankVault { GoldBars = 50 };
        var vaultB = vaultA; // Copies the pointer address on the stack!
        vaultB.GoldBars = 20; // Mutates the underlying object on the Heap
        Console.WriteLine($"    • Reference Type (Shared Heap instance):");
        Console.WriteLine($"      vaultA.GoldBars = {vaultA.GoldBars} (Altered through vaultB reference!)");

        // 3. Widening & Narrowing Type Conversions
        Console.WriteLine($"\n[3] Type Conversions & Casting:");
        int smallInt = 42;
        double widenedDouble = smallInt; // Implicit widening: safe, no data loss
        double floatingValue = 9.999;
        int truncatedInt = (int)floatingValue; // Explicit narrowing: truncates decimals
        Console.WriteLine($"    • Implicit (int 42 -> double): {widenedDouble:F2}");
        Console.WriteLine($"    • Explicit cast (double 9.999 -> int): {truncatedInt} (Fractional part discarded)");

        // 4. The Silent Overflow Trap vs 'checked' blocks
        Console.WriteLine($"\n[4] Integer Overflow Protection:");
        int maxInt = int.MaxValue;
        int silentOverflow = maxInt + 1; // Unchecked: silently wraps around to negative!
        Console.WriteLine($"    • Unchecked int.MaxValue + 1: {silentOverflow} (SILENT OVERFLOW TRAP!)");

        try
        {
            checked
            {
                int protectedOverflow = maxInt + 1;
            }
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"    • 'checked' block intercepted overflow: {ex.GetType().Name} - Safety preserved!");
        }

        // 5. Robust Parsing via TryParse
        Console.WriteLine($"\n[5] Safe Input Parsing (int.TryParse):");
        string rawInput = "1048576";
        if (int.TryParse(rawInput, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedValue))
        {
            Console.WriteLine($"    • Successfully parsed '{rawInput}' into integer: {parsedValue:N0}");
        }
    }
}
