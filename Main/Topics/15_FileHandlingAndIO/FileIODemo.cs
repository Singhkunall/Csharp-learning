using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CSharpMastery.Topics.Topic15;

/// <summary>
/// Practical demonstrations for Topic 15: File Handling, Streams & Serialization
/// </summary>
public static class FileIODemo
{
    public record ServerAuditLog(Guid LogId, string EventType, int StatusCode, DateTime Timestamp);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 💾 TOPIC 15: FILE HANDLING, STREAMS & SERIALIZATION");
        Console.WriteLine("=======================================================");

        string baseTempPath = Path.Combine(Path.GetTempPath(), "DotNetMastery_IO_Demo");
        Directory.CreateDirectory(baseTempPath);

        try
        {
            // 1. Cross-Platform Path Diagnostics
            Console.WriteLine("[1] Cross-Platform Path Handling:");
            string demoFile = Path.Combine(baseTempPath, "audit_log.txt");
            Console.WriteLine($"    • Combined Cross-Platform Path: {demoFile}");
            Console.WriteLine($"    • File Extension:               {Path.GetExtension(demoFile)}");
            Console.WriteLine($"    • Directory Separator on OS:    '{Path.DirectorySeparatorChar}'");

            // 2. StreamWriter and StreamReader (Line-by-Line Streaming)
            Console.WriteLine("\n[2] Line-by-Line Streaming (StreamWriter / StreamReader):");
            using (var writer = new StreamWriter(demoFile, append: false, Encoding.UTF8))
            {
                writer.WriteLine("# Server Boot Log Timestamp: 2026-10-21");
                writer.WriteLine("INFO: Core microservices initialized.");
                writer.WriteLine("WARN: Database pool latency at 45ms.");
            }

            using (var reader = new StreamReader(demoFile, Encoding.UTF8))
            {
                string? line;
                int lineCount = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineCount++;
                    Console.WriteLine($"    • [Line {lineCount}] {line}");
                }
            }

            // 3. FileInfo Metadata Query
            Console.WriteLine("\n[3] FileInfo Instance Metadata:");
            var fileInfo = new FileInfo(demoFile);
            Console.WriteLine($"    • File Size:       {fileInfo.Length} bytes");
            Console.WriteLine($"    • Creation Time:   {fileInfo.CreationTime}");
            Console.WriteLine($"    • Is ReadOnly:     {fileInfo.IsReadOnly}");

            // 4. In-Memory Binary File Serialization (BinaryWriter & BinaryReader)
            Console.WriteLine("\n[4] Binary Stream Operations (BinaryWriter & BinaryReader):");
            using var memStream = new MemoryStream();
            using (var binWriter = new BinaryWriter(memStream, Encoding.UTF8, leaveOpen: true))
            {
                binWriter.Write((int)1048576);     // 4 bytes integer
                binWriter.Write((double)3.14159); // 8 bytes double
                binWriter.Write(true);             // 1 byte boolean
                binWriter.Write("Compact Binary Payload");
            }

            // Rewind stream position back to beginning
            memStream.Seek(0, SeekOrigin.Begin);

            using (var binReader = new BinaryReader(memStream))
            {
                int readInt = binReader.ReadInt32();
                double readDouble = binReader.ReadDouble();
                bool readBool = binReader.ReadBoolean();
                string readString = binReader.ReadString();
                Console.WriteLine($"    • Read from raw byte stream: int = {readInt}, double = {readDouble:F4}, bool = {readBool}, text = '{readString}'");
            }

            // 5. High-Performance JSON Serialization
            Console.WriteLine("\n[5] JSON Serialization with Formatting & Options:");
            var audit = new ServerAuditLog(Guid.NewGuid(), "USER_LOGIN_SUCCESS", 200, DateTime.UtcNow);
            
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            string jsonString = JsonSerializer.Serialize(audit, jsonOptions);
            Console.WriteLine($"    • Serialized JSON Output:\n{jsonString}");

            var roundtripped = JsonSerializer.Deserialize<ServerAuditLog>(jsonString, jsonOptions);
            Console.WriteLine($"    • Deserialized event: {roundtripped?.EventType} (Status {roundtripped?.StatusCode})");

            // 6. CSV Processing (Parsing Quoted Comma Fields)
            Console.WriteLine("\n[6] CSV Stream Processing:");
            string csvContent = """
            Id,Name,Role,Salary
            1,"Kunal, Architect",Lead,145000
            2,"Jane Doe",Engineer,120000
            """;

            using var csvReader = new StringReader(csvContent);
            string? header = csvReader.ReadLine();
            Console.WriteLine($"    • CSV Header: {header}");

            string? row;
            while ((row = csvReader.ReadLine()) != null)
            {
                Console.WriteLine($"    • Row data: {row}");
            }
        }
        finally
        {
            // Clean up temporary workspace directory
            if (Directory.Exists(baseTempPath))
            {
                Directory.Delete(baseTempPath, recursive: true);
                Console.WriteLine("\n[CleanUp] Cleaned up temporary test directory successfully.");
            }
        }
    }
}
