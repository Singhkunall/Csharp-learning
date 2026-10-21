using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic04;

/// <summary>
/// Practical demonstrations for Topic 04: Decision-Making, Loops & Pattern Matching
/// </summary>
public static class LoopsDemo
{
    // Record for demonstrating modern Property Pattern Matching
    public record Parcel(double WeightKg, bool IsFragile, string Destination, int PriorityLevel);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🔀 TOPIC 04: DECISION-MAKING, LOOPS & PATTERN MATCHING");
        Console.WriteLine("=======================================================");

        // 1. Modern Pattern Matching with Switch Expressions
        Console.WriteLine("[1] Modern Pattern-Matching Switch Expressions:");
        var parcels = new List<Parcel>
        {
            new(65.0, true, "Chicago", 1),
            new(0.8, false, "NYC", 2),
            new(15.0, false, "Dallas", 3),
            new(120.0, false, "Seattle", 1)
        };

        foreach (var parcel in parcels)
        {
            string route = ClassifyParcel(parcel);
            Console.WriteLine($"    • {parcel.WeightKg}kg to {parcel.Destination} (Fragile: {parcel.IsFragile}) -> {route}");
        }

        // 2. The Four Loop Types in Action
        Console.WriteLine("\n[2] The Four Loop Strategies:");

        // A. for loop: Known iteration count
        Console.Write("    • for loop (countdown): ");
        for (int i = 3; i >= 1; i--)
        {
            Console.Write($"{i}... ");
        }
        Console.WriteLine("Launch!");

        // B. while loop: Continuous sensor condition
        int fuelLevel = 3;
        Console.Write("    • while loop (consuming fuel): ");
        while (fuelLevel > 0)
        {
            Console.Write($"[Fuel {fuelLevel}%] ");
            fuelLevel--;
        }
        Console.WriteLine("[Empty]");

        // C. do-while loop: Guaranteed first execution
        int attempt = 0;
        Console.Write("    • do-while loop (Security badge swipe): ");
        do
        {
            attempt++;
            Console.Write($"Swipe #{attempt} ");
        } while (attempt < 1); // Exits immediately after 1 iteration
        Console.WriteLine("[Access Granted]");

        // D. foreach loop: Clean iteration over collections
        string[] protocols = { "HTTPS", "gRPC", "WebSocket" };
        Console.Write("    • foreach loop (protocols): ");
        foreach (var protocol in protocols)
        {
            Console.Write($"<{protocol}> ");
        }
        Console.WriteLine();

        // 3. Loop Control (break, continue) and Nested 2D Grid
        Console.WriteLine("\n[3] Loop Control (break & continue in 2D Search):");
        int[,] grid = {
            { 1, 2, 3 },
            { 4, 99, 6 }, // 99 is our target
            { 7, 8, 9 }
        };

        bool targetFound = false;
        int targetRow = -1, targetCol = -1;

        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] % 2 != 0 && grid[r, c] != 99)
                {
                    continue; // Skip odd numbers that are not our target
                }

                if (grid[r, c] == 99)
                {
                    targetRow = r;
                    targetCol = c;
                    targetFound = true;
                    break; // Break out of inner loop
                }
            }
            if (targetFound) break; // Break out of outer loop
        }
        Console.WriteLine($"    • Target 99 located at Grid [{targetRow}, {targetCol}] via coordinated break.");

        // 4. The Collection Modification Trap Solution
        Console.WriteLine("\n[4] Safe Collection Pruning (Avoiding InvalidOperationException):");
        var workItems = new List<int> { 10, 25, 30, 45, 50, 65 };
        
        // Unsafe in foreach! Instead, iterate backwards with a for loop:
        for (int i = workItems.Count - 1; i >= 0; i--)
        {
            if (workItems[i] < 40)
            {
                workItems.RemoveAt(i); // Safe removal!
            }
        }
        Console.WriteLine($"    • Cleaned items (>= 40 only): [{string.Join(", ", workItems)}]");
    }

    private static string ClassifyParcel(Parcel p) => p switch
    {
        // Property pattern with relational check
        { WeightKg: > 50.0, IsFragile: true } => "Heavy Fragile Team",
        // Relational check with logical 'or' pattern on destination
        { WeightKg: < 1.0, Destination: "NYC" or "BOS" } => "Express Air Shuttle",
        // Relational check with high priority
        { WeightKg: > 100.0, PriorityLevel: 1 } => "Freight Priority Team",
        // Discard fallback
        _ => "Standard Ground Processing"
    };
}
