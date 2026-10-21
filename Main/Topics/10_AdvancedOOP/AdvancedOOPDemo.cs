using System;

namespace CSharpMastery.Topics.Topic10;

/// <summary>
/// Practical demonstrations for Topic 10: Advanced OOP Concepts
/// </summary>
public static class AdvancedOOPDemo
{
    // 1. Init-only properties & Record Class
    public record UserProfile
    {
        public required string Username { get; init; }
        public required string Email { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }

    // 2. Custom Indexer implementation
    public class SimpleKeyStore
    {
        private readonly string[] _values = new string[5];

        public string this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }
    }

    // 3. Explicit Interface Implementation
    public interface IEnglishSpeaker { void Greet(); }
    public interface ISpanishSpeaker { void Greet(); }

    public class BilingualPerson : IEnglishSpeaker, ISpanishSpeaker
    {
        void IEnglishSpeaker.Greet() => Console.WriteLine("Hello!");
        void ISpanishSpeaker.Greet() => Console.WriteLine("¡Hola!");
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 10: ADVANCED OOP, RECORDS & INDEXERS");
        Console.WriteLine("=======================================================");

        // Init-only immutable record with 'with' expression
        var user1 = new UserProfile { Username = "Kunal", Email = "kunal@test.com" };
        var user2 = user1 with { Email = "kunal.updated@test.com" }; // Non-destructive mutation
        Console.WriteLine($"Original User: {user1.Username} ({user1.Email})");
        Console.WriteLine($"Cloned & Mutated: {user2.Username} ({user2.Email})");

        // Indexer usage
        var store = new SimpleKeyStore();
        store[0] = "Cached Token A";
        Console.WriteLine($"Accessed via Indexer [0]: {store[0]}");

        // Explicit interface dispatch
        var bilingual = new BilingualPerson();
        ((IEnglishSpeaker)bilingual).Greet();
        ((ISpanishSpeaker)bilingual).Greet();
    }
}
