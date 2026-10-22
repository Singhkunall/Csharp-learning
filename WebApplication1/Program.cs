var builder = WebApplication.CreateBuilder(args);

// Register essential services
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// In-memory catalog store
var products = new List<Product>
{
    new(1, "Clean Architecture in .NET", 44.99m, "Books"),
    new(2, "Wireless Mechanical Keyboard", 129.50m, "Electronics"),
    new(3, "Ultra-Wide 4K Monitor", 399.00m, "Hardware")
};

// 1. Root diagnostic & discovery endpoint
app.MapGet("/", () => Results.Ok(new
{
    Title = "🚀 ASP.NET Core Learning Playground API",
    Version = "1.0.0 (.NET 10)",
    Status = "Healthy",
    AvailableEndpoints = new[]
    {
        "GET    /api/products",
        "GET    /api/products/{id}",
        "POST   /api/products",
        "DELETE /api/products/{id}",
        "GET    /healthz"
    }
}));

// 2. Health check endpoint
app.MapGet("/healthz", () => Results.Ok(new { Status = "Healthy", TimestampUtc = DateTime.UtcNow }));

// 3. GET all products
app.MapGet("/api/products", () => Results.Ok(products));

// 4. GET product by ID
app.MapGet("/api/products/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound(new { Message = $"Product with ID {id} was not found." });
});

// 5. POST create product
app.MapPost("/api/products", (CreateProductRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Price <= 0)
    {
        return Results.BadRequest(new { Error = "Name is required and price must be greater than zero." });
    }

    int nextId = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
    var newProduct = new Product(nextId, request.Name, request.Price, request.Category ?? "General");
    products.Add(newProduct);

    return Results.Created($"/api/products/{newProduct.Id}", newProduct);
});

// 6. DELETE product
app.MapDelete("/api/products/{id:int}", (int id) =>
{
    var existing = products.FirstOrDefault(p => p.Id == id);
    if (existing is null)
    {
        return Results.NotFound(new { Message = $"Product with ID {id} was not found." });
    }

    products.Remove(existing);
    return Results.NoContent();
});

app.Run();

// DTO records
public record Product(int Id, string Name, decimal Price, string Category);
public record CreateProductRequest(string Name, decimal Price, string? Category);
