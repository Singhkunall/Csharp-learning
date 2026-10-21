using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 7: Constructors & Overloading
/// Demonstrates default (parameterless) constructors, parameterized constructors, and object creation.
/// </summary>
public static class Constructors
{
    public class Car
    {
        public string Brand { get; set; }
        public double Price { get; set; }

        // Parameterless (Default) Constructor
        public Car()
        {
            Brand = "Unknown";
            Price = 0;
        }

        // Parameterized Constructor
        public Car(string brand, double price)
        {
            Brand = brand;
            Price = price;
        }

        public void Show()
        {
            Console.WriteLine($"Car Brand: {Brand}, Price: {Price:C}");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 07. CONSTRUCTORS & OVERLOADING");
        Console.WriteLine("==========================================");

        var c1 = new Car();
        c1.Show();

        var c2 = new Car("Maruti", 650000);
        c2.Show();

        var c3 = new Car("BMW", 4567890);
        c3.Show();
    }
}
