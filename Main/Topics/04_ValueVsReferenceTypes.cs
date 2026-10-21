using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 4: Value Types (struct) vs Reference Types (class)
/// - Classes live on the Heap (variables hold references).
/// - Structs live inline/Stack (variables hold copies of values).
/// </summary>
public static class ValueVsReferenceTypes
{
    // Reference Type (Class)
    public class PersonClass
    {
        public int Age { get; set; }
    }

    // Value Type (Struct)
    public struct PointStruct
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PointStruct(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 04. VALUE TYPES (struct) VS REFERENCE TYPES (class)");
        Console.WriteLine("==========================================");

        // 1. Reference Type Demo (Classes)
        Console.WriteLine("--- 1. Reference Type (Class) Demo ---");
        var person1 = new PersonClass { Age = 40 };
        var person2 = person1; // Copies the reference, not the object

        person2.Age = 45; // Mutating person2 affects person1!

        Console.WriteLine($"person1.Age: {person1.Age} (Modified through person2)");
        Console.WriteLine($"person2.Age: {person2.Age}");

        // 2. Value Type Demo (Structs)
        Console.WriteLine("\n--- 2. Value Type (Struct) Demo ---");
        var point1 = new PointStruct(10, 20);
        var point2 = point1; // Creates an independent copy of the data

        point2.X = 99; // Only point2 is modified; point1 remains unchanged

        Console.WriteLine($"point1.X: {point1.X} (Unchanged, remains 10)");
        Console.WriteLine($"point2.X: {point2.X} (Modified copy, is 99)");
    }
}
