using System;
using System.IO;
using System.Text.Json;

namespace CSharpMastery.Topics.Topic15;

/// <summary>
/// Practical demonstrations for Topic 15: File Handling and I/O
/// </summary>
public static class FileIODemo
{
    public record AppSettings(string Environment, int MaxConnections, bool EnableLogging);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 15: FILE HANDLING, JSON & PATHS");
        Console.WriteLine("=======================================================");

        // 1. Cross-platform path combining
        string tempDir = Path.GetTempPath();
        string targetFilePath = Path.Combine(tempDir, "csharp_learning_demo.json");

        // 2. JSON Serialization
        var settings = new AppSettings("Production", 100, true);
        string jsonPayload = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

        // 3. File Writing via StreamWriter
        File.WriteAllText(targetFilePath, jsonPayload);
        Console.WriteLine($"Successfully wrote configuration to: {targetFilePath}");

        // 4. File Reading & Deserialization
        if (File.Exists(targetFilePath))
        {
            string readContent = File.ReadAllText(targetFilePath);
            var deserialized = JsonSerializer.Deserialize<AppSettings>(readContent);
            Console.WriteLine($"Deserialized: Environment = {deserialized?.Environment}, MaxConnections = {deserialized?.MaxConnections}");

            // Cleanup temp file
            File.Delete(targetFilePath);
            Console.WriteLine("Cleaned up temporary demonstration file.");
        }
    }
}
