using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMastery.Topics.Topic13;

/// <summary>
/// Practical demonstrations for Topic 13: LINQ (Language Integrated Query)
/// </summary>
public static class LINQDemo
{
    public record Developer(string Name, string Department, int ExperienceYears, double Salary);
    public record DepartmentProjects(string Department, List<string> ProjectCodes);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🔍 TOPIC 13: LINQ (LANGUAGE INTEGRATED QUERY)");
        Console.WriteLine("=======================================================");

        var team = new List<Developer>
        {
            new("Alice", "Engineering", 7, 125000),
            new("Bob", "QualityAssurance", 3, 75000),
            new("Charlie", "Engineering", 10, 160000),
            new("Diana", "Security", 5, 110000),
            new("Evan", "Engineering", 2, 65000),
            new("Fiona", "QualityAssurance", 6, 95000)
        };

        // 1. Method Syntax vs. Query Syntax
        Console.WriteLine("[1] Method Syntax vs. Query Syntax:");
        
        // Method Syntax (Fluent API)
        var seniorsMethod = team
            .Where(d => d.ExperienceYears >= 5)
            .OrderByDescending(d => d.Salary)
            .Select(d => $"{d.Name} (${d.Salary:N0})")
            .ToList();

        // Query Syntax (SQL-like)
        var seniorsQuery = (from d in team
                            where d.ExperienceYears >= 5
                            orderby d.Salary descending
                            select $"{d.Name} (${d.Salary:N0})").ToList();

        Console.WriteLine($"    • Method Syntax Output : [{string.Join(", ", seniorsMethod)}]");
        Console.WriteLine($"    • Query Syntax Output  : [{string.Join(", ", seniorsQuery)}]");

        // 2. Select vs. SelectMany (The Great Flattener)
        Console.WriteLine("\n[2] Select (Hierarchy) vs. SelectMany (Flat Stream):");
        var deptWorkloads = new List<DepartmentProjects>
        {
            new("Engineering", new() { "PROJ-ALPHA", "PROJ-BETA" }),
            new("Security", new() { "SEC-AUDIT", "ZERO-TRUST", "FIREWALL" })
        };

        // Select preserves the lists-inside-list
        IEnumerable<List<string>> unflattened = deptWorkloads.Select(d => d.ProjectCodes);
        Console.WriteLine($"    • Select yields nested collections: {unflattened.Count()} list objects");

        // SelectMany flattens all internal lists into a single stream
        List<string> allProjects = deptWorkloads.SelectMany(d => d.ProjectCodes).ToList();
        Console.WriteLine($"    • SelectMany yields flat stream   : [{string.Join(", ", allProjects)}]");

        // 3. Multi-Level Sorting: OrderBy and ThenBy
        Console.WriteLine("\n[3] Multi-Level Sorting (OrderBy Department, ThenBy Salary):");
        var sortedStaff = team
            .OrderBy(d => d.Department)
            .ThenByDescending(d => d.Salary)
            .ToList();

        foreach (var dev in sortedStaff)
        {
            Console.WriteLine($"    • {dev.Department,-18} | {dev.Name,-8} | Salary: ${dev.Salary:N0}");
        }

        // 4. GroupBy Aggregations
        Console.WriteLine("\n[4] GroupBy Department with Statistics:");
        var deptSummaries = team
            .GroupBy(d => d.Department)
            .Select(g => new
            {
                Dept = g.Key,
                Headcount = g.Count(),
                AvgSalary = g.Average(d => d.Salary),
                MaxExp = g.Max(d => d.ExperienceYears)
            });

        foreach (var stat in deptSummaries)
        {
            Console.WriteLine($"    • {stat.Dept,-18} Headcount: {stat.Headcount} | Avg Salary: ${stat.AvgSalary:N0} | Top Exp: {stat.MaxExp} yrs");
        }

        // 5. Mathematical Set Operations: Distinct, Intersect, Except
        Console.WriteLine("\n[5] Mathematical Set Operations:");
        int[] batchA = { 1, 2, 3, 4, 5 };
        int[] batchB = { 4, 5, 6, 7, 8 };

        var commonItems = batchA.Intersect(batchB);
        var uniqueToA = batchA.Except(batchB);
        var combined = batchA.Union(batchB);

        Console.WriteLine($"    • Intersect (Common)  : [{string.Join(", ", commonItems)}]");
        Console.WriteLine($"    • Except (In A only)  : [{string.Join(", ", uniqueToA)}]");
        Console.WriteLine($"    • Union (Deduplicated): [{string.Join(", ", combined)}]");

        // 6. Aggregate Operator (Custom Functional Fold / Accumulator)
        Console.WriteLine("\n[6] Functional Accumulator via Aggregate:");
        int[] numbers = { 1, 2, 3, 4, 5 };
        // Calculates product: 1 * 2 * 3 * 4 * 5 = 120
        int factorialProduct = numbers.Aggregate(1, (accumulator, next) => accumulator * next);
        Console.WriteLine($"    • Aggregate Product (1 * 2 * 3 * 4 * 5) = {factorialProduct}");

        // 7. Deferred Execution Proof
        Console.WriteLine("\n[7] Proof of Deferred Execution (The Lazy Hydroelectric Dam):");
        var dynamicDataSource = new List<int> { 10, 20, 30 };

        // Define query (dam is closed, NO execution occurs here)
        var lazyQuery = dynamicDataSource.Where(n => n > 15);

        // Mutate source data AFTER query definition:
        dynamicDataSource.Add(40);
        dynamicDataSource.Add(50);

        // Materialize query (dam opens NOW):
        Console.Write("    • Query defined when list had 3 items; evaluated after adding 40 and 50: ");
        foreach (var val in lazyQuery)
        {
            Console.Write($"{val} "); // Outputs 20, 30, 40, 50!
        }
        Console.WriteLine();
    }
}
