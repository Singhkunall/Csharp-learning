using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AspNetCoreBasics.Introduction;

/// <summary>
/// Practical demonstration of ASP.NET Core architectural foundations:
/// Raw HTTP parsing, HttpContext abstraction, and response serialization.
/// </summary>
public static class IntroductionDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🌐 MODULE 01: INTRODUCTION TO ASP.NET CORE INTERNALS  ");
        Console.WriteLine("=======================================================\n");

        // 1. Simulating an incoming raw TCP byte stream captured by Kestrel
        string rawIncomingHttp = 
            "POST /api/v1/orders?priority=high HTTP/1.1\r\n" +
            "Host: api.contoso.com\r\n" +
            "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64)\r\n" +
            "Authorization: Bearer mock-jwt-token-xyz-123\r\n" +
            "Content-Type: application/json\r\n" +
            "Content-Length: 48\r\n" +
            "\r\n" +
            "{\"customer\": \"Alice\", \"item\": \"Keyboard\", \"qty\": 2}";

        Console.WriteLine("[1] Raw HTTP Stream Received by Kestrel Socket:");
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine(rawIncomingHttp);
        Console.WriteLine("-------------------------------------------------------\n");

        // 2. Kestrel parses raw bytes into an HttpContext structure
        Console.WriteLine("[2] Kestrel Parsing into HttpContext Abstractions...");
        var context = SimulatedHttpContext.Parse(rawIncomingHttp);

        Console.WriteLine($"    • HTTP Method:     {context.Request.Method}");
        Console.WriteLine($"    • Request Path:    {context.Request.Path}");
        Console.WriteLine($"    • Query Parameter: priority = {context.Request.Query["priority"]}");
        Console.WriteLine($"    • Host Header:     {context.Request.Headers["Host"]}");
        Console.WriteLine($"    • Content Type:    {context.Request.Headers["Content-Type"]}");
        Console.WriteLine($"    • Request Body:    {context.Request.Body}\n");

        // 3. Processing request down the pipeline & dispatching response
        Console.WriteLine("[3] Dispatching Response via HttpResponse Buffer...");
        context.Response.StatusCode = 201; // Created
        context.Response.Headers["Server"] = "Kestrel-Simulated";
        context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";

        var responsePayload = new
        {
            OrderId = Guid.NewGuid().ToString("N")[..8],
            Status = "Accepted",
            ProcessedAtUtc = DateTime.UtcNow,
            Message = "Order queued successfully"
        };

        context.Response.Body = JsonSerializer.Serialize(responsePayload, new JsonSerializerOptions { WriteIndented = true });

        // 4. Emitting raw HTTP response bytes back onto TCP socket
        string emittedHttpResponse = context.Response.ToHttpWireFormat();
        Console.WriteLine("\n[4] Wire-Format HTTP Response Sent Back to Client:");
        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine(emittedHttpResponse);
        Console.WriteLine("-------------------------------------------------------");
    }
}

#region Architectural Simulation Types
public class SimulatedHttpContext
{
    public SimulatedHttpRequest Request { get; } = new();
    public SimulatedHttpResponse Response { get; } = new();

    public static SimulatedHttpContext Parse(string rawHttp)
    {
        var context = new SimulatedHttpContext();
        using var reader = new StringReader(rawHttp);

        // Read Request Line: POST /api/v1/orders?priority=high HTTP/1.1
        string? requestLine = reader.ReadLine();
        if (!string.IsNullOrWhiteSpace(requestLine))
        {
            var parts = requestLine.Split(' ');
            if (parts.Length >= 2)
            {
                context.Request.Method = parts[0];
                var rawPath = parts[1];
                if (rawPath.Contains('?'))
                {
                    var pathParts = rawPath.Split('?');
                    context.Request.Path = pathParts[0];
                    foreach (var pair in pathParts[1].Split('&'))
                    {
                        var kvp = pair.Split('=');
                        if (kvp.Length == 2) context.Request.Query[kvp[0]] = kvp[1];
                    }
                }
                else
                {
                    context.Request.Path = rawPath;
                }
            }
        }

        // Read Headers
        string? headerLine;
        while (!string.IsNullOrEmpty(headerLine = reader.ReadLine()))
        {
            var colonIndex = headerLine.IndexOf(':');
            if (colonIndex > 0)
            {
                var key = headerLine[..colonIndex].Trim();
                var value = headerLine[(colonIndex + 1)..].Trim();
                context.Request.Headers[key] = value;
            }
        }

        // Read Body
        context.Request.Body = reader.ReadToEnd();
        return context;
    }
}

public class SimulatedHttpRequest
{
    public string Method { get; set; } = "GET";
    public string Path { get; set; } = "/";
    public Dictionary<string, string> Query { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
    public string Body { get; set; } = string.Empty;
}

public class SimulatedHttpResponse
{
    public int StatusCode { get; set; } = 200;
    public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
    public string Body { get; set; } = string.Empty;

    public string ToHttpWireFormat()
    {
        var sb = new StringBuilder();
        string reason = StatusCode switch
        {
            200 => "OK",
            201 => "Created",
            400 => "Bad Request",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown"
        };

        sb.AppendLine($"HTTP/1.1 {StatusCode} {reason}");
        foreach (var (key, value) in Headers)
        {
            sb.AppendLine($"{key}: {value}");
        }
        sb.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(Body)}");
        sb.AppendLine();
        sb.Append(Body);
        return sb.ToString();
    }
}
#endregion
