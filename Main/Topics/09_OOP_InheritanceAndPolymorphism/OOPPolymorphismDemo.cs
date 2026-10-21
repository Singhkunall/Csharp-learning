using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic09;

/// <summary>
/// Practical demonstrations for Topic 09: Inheritance, Abstraction & Polymorphism
/// </summary>
public static class OOPPolymorphismDemo
{
    // Interface specifying capability
    public interface IEncryptable
    {
        string EncryptionStandard { get; }
    }

    // Abstract base class establishing identity and shared state
    public abstract class CloudStorageProvider : IEncryptable
    {
        public string ProviderName { get; }
        public string StorageRegion { get; }
        public string EncryptionStandard => "AES-256 GCM";

        protected CloudStorageProvider(string name, string region)
        {
            ProviderName = name;
            StorageRegion = region;
        }

        // Abstract method: must be implemented by derived subclasses
        public abstract void UploadPayload(string fileName, int sizeBytes);

        // Virtual method: can be overridden, but has default implementation
        public virtual void DisplayDiagnostics()
        {
            Console.WriteLine($"    • [Base Diagnostics] {ProviderName} located in region '{StorageRegion}'");
        }
    }

    // Concrete derived class 1: Amazon S3
    public class AmazonS3Provider : CloudStorageProvider
    {
        public AmazonS3Provider(string region) : base("AWS S3", region) { }

        public override void UploadPayload(string fileName, int sizeBytes)
        {
            Console.WriteLine($"    • [AWS S3 ({StorageRegion})] Streaming {fileName} ({sizeBytes:N0} bytes) to S3 Bucket with multi-part upload.");
        }

        public override void DisplayDiagnostics()
        {
            Console.WriteLine($"    • [Custom AWS Diagnostics] S3 Bucket active with Cross-Region Replication.");
        }
    }

    // Concrete derived class 2: Azure Blob Storage
    public class AzureBlobProvider : CloudStorageProvider
    {
        public AzureBlobProvider(string region) : base("Azure Blob", region) { }

        public override void UploadPayload(string fileName, int sizeBytes)
        {
            Console.WriteLine($"    • [Azure Blob ({StorageRegion})] Putting block blob {fileName} ({sizeBytes:N0} bytes) in container '$web'.");
        }
    }

    // Demonstrating the 'new' keyword (Method Hiding) vs 'override'
    public class BaseSpeaker
    {
        public virtual void Speak() => Console.WriteLine("    • BaseSpeaker says: 'Hello from Base'");
    }

    public class OverridingSpeaker : BaseSpeaker
    {
        public override void Speak() => Console.WriteLine("    • OverridingSpeaker says: 'True polymorphic override!'");
    }

    public class HidingSpeaker : BaseSpeaker
    {
        // 'new' intentionally breaks the virtual method table linkage!
        public new void Speak() => Console.WriteLine("    • HidingSpeaker says: 'Hidden with new keyword!'");
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" ☁️ TOPIC 09: POLYMORPHISM, ABSTRACTION & VTABLE DISPATCH");
        Console.WriteLine("=======================================================");

        // 1. Polymorphic Collection (Treating diverse subclasses uniformly)
        Console.WriteLine("[1] Dynamic Polymorphic Dispatch over Cloud Providers:");
        var cloudFleet = new List<CloudStorageProvider>
        {
            new AmazonS3Provider("us-east-1"),
            new AzureBlobProvider("westeurope")
        };

        foreach (var provider in cloudFleet)
        {
            // Dispatches to the correct specialized implementation at runtime!
            provider.UploadPayload("customer_database_dump.sql", 10_485_760);
            provider.DisplayDiagnostics();
            Console.WriteLine($"      Encryption standard: {provider.EncryptionStandard}");
        }

        // 2. The Classic Interview Trap: 'override' vs 'new'
        Console.WriteLine("\n[2] The Interview Trap: 'override' vs 'new' (Method Hiding):");

        BaseSpeaker polyRef1 = new OverridingSpeaker();
        BaseSpeaker polyRef2 = new HidingSpeaker();

        Console.Write("    • Calling polyRef1.Speak() (Uses 'override'): ");
        polyRef1.Speak(); // Outputs: OverridingSpeaker (Dispatched via vtable)

        Console.Write("    • Calling polyRef2.Speak() (Uses 'new')     : ");
        polyRef2.Speak(); // Outputs: BaseSpeaker! (The 'new' method is hidden because variable is typed as BaseSpeaker)
    }
}
