using System;

namespace CSharpMastery.Topics.Topic04;

/// <summary>
/// Practical demonstrations for Topic 04: Decision-Making and Loops
/// </summary>
public static class LoopsDemo
{
    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 04: DECISION-MAKING, LOOPS & PATTERNS");
        Console.WriteLine("=======================================================");

        // 1. Modern Switch Expression with Relational Patterns
        int temperature = 28;
        string weatherAdvisory = temperature switch
        {
            < 0            => "Freezing conditions: Stay indoors",
            >= 0 and <= 15 => "Cold: Wear heavy coat",
            > 15 and <= 25 => "Mild: Pleasant weather",
            > 25 and <= 35 => "Warm: Stay hydrated",
            _              => "Extreme Heat warning"
        };
        Console.WriteLine($"Weather Advisory for {temperature}°C: {weatherAdvisory}");

        // 2. Foreach with index break / continue
        int[] scores = { 45, 88, 92, 100, 30, 75 };
        Console.Write("Filtered scores (> 50): ");
        foreach (var score in scores)
        {
            if (score < 50) continue; // Skip failed scores
            Console.Write($"{score} ");
        }
        Console.WriteLine();

        // 3. do-while Guaranteeing at least one execution
        int counter = 0;
        do
        {
            Console.WriteLine($"do-while executed at counter: {counter}");
            counter++;
        } while (counter < 1);
    }
}
