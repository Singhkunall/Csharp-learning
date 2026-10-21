using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic12;

/// <summary>
/// Practical demonstrations for Topic 12: Generics, Constraints & Variance
/// </summary>
public static class GenericsDemo
{
    // Interface for generic constraint demo
    public interface IEntity
    {
        int Id { get; }
        string Name { get; }
    }

    public class UserAccount : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    // Generic Repository with multi-constraint (class, interface, new())
    public class DataStore<T> where T : class, IEntity, new()
    {
        private readonly List<T> _records = new();

        public void Add(T entity) => _records.Add(entity);

        public T CreateAndRegister(int id, string name)
        {
            T instance = new T(); // Permitted by 'where T : new()'
            // Property access permitted by 'where T : IEntity'
            Console.WriteLine($"    • [DataStore<{typeof(T).Name}>] Auto-fabricated entity with Id {id}");
            _records.Add(instance);
            return instance;
        }

        public IReadOnlyList<T> GetAll() => _records.AsReadOnly();
    }

    // Domain classes for Covariance & Contravariance
    public class Animal { public string Species { get; set; } = "Unknown"; }
    public class Dog : Animal { public string Breed { get; set; } = "Golden Retriever"; }

    // Custom Contravariant Comparer
    public class AnimalComparer : IComparer<Animal>
    {
        public int Compare(Animal? x, Animal? y) => string.Compare(x?.Species, y?.Species, StringComparison.Ordinal);
    }

    // Custom Generic Delegate with 'in' and 'out' variance
    public delegate TResult Mapper<in TInput, out TResult>(TInput input);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🧬 TOPIC 12: GENERICS, CONSTRAINTS & VARIANCE");
        Console.WriteLine("=======================================================");

        // 1. Generic Methods: Universal Swap
        Console.WriteLine("[1] Generic Swap Method (Type-Safe Stack Swap):");
        int numA = 100, numB = 200;
        Swap(ref numA, ref numB);
        Console.WriteLine($"    • Swapped Integers: numA = {numA}, numB = {numB}");

        string strA = "World", strB = "Hello";
        Swap(ref strA, ref strB);
        Console.WriteLine($"    • Swapped Strings : strA = '{strA}', strB = '{strB}'");

        // 2. Multi-Constrained Generic Class
        Console.WriteLine("\n[2] Generic Class with Multiple Constraints (where T : class, IEntity, new()):");
        var store = new DataStore<UserAccount>();
        store.CreateAndRegister(1, "Kunal Singh");

        // 3. Covariance in Action (out T - Output Positions)
        Console.WriteLine("\n[3] Covariance (out T - Producers / IEnumerable<out T>):");
        List<Dog> kennelOfDogs = new List<Dog> { new Dog { Species = "Canine", Breed = "Husky" } };
        
        // LEGAL because IEnumerable<out T> is Covariant!
        // We can assign IEnumerable<Dog> to IEnumerable<Animal>:
        IEnumerable<Animal> shelterAnimals = kennelOfDogs;
        foreach (var animal in shelterAnimals)
        {
            Console.WriteLine($"    • Covariant Producer Output: {animal.Species}");
        }

        // 4. Contravariance in Action (in T - Consumers / IComparer<in T>)
        Console.WriteLine("\n[4] Contravariance (in T - Consumers / IComparer<in T>):");
        IComparer<Animal> generalAnimalComparer = new AnimalComparer();
        
        // LEGAL because IComparer<in T> is Contravariant!
        // An Animal comparer can be safely passed to sort a list of Dogs:
        IComparer<Dog> dogComparer = generalAnimalComparer;
        Console.WriteLine($"    • Contravariant Consumer: AnimalComparer assigned to IComparer<Dog> successfully!");

        // 5. Custom Generic Delegate with Variance
        Console.WriteLine("\n[5] Custom Generic Delegate (Mapper<in TInput, out TResult>):");
        Mapper<string, int> stringLengthMapper = text => text.Length;
        int length = stringLengthMapper("Enterprise C# Architecture");
        Console.WriteLine($"    • Delegate mapped string length: {length}");
    }

    private static void Swap<T>(ref T left, ref T right)
    {
        T temp = left;
        left = right;
        right = temp;
    }
}
