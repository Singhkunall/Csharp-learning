using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 3: Pattern Matching & Switch Expressions (Modern C# syntax)
/// </summary>
public static class PatternMatchingAndSwitch
{
    public static class Classifier
    {
        // Switch expression with relational and logical patterns
        public static string GetDayType(int dayOfWeek) => dayOfWeek switch
        {
            0 or 6        => "Weekend",
            >= 1 and <= 5 => "Weekday",
            _             => "Invalid"
        };

        // Grade classifier using relational patterns
        public static string GetGrade(int score) => score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _     => "F"
        };

        // Pattern matching with 'is' and 'or'
        public static bool IsWeekend(int day) => day is 0 or 6;
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 03. PATTERN MATCHING & SWITCH EXPRESSIONS");
        Console.WriteLine("==========================================");

        Console.WriteLine($"Day 0: {Classifier.GetDayType(0)}"); // Weekend
        Console.WriteLine($"Day 3: {Classifier.GetDayType(3)}"); // Weekday
        Console.WriteLine($"Day 6: {Classifier.GetDayType(6)}"); // Weekend
        Console.WriteLine($"Day 9: {Classifier.GetDayType(9)}"); // Invalid

        Console.WriteLine($"Score 95: Grade {Classifier.GetGrade(95)}");
        Console.WriteLine($"Score 85: Grade {Classifier.GetGrade(85)}");
        Console.WriteLine($"Score 72: Grade {Classifier.GetGrade(72)}");
        Console.WriteLine($"Score 65: Grade {Classifier.GetGrade(65)}");
        Console.WriteLine($"Score 40: Grade {Classifier.GetGrade(40)}");

        Console.WriteLine($"Is Day 6 weekend? {Classifier.IsWeekend(6)}");
        Console.WriteLine($"Is Day 2 weekend? {Classifier.IsWeekend(2)}");
    }
}
