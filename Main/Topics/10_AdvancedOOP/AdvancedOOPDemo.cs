using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic10;

// 1. Partial Class demonstration (Part 1: Core Identity)
public partial class EnterpriseServer
{
    public string Hostname { get; init; } = "prod-db-01";
}

// Partial Class demonstration (Part 2: Health Checks)
public partial class EnterpriseServer
{
    public void PerformDiagnostics()
    {
        Console.WriteLine($"    • [Partial Class Merged] Diagnostics complete on {Hostname}. CPU: Normal, RAM: 74% utilized.");
    }
}

/// <summary>
/// Practical demonstrations for Topic 10: Advanced OOP Concepts
/// </summary>
public static class AdvancedOOPDemo
{
    // 2. Explicit Interface Implementation Interfaces
    public interface ICitizen
    {
        void ReportStatus();
    }

    public interface ISecretAgent
    {
        void ReportStatus();
    }

    public class Operative : ICitizen, ISecretAgent
    {
        public string CodeName { get; }

        public Operative(string codeName) => CodeName = codeName;

        // Explicit implementation 1: Bound strictly to ICitizen
        void ICitizen.ReportStatus()
        {
            Console.WriteLine($"    • [ICitizen View]: Citizen '{CodeName}' reports all annual taxes paid.");
        }

        // Explicit implementation 2: Bound strictly to ISecretAgent
        void ISecretAgent.ReportStatus()
        {
            Console.WriteLine($"    • [ISecretAgent View]: 007 Agent '{CodeName}' reports mission compromised, moving to extract.");
        }
    }

    // 3. Composition vs. Inheritance (The Modular Sound System)
    public interface IAudioSource
    {
        string PlayStream();
    }

    public class BluetoothSource : IAudioSource
    {
        public string PlayStream() => "Streaming AAC 320kbps over Bluetooth 5.3";
    }

    public class VinylTurntableSource : IAudioSource
    {
        public string PlayStream() => "Analog vinyl playback via Phono Pre-Amp";
    }

    public class SoundSystem
    {
        // HAS-A relationship (Composition)
        private IAudioSource _source;

        public SoundSystem(IAudioSource initialSource) => _source = initialSource;

        public void ChangeSource(IAudioSource newSource)
        {
            _source = newSource;
            Console.WriteLine($"    • Audio input swapped: {_source.GetType().Name}");
        }

        public void Play() => Console.WriteLine($"    • Outputting: {_source.PlayStream()}");
    }

    // 4. Proper Object Equality Contract (Equals + GetHashCode)
    public class ProductSku : IEquatable<ProductSku>
    {
        public string Category { get; }
        public int ItemCode { get; }

        public ProductSku(string category, int code)
        {
            Category = category;
            ItemCode = code;
        }

        public bool Equals(ProductSku? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Category == other.Category && ItemCode == other.ItemCode;
        }

        public override bool Equals(object? obj) => Equals(obj as ProductSku);

        public override int GetHashCode() => HashCode.Combine(Category, ItemCode);

        public static bool operator ==(ProductSku? left, ProductSku? right) => Equals(left, right);
        public static bool operator !=(ProductSku? left, ProductSku? right) => !Equals(left, right);
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🏛 TOPIC 10: ADVANCED OOP, COMPOSITION & EQUALITY");
        Console.WriteLine("=======================================================");

        // 1. Partial Classes Merged by Roslyn Compiler
        Console.WriteLine("[1] Partial Classes (Two Files/Blocks Merged into One Type):");
        var server = new EnterpriseServer();
        server.PerformDiagnostics();

        // 2. Explicit Interface Implementation
        Console.WriteLine("\n[2] Explicit Interface Implementation (Dual Identity):");
        var agent = new Operative("James");
        
        // Cannot call agent.ReportStatus() directly because it's implemented explicitly!
        // You must cast to the specific interface:
        ((ICitizen)agent).ReportStatus();
        ((ISecretAgent)agent).ReportStatus();

        // 3. Composition over Inheritance in Action
        Console.WriteLine("\n[3] Composition Over Inheritance (Swappable Audio Sources):");
        var hiFi = new SoundSystem(new BluetoothSource());
        hiFi.Play();

        // Swapping component dynamically at runtime with zero class hierarchy rewrite:
        hiFi.ChangeSource(new VinylTurntableSource());
        hiFi.Play();

        // 4. Value Equality & GetHashCode Contract in Hash Collections
        Console.WriteLine("\n[4] Object Equality & GetHashCode Contract:");
        var skuA = new ProductSku("ELEC", 4001);
        var skuB = new ProductSku("ELEC", 4001);

        Console.WriteLine($"    • skuA == skuB                  : {skuA == skuB} (Custom operator)");
        Console.WriteLine($"    • skuA.Equals(skuB)             : {skuA.Equals(skuB)} (IEquatable<T>)");
        Console.WriteLine($"    • ReferenceEquals(skuA, skuB)   : {ReferenceEquals(skuA, skuB)} (Separate heap objects)");
        Console.WriteLine($"    • HashCode skuA = {skuA.GetHashCode()}, skuB = {skuB.GetHashCode()} (Identical!)");

        var skuLookup = new HashSet<ProductSku> { skuA };
        Console.WriteLine($"    • HashSet contains identical skuB: {skuLookup.Contains(skuB)} (Succeeds via GetHashCode + Equals!)");
    }
}
