using System;
using System.Text;

namespace CSharpMastery.Topics.Topic06;

/// <summary>
/// Practical demonstrations for Topic 06: Arrays and Strings
/// </summary>
public static class ArraysStringsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 06: ARRAYS, STRINGS & STRINGBUILDER");
        Console.WriteLine("=======================================================");

        // 1. Single & Jagged Arrays
        int[] scores = { 90, 45, 78, 60, 99 };
        Array.Sort(scores);
        Console.WriteLine($"Sorted array: [{string.Join(", ", scores)}]");

        int[][] jagged = new int[2][];
        jagged[0] = new int[] { 1, 2 };
        jagged[1] = new int[] { 10, 20, 30, 40 };
        Console.WriteLine($"Jagged array row 1 length: {jagged[1].Length}");

        // 2. String Immutability vs StringBuilder
        var sb = new StringBuilder("Initial text");
        for (int i = 1; i <= 3; i++)
        {
            sb.Append($" -> Chunk {i}");
        }
        Console.WriteLine($"StringBuilder output: {sb}");

        // 3. Verbatim and Raw String Literals (C# 11+)
        string jsonRaw = """
        {
            "name": "Kunal",
            "role": "Developer"
        }
        """;
        Console.WriteLine($"Raw String Literal JSON:\n{jsonRaw}");

        // 4. String comparison (Ordinal vs Culture)
        string a = "apple", b = "Apple";
        bool equalOrdinal = string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"Case-insensitive string comparison: {equalOrdinal}");
    }
}
