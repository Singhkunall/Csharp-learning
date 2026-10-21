using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic11;

/// <summary>
/// Practical demonstrations for Topic 11: Collections & Data Structures
/// </summary>
public static class CollectionsDemo
{
    // Custom entity for sorting demonstration
    public record ServerNode(string Name, int LatencyMs, double LoadFactor);

    // Custom IComparer for priority queue sorting
    public class LatencyComparer : IComparer<ServerNode>
    {
        public int Compare(ServerNode? x, ServerNode? y)
        {
            if (x is null || y is null) return 0;
            return x.LatencyMs.CompareTo(y.LatencyMs);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 📦 TOPIC 11: COLLECTIONS & DATA STRUCTURES IN ACTION");
        Console.WriteLine("=======================================================");

        // 1. The Obsolete ArrayList vs. Modern Generic List<T>
        Console.WriteLine("[1] The Dead ArrayList vs. Modern Generic List<T>:");
        
        // Old non-generic ArrayList: accepts everything, requires runtime unboxing/casts
        ArrayList legacyList = new ArrayList();
        legacyList.Add(42);         // BOXED to object
        legacyList.Add("Not a number"); // Danger! Type safety broken!
        int safeFirstItem = (int)legacyList[0]!; // Unboxed with explicit cast
        Console.WriteLine($"    • Legacy ArrayList unboxed item: {safeFirstItem} (Risk of runtime InvalidCastException)");

        // Modern List<T>: Compile-time strict, zero-boxing
        var typedList = new List<int>(capacity: 4) { 10, 20, 30 };
        Console.WriteLine($"    • List<T> Count = {typedList.Count}, Capacity = {typedList.Capacity}");
        typedList.Add(40);
        typedList.Add(50); // Forces internal array reallocation (capacity doubles to 8)
        Console.WriteLine($"    • After exceeding capacity: Count = {typedList.Count}, Capacity = {typedList.Capacity} (Doubled!)");

        // 2. Dictionary<TKey, TValue> High-Speed O(1) Lookup
        Console.WriteLine("\n[2] Dictionary<TKey, TValue> (O(1) Hash Table):");
        var userRegistry = new Dictionary<string, string>
        {
            ["usr_101"] = "Alice (Security Admin)",
            ["usr_102"] = "Bob (Database Architect)"
        };

        // Always use TryGetValue to avoid KeyNotFoundException:
        if (userRegistry.TryGetValue("usr_101", out string? role))
        {
            Console.WriteLine($"    • Found usr_101 via TryGetValue: {role}");
        }

        // 3. HashSet<T> vs. SortedSet<T> (Set Mathematics)
        Console.WriteLine("\n[3] HashSet<T> vs. SortedSet<T> (Set Operations):");
        var setA = new HashSet<string> { "C#", "Go", "Rust", "Python" };
        var setB = new HashSet<string> { "Rust", "Python", "TypeScript", "Kotlin" };

        var intersection = new HashSet<string>(setA);
        intersection.IntersectWith(setB);
        Console.WriteLine($"    • Intersection (Languages in both): [{string.Join(", ", intersection)}]");

        var difference = new HashSet<string>(setA);
        difference.ExceptWith(setB);
        Console.WriteLine($"    • Difference (In A but not B):       [{string.Join(", ", difference)}]");

        // SortedSet maintains automatic Red-Black binary tree sorting:
        var sortedScores = new SortedSet<int> { 98, 45, 82, 100, 12, 67 };
        Console.WriteLine($"    • SortedSet automatic tree ordering: [{string.Join(" < ", sortedScores)}]");

        // 4. Queue (FIFO) vs. Stack (LIFO)
        Console.WriteLine("\n[4] Queue<T> (FIFO) vs. Stack<T> (LIFO):");
        var taskQueue = new Queue<string>();
        taskQueue.Enqueue("Task 1: Process Payment");
        taskQueue.Enqueue("Task 2: Send Invoice Email");
        Console.WriteLine($"    • Queue Dequeue (FIFO):  {taskQueue.Dequeue()}");

        var undoStack = new Stack<string>();
        undoStack.Push("State 1: Initial Draft");
        undoStack.Push("State 2: Applied Watermark");
        Console.WriteLine($"    • Stack Pop (LIFO):      {undoStack.Pop()} (Undid latest state)");

        // 5. LinkedList<T> (O(1) In-Place Insertion)
        Console.WriteLine("\n[5] LinkedList<T> Node Pointer Manipulations:");
        var playlist = new LinkedList<string>();
        var firstNode = playlist.AddFirst("Track 01 - Intro");
        var lastNode = playlist.AddLast("Track 03 - Finale");

        // Inserting between nodes in O(1) time without shifting arrays:
        playlist.AddAfter(firstNode, "Track 02 - Interlude (Inserted O(1))");
        foreach (var track in playlist)
        {
            Console.WriteLine($"    • Playing: {track}");
        }

        // 6. Custom IComparer<T> with Priority Sorting
        Console.WriteLine("\n[6] Custom IComparer<T> Sorting (Fastest Ping First):");
        var servers = new List<ServerNode>
        {
            new("Frankfurt-01", 38, 0.65),
            new("Virginia-02", 112, 0.30),
            new("Mumbai-01", 14, 0.82),
            new("Tokyo-03", 85, 0.40)
        };

        servers.Sort(new LatencyComparer());
        foreach (var s in servers)
        {
            Console.WriteLine($"    • {s.Name,-15} Ping: {s.LatencyMs,3} ms | Load: {s.LoadFactor:P0}");
        }
    }
}
