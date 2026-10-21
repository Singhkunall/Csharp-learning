using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpMastery.Topics.Topic17;

// 1. Custom Attribute Definition
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class ComplianceAuditAttribute : Attribute
{
    public string ClassificationLevel { get; }
    public string ApprovedBy { get; }

    public ComplianceAuditAttribute(string level, string approvedBy)
    {
        ClassificationLevel = level;
        ApprovedBy = approvedBy;
    }
}

// Applying custom attribute to a domain entity
[ComplianceAudit("Strict-Enterprise", "ChiefSecurityOfficer")]
public class ClassifiedDocument
{
    [ComplianceAudit("Confidential", "SecurityLead")]
    public string DocumentId { get; set; } = "DOC-99882";
    public string Title { get; set; } = "Quarterly Strategic Architecture";

    public void PrintMetadata()
    {
        Console.WriteLine($"    • [Document Internal Method]: {Title} ({DocumentId})");
    }
}

// 2. Extension Method on System.String
public static class TextExtensions
{
    public static string MaskSensitive(this string input, int visibleChars = 4)
    {
        if (string.IsNullOrEmpty(input) || input.Length <= visibleChars) return input;
        return new string('*', input.Length - visibleChars) + input[^visibleChars..];
    }
}

/// <summary>
/// Practical demonstrations for Topic 17: Modern C# Features, Reflection & Attributes
/// </summary>
public static class ModernFeaturesDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" ✨ TOPIC 17: MODERN C#, REFLECTION & METAPROGRAMMING");
        Console.WriteLine("=======================================================");

        // 1. C# 12 Collection Expressions & Spread Operator
        Console.WriteLine("[1] Modern Collection Expressions & Spread Operator [..a, ..b]:");
        int[] baseline = [10, 20, 30];
        int[] expansion = [40, 50];
        
        // C# 12 Spread syntax:
        int[] combined = [.. baseline, 35, .. expansion];
        Console.WriteLine($"    • Combined array via spread [..baseline, 35, ..expansion]: [{string.Join(", ", combined)}]");

        // 2. Extension Method Call
        Console.WriteLine("\n[2] Extension Methods in Action:");
        string secretApiKey = "sk-live-98234710928347109283";
        Console.WriteLine($"    • Masked API Key: {secretApiKey.MaskSensitive(visibleChars: 4)}");

        // 3. Custom Attributes & Reflection Metaprogramming
        Console.WriteLine("\n[3] Runtime Reflection & Custom Attribute Inspection:");
        Type docType = typeof(ClassifiedDocument);
        
        // Reading class-level custom attribute:
        var classAudit = docType.GetCustomAttribute<ComplianceAuditAttribute>();
        if (classAudit != null)
        {
            Console.WriteLine($"    • [Reflection] Class '{docType.Name}' is tagged with [ComplianceAudit]:");
            Console.WriteLine($"      Classification Level : {classAudit.ClassificationLevel}");
            Console.WriteLine($"      Approved By          : {classAudit.ApprovedBy}");
        }

        // Dynamically invoking a method via Reflection:
        object dynamicInstance = Activator.CreateInstance(docType)!;
        MethodInfo? printMethod = docType.GetMethod("PrintMetadata");
        printMethod?.Invoke(dynamicInstance, null);

        // 4. The 'dynamic' keyword (DLR Runtime Resolution)
        Console.WriteLine("\n[4] The 'dynamic' Keyword (Resolved at Runtime via DLR):");
        dynamic lateBound = "Hello Modern C#";
        Console.WriteLine($"    • Late-bound length: {lateBound.Length}");
        lateBound = 999; // Type shifts dynamically at runtime
        Console.WriteLine($"    • Late-bound shifted to integer: {lateBound + 1}");

        // 5. yield return On-Demand Lazy Stream Generator
        Console.WriteLine("\n[5] yield return On-Demand Streaming Generator:");
        Console.Write("    • Streaming prime candidate numbers: ");
        foreach (var number in StreamMultiplesOf(7, limit: 5))
        {
            Console.Write($"{number} ");
        }
        Console.WriteLine();
    }

    private static IEnumerable<int> StreamMultiplesOf(int multiplier, int limit)
    {
        for (int i = 1; i <= limit; i++)
        {
            // State machine freezes execution here until next foreach pull:
            yield return multiplier * i;
        }
    }
}
