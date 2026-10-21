using System;
using System.Threading.Tasks;
using CSharpLearning;
using CSharpLearning.Topics;

Console.WriteLine("====================================================");
Console.WriteLine("     MASTER C# LEARNING & REFERENCE WORKSPACE       ");
Console.WriteLine("====================================================");
Console.WriteLine("Choose a topic to explore:");
Console.WriteLine("   1. Basics & Data Types");
Console.WriteLine("   2. Records & Value Equality");
Console.WriteLine("   3. Pattern Matching & Switch Expressions");
Console.WriteLine("   4. Value Types (struct) vs Reference Types (class)");
Console.WriteLine("   5. Exception Handling (try/catch, StackTrace, Inner)");
Console.WriteLine("   6. Classes, Objects & Encapsulation");
Console.WriteLine("   7. Constructors & Overloading");
Console.WriteLine("   8. OOP Polymorphism & Abstraction");
Console.WriteLine("   9. Generics & Generic Collections");
Console.WriteLine("  10. LINQ (Language Integrated Query)");
Console.WriteLine("  11. Asynchronous Programming (async / await)");
Console.WriteLine("  12. Unit Testing Demo (Calculator & xUnit)");
Console.WriteLine("   A. Run ALL topics in sequence");
Console.WriteLine("   Q. Quit");
Console.Write("\nEnter choice (press Enter for 'A'): ");

string? choice = Console.ReadLine()?.Trim().ToUpper();
if (string.IsNullOrEmpty(choice))
{
    choice = "A";
}

switch (choice)
{
    case "1":
        BasicsAndDataTypes.Run();
        break;
    case "2":
        RecordsAndEquality.Run();
        break;
    case "3":
        PatternMatchingAndSwitch.Run();
        break;
    case "4":
        ValueVsReferenceTypes.Run();
        break;
    case "5":
        ExceptionHandling.Run();
        break;
    case "6":
        ClassesAndEncapsulation.Run();
        break;
    case "7":
        Constructors.Run();
        break;
    case "8":
        Polymorphism.Run();
        break;
    case "9":
        GenericsAndCollections.Run();
        break;
    case "10":
        LINQDemo.Run();
        break;
    case "11":
        await AsyncAwaitDemo.RunAsync();
        break;
    case "12":
        RunCalculatorDemo();
        break;
    case "A":
        BasicsAndDataTypes.Run();
        RecordsAndEquality.Run();
        PatternMatchingAndSwitch.Run();
        ValueVsReferenceTypes.Run();
        ExceptionHandling.Run();
        ClassesAndEncapsulation.Run();
        Constructors.Run();
        Polymorphism.Run();
        GenericsAndCollections.Run();
        LINQDemo.Run();
        await AsyncAwaitDemo.RunAsync();
        RunCalculatorDemo();
        break;
    default:
        Console.WriteLine($"Exiting. Happy coding!");
        break;
}

static void RunCalculatorDemo()
{
    Console.WriteLine("\n==========================================");
    Console.WriteLine(" 12. UNIT TESTING & METHODS (CALCULATOR)");
    Console.WriteLine("==========================================");
    var calc = new Calculator();
    Console.WriteLine($"Calculator.Add(4, 5)       = {calc.Add(4, 5)}");
    Console.WriteLine($"Calculator.Subtract(10, 4) = {calc.Subtract(10, 4)}");
    Console.WriteLine($"Calculator.Multiply(3, 7)  = {calc.Multiply(3, 7)}");
    Console.WriteLine($"Calculator.Divide(20, 4)   = {calc.Divide(20, 4)}");
    Console.WriteLine("\n-> To run the full automated xUnit test suite:");
    Console.WriteLine("   Execute: dotnet test main.Tests");
}
