using System;
using System.Collections.Generic;

namespace AspNetCoreBasics.FirstProject;

/// <summary>
/// Practical demonstration of ASP.NET Core Project initialization:
/// Simulating WebApplicationBuilder, service registration locks,
/// environment-specific branch configuration, and endpoint registration.
/// </summary>
public static class FirstProjectDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🏗️ MODULE 02: ASP.NET CORE PROJECT LIFECYCLE & BUILDER ");
        Console.WriteLine("=======================================================\n");

        // 1. Initializing the simulated Builder
        Console.WriteLine("[1] Initializing WebApplicationBuilder...");
        var builder = new SimulatedAppBuilder("Development");
        Console.WriteLine($"    • Detected Environment: {builder.Environment.EnvironmentName}");
        Console.WriteLine($"    • Is Development:       {builder.Environment.IsDevelopment}");

        // 2. Registering Services into IServiceCollection
        Console.WriteLine("\n[2] Registering Dependencies into Service Collection...");
        builder.Services.Add("IOrderRepository", "SqlOrderRepository");
        builder.Services.Add("IEmailSender", "SendGridEmailSender");
        builder.Services.Add("IPaymentGateway", "StripePaymentGateway");
        foreach (var (svc, impl) in builder.Services)
        {
            Console.WriteLine($"    ✓ Registered Service: {svc} => {impl}");
        }

        // 3. Freezing the Container with Build()
        Console.WriteLine("\n[3] Freezing Container with builder.Build()...");
        var app = builder.Build();
        Console.WriteLine("    ✓ Container is frozen. No further services can be added.");

        // Demonstrating the Freeze Trap:
        try
        {
            builder.Services.Add("ILateService", "CannotRegisterAfterBuild");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"    ⚠️ Caught Expected Exception: {ex.Message}");
        }

        // 4. Configuring Middleware & Endpoints
        Console.WriteLine("\n[4] Configuring Middleware Pipeline & Endpoint Mappings...");
        if (app.Environment.IsDevelopment)
        {
            Console.WriteLine("    • [DEV ONLY] Swagger & SwaggerUI Middleware Enabled.");
        }
        else
        {
            Console.WriteLine("    • [PROD ONLY] Strict HSTS & Production Exception Masking Enabled.");
        }

        app.MapGet("/api/health", () => "{ \"status\": \"Healthy\", \"uptime\": \"99.99%\" }");
        app.MapGet("/api/products", () => "[ { \"id\": 101, \"title\": \"C# Masterclass Guide\" } ]");

        // 5. Simulating Endpoint Execution
        Console.WriteLine("\n[5] Simulating Client Request Routing to Mapped Endpoints:");
        app.SimulateIncomingRequest("/api/health");
        app.SimulateIncomingRequest("/api/products");
        app.SimulateIncomingRequest("/api/unknown-route");
    }
}

#region Builder & Application Simulation Types
public class SimulatedAppBuilder
{
    public SimulatedEnvironment Environment { get; }
    public Dictionary<string, string> Services { get; } = new();
    private bool _isBuilt;
    public bool IsBuilt => _isBuilt;

    public SimulatedAppBuilder(string environmentName)
    {
        Environment = new SimulatedEnvironment(environmentName);
    }

    public SimulatedApp Build()
    {
        _isBuilt = true;
        return new SimulatedApp(Environment, new Dictionary<string, string>(Services));
    }
}

public class SimulatedEnvironment
{
    public string EnvironmentName { get; }
    public bool IsDevelopment => string.Equals(EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase);

    public SimulatedEnvironment(string environmentName) => EnvironmentName = environmentName;
}

public class SimulatedApp
{
    public SimulatedEnvironment Environment { get; }
    public IReadOnlyDictionary<string, string> Services { get; }
    private readonly Dictionary<string, Func<string>> _endpoints = new(StringComparer.OrdinalIgnoreCase);

    public SimulatedApp(SimulatedEnvironment environment, Dictionary<string, string> services)
    {
        Environment = environment;
        Services = services;
    }

    public void MapGet(string route, Func<string> handler)
    {
        _endpoints[route] = handler;
        Console.WriteLine($"    ✓ Mapped Route: GET {route}");
    }

    public void SimulateIncomingRequest(string route)
    {
        Console.WriteLine($"\n    --> Incoming GET {route}");
        if (_endpoints.TryGetValue(route, out var handler))
        {
            Console.WriteLine($"        <-- HTTP 200 OK: {handler()}");
        }
        else
        {
            Console.WriteLine($"        <-- HTTP 404 Not Found: Resource '{route}' does not exist.");
        }
    }
}
#endregion
