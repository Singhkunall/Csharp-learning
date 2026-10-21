using System;

namespace CSharpMastery.Topics.Topic07;

/// <summary>
/// Practical demonstrations for Topic 07: Essential Built-in Types
/// </summary>
public static class BuiltInTypesDemo
{
    public enum OrderStatus
    {
        Pending = 1,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 07: BUILT-IN TYPES, DATES, ENUMS & TUPLES");
        Console.WriteLine("=======================================================");

        // 1. Decimal vs Double precision
        double doubleSum = 0.1 + 0.2;
        decimal decimalSum = 0.1m + 0.2m;
        Console.WriteLine($"Double (binary drift):  0.1 + 0.2 = {doubleSum}");
        Console.WriteLine($"Decimal (exact base-10): 0.1 + 0.2 = {decimalSum}");

        // 2. Modern DateOnly and TimeOnly (.NET 6+)
        DateOnly releaseDate = new DateOnly(2026, 10, 21);
        TimeOnly meetingTime = new TimeOnly(14, 30);
        Console.WriteLine($"DateOnly: {releaseDate:yyyy-MM-dd}, TimeOnly: {meetingTime}");

        // 3. Guid generation
        Guid uniqueId = Guid.NewGuid();
        Console.WriteLine($"Generated Guid: {uniqueId}");

        // 4. Strongly-Typed Enums
        OrderStatus status = OrderStatus.Shipped;
        Console.WriteLine($"Order Status: {status} (Numeric value: {(int)status})");

        // 5. ValueTuple with named elements
        (string Title, int Pages, bool InStock) book = ("C# in Depth", 528, true);
        Console.WriteLine($"Tuple: Title = '{book.Title}', Pages = {book.Pages}, Available = {book.InStock}");
    }
}
