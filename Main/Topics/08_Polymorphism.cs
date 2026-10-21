using System;
using System.Collections.Generic;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 8: Object-Oriented Programming - Polymorphism & Abstraction
/// Demonstrates abstract classes, abstract methods, and overriding.
/// </summary>
public static class Polymorphism
{
    // Abstract base class
    public abstract class Animal
    {
        public string Name { get; set; }

        protected Animal(string name)
        {
            Name = name;
        }

        // Must be implemented by derived classes
        public abstract string Speak();

        // Concrete method shared by all animals
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"{Name} says: \"{Speak()}\"");
        }
    }

    public class Dog : Animal
    {
        public Dog(string name) : base(name) { }

        public override string Speak() => "Woof! Woof!";
    }

    public class Cat : Animal
    {
        public Cat(string name) : base(name) { }

        public override string Speak() => "Meow!";
    }

    public class Cow : Animal
    {
        public Cow(string name) : base(name) { }

        public override string Speak() => "Moo!";
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 08. POLYMORPHISM & ABSTRACTION");
        Console.WriteLine("==========================================");

        // Polymorphism: A collection of base class references pointing to various derived objects
        var animals = new List<Animal>
        {
            new Dog("Buddy"),
            new Cat("Whiskers"),
            new Cow("Daisy")
        };

        foreach (var animal in animals)
        {
            animal.DisplayInfo();
        }
    }
}
