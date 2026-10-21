using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 1: Variables, Data Types, and Console Output
/// </summary>
public static class BasicsAndDataTypes
{
    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 01. BASICS & DATA TYPES");
        Console.WriteLine("==========================================");

        // String variable
        string name = "GFG";
        Console.WriteLine($"String: {name}");

        // Boolean variables
        bool isCsharpEasy = true;
        bool isCompleted = false;
        Console.WriteLine($"Is C# easy: {isCsharpEasy}");
        Console.WriteLine($"Is Completed: {isCompleted}");

        // Numeric types
        int count = 10;
        double price = 99.99;
        Console.WriteLine($"Int: {count}, Double: {price}");
    }
}
