using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic11;

/// <summary>
/// Practical demonstrations for Topic 11: Collections and Data Structures
/// </summary>
public static class CollectionsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 11: COLLECTIONS & DATA STRUCTURES");
        Console.WriteLine("=======================================================");

        // 1. Dictionary for O(1) Key-Value lookup
        var userAges = new Dictionary<string, int>
        {
            ["Alice"] = 28,
            ["Bob"] = 34
        };
        if (userAges.TryGetValue("Alice", out int aliceAge))
        {
            Console.WriteLine($"Dictionary lookup: Alice is {aliceAge} years old.");
        }

        // 2. HashSet for distinct items
        var tags = new HashSet<string> { "csharp", "dotnet", "csharp", "azure" };
        Console.WriteLine($"HashSet Count: {tags.Count} (Duplicates automatically ignored)");

        // 3. Queue (FIFO)
        var printQueue = new Queue<string>();
        printQueue.Enqueue("Document1.pdf");
        printQueue.Enqueue("Invoice.pdf");
        Console.WriteLine($"Queue dequeued first: {printQueue.Dequeue()} (FIFO)");

        // 4. Stack (LIFO)
        var undoHistory = new Stack<string>();
        undoHistory.Push("Type 'Hello'");
        undoHistory.Push("Format Bold");
        Console.WriteLine($"Stack popped last action: {undoHistory.Pop()} (LIFO)");
    }
}
