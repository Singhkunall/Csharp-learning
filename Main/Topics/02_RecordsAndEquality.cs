using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 2: Records and Value Equality vs Reference Equality
/// In C#, records provide value-based equality out of the box,
/// meaning two instances with identical values are considered equal.
/// </summary>
public static class RecordsAndEquality
{
    public record Person(string Name, int Age);

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 02. RECORDS & EQUALITY");
        Console.WriteLine("==========================================");

        var p1 = new Person("Alice", 30);
        var p2 = new Person("Alice", 30);

        // Value Equality: Records compare data properties, not memory addresses
        Console.WriteLine($"p1 == p2:               {p1 == p2}");               // True
        Console.WriteLine($"p1.Equals(p2):         {p1.Equals(p2)}");          // True

        // Reference Equality: Checks if they point to the exact same object in memory
        Console.WriteLine($"ReferenceEquals(p1, p2): {ReferenceEquals(p1, p2)}"); // False (different instances)
    }
}
