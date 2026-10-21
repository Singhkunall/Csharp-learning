using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic16;

/// <summary>
/// Practical demonstrations for Topic 16: Delegates, Events & Lambdas
/// </summary>
public static class DelegatesEventsDemo
{
    // Custom delegate definition
    public delegate void AlertHandler(string alertMessage, int severity);

    // Custom Event Arguments
    public class IncidentEventArgs : EventArgs
    {
        public string Sector { get; }
        public string IncidentDescription { get; }

        public IncidentEventArgs(string sector, string description)
        {
            Sector = sector;
            IncidentDescription = description;
        }
    }

    // Publisher Class
    public class CentralDispatchStation
    {
        // Standard event pattern: EventHandler<TEventArgs>
        public event EventHandler<IncidentEventArgs>? IncidentReported;

        public void BroadcastIncident(string sector, string description)
        {
            Console.WriteLine($"\n    • [Dispatch Tower] 🚨 Broadcasting incident in Sector '{sector}': {description}");
            // Thread-safe invocation using null-conditional operator:
            IncidentReported?.Invoke(this, new IncidentEventArgs(sector, description));
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 📡 TOPIC 16: DELEGATES, EVENTS & FUNCTIONAL LAMBDAS");
        Console.WriteLine("=======================================================");

        // 1. Multicast Delegate Invocation List Inspection
        Console.WriteLine("[1] Multicast Delegates & Invocation Lists:");
        AlertHandler alertChannel = (msg, sev) => Console.WriteLine($"    • [Console Logger (Sev {sev})]: {msg}");
        alertChannel += (msg, sev) => Console.WriteLine($"    • [Audit Telemetry (Sev {sev})]: Transmitted to cloud gateway.");

        // Inspecting internal invocation list:
        Delegate[] subscribers = alertChannel.GetInvocationList();
        Console.WriteLine($"    • Multicast delegate contains {subscribers.Length} registered subscriber methods.");
        alertChannel.Invoke("Cooling pump pressure alert", 3);

        // 2. The Functional Trinity: Action, Func, Predicate
        Console.WriteLine("\n[2] The Standard Functional Trinity (Action, Func, Predicate):");
        Action<string> logAction = message => Console.WriteLine($"    • Action<string> (Side-effect only): {message}");
        Func<double, double, double> hypotenuseFunc = (a, b) => Math.Sqrt((a * a) + (b * b));
        Predicate<int> isEvenPredicate = n => n % 2 == 0;

        logAction("Testing Action invocation");
        Console.WriteLine($"    • Func<double, double, double> Hypotenuse(3, 4) = {hypotenuseFunc(3, 4)}");
        Console.WriteLine($"    • Predicate<int> IsEven(42) = {isEvenPredicate(42)}");

        // 3. Anonymous Methods vs. Lambda Expressions
        Console.WriteLine("\n[3] Anonymous Methods (delegate keyword) vs. Lambdas (=>):");
        
        // C# 2.0 Anonymous Method syntax:
        Func<int, int> legacySquare = delegate (int x) { return x * x; };
        
        // Modern C# Lambda Expression syntax:
        Func<int, int> modernSquare = x => x * x;

        Console.WriteLine($"    • C# 2.0 Anonymous Method Result: {legacySquare(5)}");
        Console.WriteLine($"    • Modern C# Lambda Result        : {modernSquare(5)}");

        // 4. Publisher-Subscriber Event Pattern & Memory Leak Prevention
        Console.WriteLine("\n[4] Events & Safe Unsubscription (Preventing Vampire Leaks):");
        var station = new CentralDispatchStation();

        EventHandler<IncidentEventArgs> handler = (sender, e) =>
        {
            Console.WriteLine($"      -> [Ambulance Unit 10] Responding to Sector '{e.Sector}': {e.IncidentDescription}");
        };

        // Subscribe:
        station.IncidentReported += handler;
        station.BroadcastIncident("North District", "Transformer fire reported.");

        // Unsubscribe: Cleans up memory reference from Publisher to Subscriber
        station.IncidentReported -= handler;
        Console.WriteLine("    • [Subscriber Cleanup] Ambulance Unit 10 successfully unsubscribed (-=).");
        station.BroadcastIncident("South District", "False alarm."); // Handler will NOT execute!

        // 5. Closures & Variable Hoisting
        Console.WriteLine("\n[5] Closures & Variable Capture:");
        int multiplier = 10; // Local stack variable
        
        // Lambda captures 'multiplier' from enclosing scope:
        Func<int, int> multiplyByCaptured = x => x * multiplier;
        Console.WriteLine($"    • Captured multiplier (10) * 5 = {multiplyByCaptured(5)}");

        multiplier = 100; // Mutating outer variable alters captured state!
        Console.WriteLine($"    • After mutating outer variable to 100: * 5 = {multiplyByCaptured(5)}");
    }
}
