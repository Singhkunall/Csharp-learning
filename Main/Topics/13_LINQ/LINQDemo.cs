using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMastery.Topics.Topic13;

/// <summary>
/// Practical demonstrations for Topic 13: LINQ
/// </summary>
public static class LINQDemo
{
    public record Product(int Id, string Name, string Category, double Price);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 13: LINQ (LANGUAGE INTEGRATED QUERY)");
        Console.WriteLine("=======================================================");

        var catalog = new List<Product>
        {
            new(1, "Laptop", "Electronics", 1200.00),
            new(2, "Headphones", "Electronics", 150.00),
            new(3, "Coffee Mug", "Home", 15.00),
            new(4, "Keyboard", "Electronics", 80.00),
            new(5, "Desk Chair", "Furniture", 250.00)
        };

        // 1. Where, OrderBy, and Select projection
        var affordableElectronics = catalog
            .Where(p => p.Category == "Electronics" && p.Price < 500)
            .OrderBy(p => p.Price)
            .Select(p => $"{p.Name} (${p.Price})")
            .ToList();

        Console.WriteLine("Affordable Electronics (< $500): " + string.Join(", ", affordableElectronics));

        // 2. GroupBy Category
        var groupedByCategory = catalog.GroupBy(p => p.Category);
        foreach (var group in groupedByCategory)
        {
            Console.WriteLine($"Category '{group.Key}': {group.Count()} items, Avg Price: {group.Average(p => p.Price):C}");
        }

        // 3. FirstOrDefault & Any
        bool hasExpensiveItems = catalog.Any(p => p.Price > 1000);
        Product? topItem = catalog.FirstOrDefault(p => p.Price > 1000);
        Console.WriteLine($"Has items > $1000: {hasExpensiveItems} (Top item: {topItem?.Name})");
    }
}
