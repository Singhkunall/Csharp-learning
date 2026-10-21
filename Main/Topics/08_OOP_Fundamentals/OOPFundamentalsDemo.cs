using System;

namespace CSharpMastery.Topics.Topic08;

/// <summary>
/// Practical demonstrations for Topic 08: OOP Fundamentals & Encapsulation
/// </summary>
public static class OOPFundamentalsDemo
{
    public class NuclearReactor
    {
        // 1. Compile-time constant (Inlined everywhere by Roslyn)
        public const double MeltdownThresholdCelsius = 2500.0;

        // 2. Runtime instance-specific immutability (Configured only in constructor)
        public readonly string ReactorId;

        // 3. Static runtime immutability (Application-wide singleton timestamp)
        public static readonly DateTime CommissionedDate = DateTime.UtcNow;

        // 4. Encapsulated private state (Cannot be tampered with directly)
        private double _coreTemperatureCelsius;
        private int _coolantLitersPerSecond;

        // 5. Full property with business validation
        public double CoreTemperatureCelsius
        {
            get => _coreTemperatureCelsius;
            private set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Temperature cannot be below absolute zero.");
                _coreTemperatureCelsius = value;
            }
        }

        // 6. Expression-bodied read-only property (Calculated dynamically)
        public bool IsOverheating => _coreTemperatureCelsius > 2000.0;

        public NuclearReactor(string reactorId, double initialTemp)
        {
            ReactorId = reactorId;
            _coreTemperatureCelsius = initialTemp;
            _coolantLitersPerSecond = 500;
        }

        // Controlled public methods protecting state invariants
        public void AdjustCoolantFlow(int liters)
        {
            if (liters < 0) throw new ArgumentException("Coolant flow cannot be negative.");
            _coolantLitersPerSecond = liters;
            _coreTemperatureCelsius -= (liters * 0.1); // Coolant cools the core
            Console.WriteLine($"    • [Reactor {ReactorId}] Coolant increased to {liters} L/s -> Core cooled to {CoreTemperatureCelsius:F1}°C");
        }

        public void EmergencyScram()
        {
            Console.WriteLine($"    • [Reactor {ReactorId}] 🚨 EMERGENCY SCRAM ACTIVATED! Dropping control rods...");
            _coreTemperatureCelsius = 150.0;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" ⚛️ TOPIC 08: OOP ENCAPSULATION & ACCESS CONTROL");
        Console.WriteLine("=======================================================");

        var reactor = new NuclearReactor("REACTOR-ALPHA-01", 1850.0);
        Console.WriteLine($"[1] Reactor Initialized:");
        Console.WriteLine($"    • Identifier:            {reactor.ReactorId} (readonly instance field)");
        Console.WriteLine($"    • Initial Core Temp:     {reactor.CoreTemperatureCelsius}°C");
        Console.WriteLine($"    • Meltdown Limit:        {NuclearReactor.MeltdownThresholdCelsius}°C (const)");
        Console.WriteLine($"    • Is Overheating:        {reactor.IsOverheating}");

        Console.WriteLine("\n[2] Controlled State Modification (Encapsulation):");
        reactor.AdjustCoolantFlow(1200);
        Console.WriteLine($"    • Post-Cooling Status:   Is Overheating = {reactor.IsOverheating}");

        // Attempting direct raw field corruption is forbidden by the compiler:
        // reactor._coreTemperatureCelsius = 99999; // COMPILE ERROR: Inaccessible due to protection level!

        Console.WriteLine("\n[3] Triggering Emergency Protocol:");
        reactor.EmergencyScram();
        Console.WriteLine($"    • Safe Temperature Restored: {reactor.CoreTemperatureCelsius}°C");
    }
}
