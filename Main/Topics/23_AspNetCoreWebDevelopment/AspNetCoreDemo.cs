using System;

namespace CSharpMastery.Topics.Topic23;

/// <summary>
/// Practical demonstrations for Topic 23: ASP.NET Core Concepts
/// </summary>
public static class AspNetCoreDemo
{
    // DTO (Data Transfer Object)
    public record CreateUserRequest(string FullName, string Email);
    public record UserResponse(int Id, string FullName, string Email, DateTime CreatedAt);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 23: ASP.NET CORE REST API ARCHITECTURE");
        Console.WriteLine("=======================================================");

        Console.WriteLine("ASP.NET Core Minimal API endpoint pattern:");
        Console.WriteLine("""
        // In Program.cs of WebApplication1:
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/api/users/{id:int}", (int id) => Results.Ok(new UserResponse(id, "Kunal", "kunal@test.com", DateTime.UtcNow)));
        app.MapPost("/api/users", (CreateUserRequest dto) => Results.Created($"/api/users/1", dto));

        app.Run();
        """);

        var request = new CreateUserRequest("Kunal Singh", "kunal@example.com");
        var response = new UserResponse(101, request.FullName, request.Email, DateTime.UtcNow);
        Console.WriteLine($"DTO Transformation: Created user #{response.Id} - {response.FullName}");
    }
}
