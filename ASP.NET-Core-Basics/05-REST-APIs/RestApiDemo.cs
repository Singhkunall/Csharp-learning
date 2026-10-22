using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace AspNetCoreBasics.RestApis;

/// <summary>
/// Practical demonstration of REST API architecture in ASP.NET Core:
/// DTO mapping, DataAnnotations validation, HTTP status codes,
/// ProblemDetails error responses, and over-posting prevention.
/// </summary>
public static class RestApiDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🌐 MODULE 05: REST API ARCHITECTURE & DTO PATTERNS   ");
        Console.WriteLine("=======================================================\n");

        var controller = new SimulatedProductsController();

        // 1. GET: Fetching all resources (200 OK)
        Console.WriteLine("[1] GET /api/v1/products");
        var listResult = controller.GetAll();
        Console.WriteLine($"    Response Status: {listResult.StatusCode}");
        Console.WriteLine($"    Payload: {JsonSerializer.Serialize(listResult.Data, new JsonSerializerOptions { WriteIndented = true })}\n");

        // 2. POST: Successful resource creation (201 Created + Location)
        Console.WriteLine("[2] POST /api/v1/products (Valid Payload)");
        var validDto = new CreateProductDto
        {
            Title = "C# 13 in a Nutshell",
            Price = 49.99m,
            Category = "Books"
        };
        var createResult = controller.Create(validDto);
        Console.WriteLine($"    Response Status:  {createResult.StatusCode}");
        Console.WriteLine($"    Location Header:  {createResult.Location}");
        Console.WriteLine($"    Payload: {JsonSerializer.Serialize(createResult.Data)}\n");

        // 3. POST: Validation Failure (400 Bad Request with ProblemDetails)
        Console.WriteLine("[3] POST /api/v1/products (Invalid Payload - Demonstrating RFC 7807)");
        var invalidDto = new CreateProductDto
        {
            Title = "", // Required violation!
            Price = -5.00m, // Range violation!
            Category = "X" // Min length violation!
        };
        var badRequestResult = controller.Create(invalidDto);
        Console.WriteLine($"    Response Status: {badRequestResult.StatusCode}");
        Console.WriteLine($"    Problem Details: {JsonSerializer.Serialize(badRequestResult.Data, new JsonSerializerOptions { WriteIndented = true })}\n");

        // 4. GET: Resource not found (404 Not Found)
        Console.WriteLine("[4] GET /api/v1/products/999 (Non-existent ID)");
        var notFoundResult = controller.GetById(999);
        Console.WriteLine($"    Response Status: {notFoundResult.StatusCode}");
        Console.WriteLine($"    Error Message:   {notFoundResult.Data}");
    }
}

#region DTOs and Domain Models
public class CreateProductDto
{
    [Required(ErrorMessage = "Product title is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Range(0.01, 10000.00, ErrorMessage = "Price must be between $0.01 and $10,000.00.")]
    public decimal Price { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Category must have at least 2 characters.")]
    public string Category { get; set; } = string.Empty;
}

public record ProductResponseDto(int Id, string Title, decimal Price, string Category, DateTime CreatedAtUtc);

public class ProductEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal InternalProfitMargin { get; set; } // SENSITIVE: Never expose in DTO!
    public DateTime CreatedAtUtc { get; set; }
}

public record ApiResult<T>(int StatusCode, T? Data, string? Location = null);
#endregion

#region Controller Simulation
public class SimulatedProductsController
{
    private readonly List<ProductEntity> _database = new()
    {
        new() { Id = 1, Title = "Wireless Mouse", Price = 29.99m, Category = "Electronics", InternalProfitMargin = 0.45m, CreatedAtUtc = DateTime.UtcNow.AddDays(-10) },
        new() { Id = 2, Title = "USB-C Hub", Price = 39.99m, Category = "Electronics", InternalProfitMargin = 0.50m, CreatedAtUtc = DateTime.UtcNow.AddDays(-5) }
    };
    private int _nextId = 3;

    public ApiResult<List<ProductResponseDto>> GetAll()
    {
        var dtos = _database.ConvertAll(p => new ProductResponseDto(p.Id, p.Title, p.Price, p.Category, p.CreatedAtUtc));
        return new ApiResult<List<ProductResponseDto>>(200, dtos);
    }

    public ApiResult<object> GetById(int id)
    {
        var entity = _database.Find(p => p.Id == id);
        if (entity is null)
        {
            return new ApiResult<object>(404, new { Error = $"Product with ID #{id} was not found." });
        }

        var dto = new ProductResponseDto(entity.Id, entity.Title, entity.Price, entity.Category, entity.CreatedAtUtc);
        return new ApiResult<object>(200, dto);
    }

    public ApiResult<object> Create(CreateProductDto dto)
    {
        // 1. DataAnnotations Validation Engine
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        bool isValid = Validator.TryValidateObject(dto, context, validationResults, validateAllProperties: true);

        if (!isValid)
        {
            var problemDetails = new Dictionary<string, List<string>>();
            foreach (var vr in validationResults)
            {
                foreach (var member in vr.MemberNames)
                {
                    if (!problemDetails.ContainsKey(member)) problemDetails[member] = new List<string>();
                    problemDetails[member].Add(vr.ErrorMessage ?? "Invalid value");
                }
            }

            return new ApiResult<object>(400, new
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = 400,
                Errors = problemDetails
            });
        }

        // 2. Safe mapping to Database Entity (Preventing over-posting)
        var newEntity = new ProductEntity
        {
            Id = _nextId++,
            Title = dto.Title,
            Price = dto.Price,
            Category = dto.Category,
            InternalProfitMargin = 0.35m, // Calculated server-side!
            CreatedAtUtc = DateTime.UtcNow
        };
        _database.Add(newEntity);

        // 3. Return 201 Created with Location header and projected response DTO
        var responseDto = new ProductResponseDto(newEntity.Id, newEntity.Title, newEntity.Price, newEntity.Category, newEntity.CreatedAtUtc);
        return new ApiResult<object>(201, responseDto, Location: $"/api/v1/products/{newEntity.Id}");
    }
}
#endregion
