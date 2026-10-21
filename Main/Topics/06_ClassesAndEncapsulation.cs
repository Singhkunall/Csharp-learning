using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 6: Classes, Objects, Fields, Methods, Encapsulation, const vs readonly
/// </summary>
public static class ClassesAndEncapsulation
{
    // 1. Basic Class
    public class Student
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }

        public void Display()
        {
            Console.WriteLine($"Student Name: {Name}, Age: {Age}");
        }
    }

    // 2. Constants vs Readonly Fields
    public class Circle
    {
        public const double Pi = 3.14159;  // Compile-time constant
        public readonly int Id;            // Set only at initialization or constructor

        public Circle(int id)
        {
            Id = id;
        }
    }

    // 3. Encapsulation (Private fields with public methods)
    public class BankAccount
    {
        private double _balance; // Hidden from outside access
        public string Owner { get; set; } = string.Empty;

        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                _balance += amount;
                WriteLog($"Deposited {amount:C}");
            }
        }

        private void WriteLog(string message)
        {
            Console.WriteLine($"[Audit Log] {message}");
        }

        public void Display()
        {
            Console.WriteLine($"Account Owner: {Owner}, Balance: {_balance:C}");
        }
    }

    // 4. Product with Calculations & Constants
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Brand { get; set; }

        public const double GST = 0.18; // 18% GST

        public double FinalPrice()
        {
            return Price * Quantity * (1 + GST);
        }

        public void Show()
        {
            Console.WriteLine($"Product: {Name}, Brand: {Brand ?? "N/A"}, Price: {Price:C}, Qty: {Quantity}, Final (+GST): {FinalPrice():C}");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 06. CLASSES, OBJECTS & ENCAPSULATION");
        Console.WriteLine("==========================================");

        // Student instances
        var s1 = new Student { Name = "Kunal", Age = 22 };
        var s2 = new Student { Name = "Ruchi", Age = 34 };
        s1.Display();
        s2.Display();

        // BankAccount Encapsulation
        var account = new BankAccount { Owner = "Ruchi" };
        account.Deposit(500);
        account.Display();

        // Products with GST Calculation
        var p1 = new Product { Name = "Wireless Mouse", Price = 599, Quantity = 5, Brand = "Dell" };
        var p2 = new Product { Name = "Keyboard", Price = 678, Brand = "HP" };
        var p3 = new Product { Name = "Monitor", Price = 14999, Quantity = 2, Brand = "Samsung" };
        p1.Show();
        p2.Show();
        p3.Show();
    }
}
