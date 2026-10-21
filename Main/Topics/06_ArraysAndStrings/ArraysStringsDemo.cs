using System;
using System.Diagnostics;
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
        Console.WriteLine(" 🔠 TOPIC 06: ARRAYS, STRINGS & THE INTERN POOL");
        Console.WriteLine("=======================================================");

        // 1. Single-Dimensional Arrays & Array Algorithms
        Console.WriteLine("[1] Single-Dimensional Array Algorithms:");
        int[] sensorReadings = { 45, 12, 89, 34, 67, 23, 99, 5 };
        Array.Sort(sensorReadings);
        Console.WriteLine($"    • Sorted Readings: [{string.Join(", ", sensorReadings)}]");

        int searchTarget = 67;
        int targetIndex = Array.BinarySearch(sensorReadings, searchTarget);
        Console.WriteLine($"    • BinarySearch for {searchTarget}: Found at index {targetIndex}");

        int[] highReadings = Array.FindAll(sensorReadings, x => x > 50);
        Console.WriteLine($"    • High readings (> 50): [{string.Join(", ", highReadings)}]");

        // 2. Multidimensional (2D) vs. Jagged Arrays
        Console.WriteLine("\n[2] Rectangular vs. Jagged Arrays:");
        
        // Rectangular 2x3 contiguous matrix
        int[,] matrix = {
            { 1, 2, 3 },
            { 4, 5, 6 }
        };
        Console.WriteLine($"    • Rectangular Matrix [1, 2]: {matrix[1, 2]} (Total elements: {matrix.Length})");

        // Jagged array (Array of arrays with variable row lengths)
        int[][] jaggedNetwork = new int[3][];
        jaggedNetwork[0] = new int[] { 101, 102 };
        jaggedNetwork[1] = new int[] { 201, 202, 203, 204 };
        jaggedNetwork[2] = new int[] { 301 };
        Console.WriteLine($"    • Jagged Array: Row 0 has {jaggedNetwork[0].Length} nodes, Row 1 has {jaggedNetwork[1].Length} nodes");

        // 3. String Immutability vs. StringBuilder Performance
        Console.WriteLine("\n[3] String Immutability vs. StringBuilder:");
        
        // Demonstrating that string modifications allocate brand new objects
        string baseWord = "Architecture";
        string modifiedWord = baseWord.ToUpper();
        Console.WriteLine($"    • Original string untouched: '{baseWord}' vs '{modifiedWord}'");

        // Fast StringBuilder buffer
        var sb = new StringBuilder(capacity: 128);
        sb.Append("HTTP/1.1 200 OK\r\n");
        sb.Append("Content-Type: application/json\r\n");
        sb.Append("\r\n");
        sb.Append("{\"status\": \"healthy\"}");
        Console.WriteLine($"    • StringBuilder generated payload:\n{sb}");

        // 4. The CLR String Intern Pool
        Console.WriteLine("\n[4] The CLR String Intern Pool:");
        string literalA = "DotNetMastery";
        string literalB = "DotNetMastery";
        
        // Dynamically generated string (NOT automatically in the intern pool)
        string dynamicString = new StringBuilder().Append("DotNet").Append("Mastery").ToString();

        Console.WriteLine($"    • Literals share identical reference: {ReferenceEquals(literalA, literalB)} (Interned)");
        Console.WriteLine($"    • Dynamic string shares reference   : {ReferenceEquals(literalA, dynamicString)} (Separate heap object!)");

        // Interning dynamic string into the pool
        string internedDynamic = string.Intern(dynamicString);
        Console.WriteLine($"    • After string.Intern() reference   : {ReferenceEquals(literalA, internedDynamic)} (Now shares reference!)");

        // 5. String Comparisons: Ordinal vs. Cultural
        Console.WriteLine("\n[5] String Comparisons (Ordinal vs. Culture):");
        string routeA = "api/v1/users";
        string routeB = "API/V1/USERS";

        bool ordinalMatch = string.Equals(routeA, routeB, StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"    • Ordinal Case-Insensitive Route Match: {ordinalMatch} (Fastest for system identifiers)");

        // 6. Modern C# Slicing (Ranges and Indices)
        string sentence = "Modern C# is incredibly powerful!";
        string sliceMiddle = sentence[7..9]; // "C#"
        string lastWord = sentence[^9..^1];  // "powerful"
        Console.WriteLine($"\n[6] C# Range Slicing: sentence[7..9] = '{sliceMiddle}', sentence[^9..^1] = '{lastWord}'");
    }
}
