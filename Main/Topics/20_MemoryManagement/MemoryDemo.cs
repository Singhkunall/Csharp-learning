using System;

namespace CSharpMastery.Topics.Topic20;

/// <summary>
/// Practical demonstrations for Topic 20: Memory Management
/// </summary>
public static class MemoryDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 20: MEMORY ARCHITECTURE, SPAN<T> & GC");
        Console.WriteLine("=======================================================");

        // 1. Zero-Allocation string slicing with ReadOnlySpan<char>
        string dateString = "2026-10-21";
        ReadOnlySpan<char> dateSpan = dateString.AsSpan();

        ReadOnlySpan<char> yearSpan = dateSpan.Slice(0, 4);
        ReadOnlySpan<char> monthSpan = dateSpan.Slice(5, 2);
        ReadOnlySpan<char> daySpan = dateSpan.Slice(8, 2);

        int year = int.Parse(yearSpan);
        int month = int.Parse(monthSpan);
        int day = int.Parse(daySpan);

        Console.WriteLine($"Parsed via Span (0 allocations): Year = {year}, Month = {month}, Day = {day}");

        // 2. GC Diagnostics
        long totalMemory = GC.GetTotalMemory(forceFullCollection: false);
        Console.WriteLine($"Total GC Allocated Memory: {totalMemory:N0} bytes");
        Console.WriteLine($"Max Supported GC Generations: {GC.MaxGeneration}");
    }
}
