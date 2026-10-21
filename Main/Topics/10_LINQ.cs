using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 10: LINQ (Language Integrated Query)
/// Theory: Declarative querying for in-memory collections, databases, and XML.
/// Key Feature: Deferred Execution (queries run only when iterated, e.g. foreach, ToList()).
/// </summary>
public static class LINQDemo
{
    public record Developer(string Name, string Language, int YearsExperience, double Salary);

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 10. LINQ (LANGUAGE INTEGRATED QUERY)");
        Console.WriteLine("==========================================");

        var devs = new List<Developer>
        {
            new("Alice", "C#", 5, 95000),
            new("Bob", "JavaScript", 2, 65000),
            new("Charlie", "C#", 8, 125000),
            new("Diana", "Python", 4, 85000),
            new("Evan", "C#", 1, 55000)
        };

        // 1. Filtering (Where) and Projection (Select)
        var csharpDevs = devs
            .Where(d => d.Language == "C#")
            .Select(d => $"{d.Name} ({d.YearsExperience} yrs exp)")
            .ToList();

        Console.WriteLine("--- C# Developers ---");
        foreach (var dev in csharpDevs)
        {
            Console.WriteLine($"• {dev}");
        }

        // 2. Sorting (OrderByDescending) & Slicing (Take)
        var topPaid = devs.OrderByDescending(d => d.Salary).First();
        Console.WriteLine($"\nHighest Paid Developer: {topPaid.Name} (${topPaid.Salary:N0})");

        // 3. Aggregations (Average, Count, Any)
        double avgSalary = devs.Average(d => d.Salary);
        bool hasJunior = devs.Any(d => d.YearsExperience < 2);
        Console.WriteLine($"Average Salary across team: ${avgSalary:N0}");
        Console.WriteLine($"Has junior developer (<2 yrs): {hasJunior}");

        // 4. Grouping (GroupBy)
        Console.WriteLine("\n--- Developers Grouped by Language ---");
        var grouped = devs.GroupBy(d => d.Language);
        foreach (var group in grouped)
        {
            Console.WriteLine($"Language: {group.Key} (Count: {group.Count()})");
        }
    }
}
