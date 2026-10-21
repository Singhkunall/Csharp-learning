using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CSharpMastery.Topics.Topic23;

/// <summary>
/// Practical demonstrations for Topic 23: ASP.NET Core & REST Web Architecture
/// </summary>
public static class AspNetCoreDemo
{
    // 1. Data Transfer Object (DTO) with Declarative Validation Attributes
    public record CreateAccountDto
    {
        [Required(ErrorMessage = "Full name is mandatory.")]
        [StringLength(50, MinimumLength = 3)]
        public string FullName { get; init; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "A valid RFC email address is required.")]
        public string Email { get; init; } = string.Empty;

        [Range(18, 120, ErrorMessage = "Must be an adult legal age.")]
        public int Age { get; init; }
    }

    // 2. Simulated Middleware Component Pattern
    public class PerformanceTelemetryMiddleware
    {
        public async Task InvokeAsync(string httpPath, Func<Task> next)
        {
            Console.WriteLine($"    • [Middleware Inbound] Inspecting incoming HTTP request for '{httpPath}'");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Pass control down to the next middleware / endpoint
            await next();

            stopwatch.Stop();
            Console.WriteLine($"    • [Middleware Outbound] Response processed in {stopwatch.ElapsedMilliseconds} ms (Headers appended).");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🌐 TOPIC 23: ASP.NET CORE REST API ARCHITECTURE");
        Console.WriteLine("=======================================================");

        // 1. DTO Validation Verification
        Console.WriteLine("[1] Defensive DTO Validation (Preventing Over-Posting):");
        var invalidDto = new CreateAccountDto { FullName = "A", Email = "not-an-email", Age = 12 };
        
        var validationContext = new ValidationContext(invalidDto);
        var validationResults = new System.Collections.Generic.List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(invalidDto, validationContext, validationResults, validateAllProperties: true);
        Console.WriteLine($"    • DTO Validation Succeeded: {isValid}");
        foreach (var error in validationResults)
        {
            Console.WriteLine($"      ❌ Validation Fault: {error.ErrorMessage}");
        }

        // 2. Middleware Pipeline Execution Simulation
        Console.WriteLine("\n[2] Middleware Pipeline Execution Flow (Bidirectional Nesting):");
        var middleware = new PerformanceTelemetryMiddleware();

        middleware.InvokeAsync("/api/v1/orders/submit", async () =>
        {
            Console.WriteLine("        -> [ENDPOINT EXECUTING] Minimal API MapPost(\"/api/v1/orders/submit\") running business logic.");
            await Task.Delay(30); // Simulating database save
        }).Wait();

        // 3. Modern Minimal API Syntax Reference
        Console.WriteLine("\n[3] Production Minimal API Code Pattern:");
        Console.WriteLine("""
            // In Program.cs of WebApplication1:
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/api/users/{id:int}", (int id) => Results.Ok(new { Id = id, Status = "Active" }));
            app.MapPost("/api/users", (CreateAccountDto dto) => Results.Created($"/api/users/1", dto));

            app.Run();
            """);
    }
}
