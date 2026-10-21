using System;
using System.Collections.Generic;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 9: Generics and Collections
/// Theory: Generics allow code reusability without sacrificing type safety or incurring boxing/unboxing overhead.
/// </summary>
public static class GenericsAndCollections
{
    // Generic Class with Constraint (where T : class)
    public class Repository<T> where T : class
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);

        public IReadOnlyList<T> GetAll() => _items.AsReadOnly();
    }

    // Generic Method with multiple type parameters
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 09. GENERICS & COLLECTIONS");
        Console.WriteLine("==========================================");

        // 1. Generic Method (Swap)
        int x = 10, y = 20;
        Console.WriteLine($"Before Swap: x = {x}, y = {y}");
        Swap(ref x, ref y);
        Console.WriteLine($"After Swap:  x = {x}, y = {y}");

        string first = "World", second = "Hello";
        Swap(ref first, ref second);
        Console.WriteLine($"Swapped Strings: {first} {second}");

        // 2. Generic Collections: List, Dictionary, HashSet
        Console.WriteLine("\n--- Generic Collections ---");
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        numbers.Add(6);
        Console.WriteLine($"List count: {numbers.Count}, First: {numbers[0]}");

        var studentGrades = new Dictionary<string, int>
        {
            ["Alice"] = 95,
            ["Bob"] = 82,
            ["Charlie"] = 78
        };
        Console.WriteLine($"Alice's Grade: {studentGrades["Alice"]}");

        var uniqueTags = new HashSet<string> { "csharp", "dotnet", "csharp" };
        Console.WriteLine($"Unique tags in HashSet: {uniqueTags.Count} (duplicates automatically ignored)");

        // 3. Custom Generic Class
        var userRepo = new Repository<string>();
        userRepo.Add("Admin");
        userRepo.Add("Manager");
        Console.WriteLine($"Repository holds: {string.Join(", ", userRepo.GetAll())}");
    }
}
