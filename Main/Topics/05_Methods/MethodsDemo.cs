using System;

namespace CSharpMastery.Topics.Topic05;

/// <summary>
/// Practical demonstrations for Topic 05: Methods & Parameter Passing
/// </summary>
public static class MethodsDemo
{
    // A sample struct to demonstrate the 'in' modifier (avoiding copy overhead)
    public readonly struct SensorData
    {
        public double Temperature { get; }
        public double Pressure { get; }
        public double Humidity { get; }

        public SensorData(double temp, double pressure, double humidity)
        {
            Temperature = temp;
            Pressure = pressure;
            Humidity = humidity;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🛠 TOPIC 05: METHODS, PARAMETERS & RECURSION");
        Console.WriteLine("=======================================================");

        // 1. The 4 Parameter Delivery Protocols: val, ref, out, in
        Console.WriteLine("[1] Parameter Modifiers (val, ref, out, in):");

        // A. Default Pass-by-Value
        int originalValue = 50;
        PassByValue(originalValue);
        Console.WriteLine($"    • Default (by-value): original remains {originalValue} (Untouched)");

        // B. ref modifier (Pass-by-Reference)
        PassByRef(ref originalValue);
        Console.WriteLine($"    • ref modifier      : original changed to {originalValue} (Mutated in-place!)");

        // C. out modifier (Multiple Return Values)
        CalculateDivision(35, 4, out int quotient, out int remainder);
        Console.WriteLine($"    • out modifier      : 35 / 4 -> Quotient = {quotient}, Remainder = {remainder}");

        // D. in modifier (Read-Only Reference)
        var sensor = new SensorData(24.5, 1013.25, 45.0);
        InspectSensor(in sensor);

        // 2. Named & Optional Arguments
        Console.WriteLine("\n[2] Named and Optional Arguments:");
        ConfigureServer("api.production.com", port: 443, useSsl: true, maxConnections: 5000);
        ConfigureServer("localhost"); // Uses all defaults

        // 3. Method Overloading
        Console.WriteLine("\n[3] Method Overloading:");
        Console.WriteLine($"    • Add(int, int)       : {Add(10, 20)}");
        Console.WriteLine($"    • Add(double, double) : {Add(12.5, 7.5)}");
        Console.WriteLine($"    • Add(string, string) : {Add("C#", " Mastery")}");

        // 4. params Keyword (Variable Argument List)
        Console.WriteLine("\n[4] Variable Arguments via 'params':");
        double averageScore = CalculateAverage(88.5, 92.0, 79.5, 95.0, 100.0);
        Console.WriteLine($"    • Average calculated from 5 inputs: {averageScore:F1}");
        Console.WriteLine($"    • Average calculated from 0 inputs: {CalculateAverage()}");

        // 5. Recursion & Local Functions
        Console.WriteLine("\n[5] Recursion with Local Function (Fibonacci & Factorial):");
        Console.WriteLine($"    • Factorial of 6: {Factorial(6)}");

        Console.Write("    • Fibonacci sequence (first 8 terms): ");
        for (int i = 0; i < 8; i++)
        {
            Console.Write($"{Fibonacci(i)} ");
        }
        Console.WriteLine();

        // Local function encapsulating recursion
        static long Factorial(int n)
        {
            if (n <= 1) return 1; // Base case
            return n * Factorial(n - 1); // Recursive step
        }

        static int Fibonacci(int n)
        {
            if (n <= 0) return 0; // Base case 1
            if (n == 1) return 1; // Base case 2
            return Fibonacci(n - 1) + Fibonacci(n - 2); // Recursive step
        }
    }

    private static void PassByValue(int value) => value += 1000;

    private static void PassByRef(ref int value) => value += 1000;

    private static void CalculateDivision(int dividend, int divisor, out int quotient, out int remainder)
    {
        quotient = dividend / divisor;
        remainder = dividend % divisor;
    }

    private static void InspectSensor(in SensorData data)
    {
        // data.Temperature = 99; // COMPILE ERROR: Cannot assign to members because it is an 'in' variable
        Console.WriteLine($"    • in modifier       : Temp = {data.Temperature}°C, Pressure = {data.Pressure} hPa (Zero-copy read)");
    }

    private static void ConfigureServer(string host, int port = 80, bool useSsl = false, int maxConnections = 100)
    {
        Console.WriteLine($"    • Server: {host}:{port} | SSL: {useSsl} | Max Conn: {maxConnections}");
    }

    private static int Add(int a, int b) => a + b;
    private static double Add(double a, double b) => a + b;
    private static string Add(string a, string b) => a + b;

    private static double CalculateAverage(params double[] numbers)
    {
        if (numbers.Length == 0) return 0.0;
        double sum = 0;
        foreach (var n in numbers) sum += n;
        return sum / numbers.Length;
    }
}
