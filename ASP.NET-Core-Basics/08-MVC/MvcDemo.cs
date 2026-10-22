using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreBasics.Mvc;

// ============================================================================
// ViewModels & Domain Models
// ============================================================================

public class ProductItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Sku { get; set; } = string.Empty;
}

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Product title is mandatory.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 60 characters.")]
    public string Title { get; set; } = string.Empty;

    [Range(0.01, 10000.0, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = string.Empty;
}

public class ProductListViewModel
{
    public List<ProductItem> Products { get; set; } = new();
    public string? FlashMessage { get; set; }
}

// ============================================================================
// MVC Framework Abstractions (Simulating Controller, ModelState, TempData)
// ============================================================================

public class SimulatedModelState
{
    public Dictionary<string, List<string>> Errors { get; } = new();
    public bool IsValid => Errors.Count == 0;

    public void AddError(string key, string message)
    {
        if (!Errors.TryGetValue(key, out var list))
        {
            list = new List<string>();
            Errors[key] = list;
        }
        list.Add(message);
    }
}

public class SimulatedTempData
{
    private readonly Dictionary<string, string> _current = new();
    private readonly Dictionary<string, string> _next = new();

    public string? this[string key]
    {
        get
        {
            _current.TryGetValue(key, out var val);
            return val;
        }
        set
        {
            if (value != null) _next[key] = value;
        }
    }

    public void Rotate()
    {
        _current.Clear();
        foreach (var (k, v) in _next)
        {
            _current[k] = v;
        }
        _next.Clear();
    }
}

public abstract class SimulatedController
{
    public SimulatedModelState ModelState { get; } = new();
    public SimulatedTempData TempData { get; set; } = new();

    protected bool TryValidateModel(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        if (!isValid)
        {
            foreach (var result in results)
            {
                foreach (var memberName in result.MemberNames)
                {
                    ModelState.AddError(memberName, result.ErrorMessage ?? "Invalid field");
                }
            }
        }
        return isValid;
    }
}

public class SimulatedProductsController : SimulatedController
{
    private static readonly List<ProductItem> _database = new()
    {
        new ProductItem { Id = 1, Name = "USB-C Fast Charger", Price = 24.99m, Sku = "CHG-901" },
        new ProductItem { Id = 2, Name = "Mechanical Keyboard", Price = 119.50m, Sku = "KB-302" }
    };

    public string Index()
    {
        var flash = TempData["SuccessMessage"];
        var viewModel = new ProductListViewModel
        {
            Products = _database,
            FlashMessage = flash
        };

        // Render HTML representation
        return RenderHtmlView("Views/Products/Index.cshtml", viewModel);
    }

    public (bool Success, string RenderedOutput) Create(CreateProductViewModel model)
    {
        if (!TryValidateModel(model))
        {
            // Validation failed: Re-render form with error tags (HTTP 200 with validation summaries)
            string formHtml = RenderFormView("Views/Products/Create.cshtml", model, ModelState);
            return (false, formHtml);
        }

        // Add to database
        int newId = _database.Count + 1;
        _database.Add(new ProductItem { Id = newId, Name = model.Title, Price = model.Price, Sku = $"SKU-{newId:D4}" });

        // PRG Pattern: Store message in TempData and redirect
        TempData["SuccessMessage"] = $"Product '{model.Title}' successfully created!";
        return (true, "REDIRECT -> 302 Found -> /Products/Index");
    }

    private string RenderHtmlView(string viewPath, ProductListViewModel vm)
    {
        string html = $"<!-- Rendered: {viewPath} inside _Layout.cshtml -->\n" +
                      "<!DOCTYPE html>\n<html>\n<body>\n" +
                      (string.IsNullOrEmpty(vm.FlashMessage) ? "" : $"  <div class='alert alert-success'>{vm.FlashMessage}</div>\n") +
                      "  <h2>Product Catalog</h2>\n  <ul>\n";

        foreach (var p in vm.Products)
        {
            html += $"    <li><strong>{p.Name}</strong> - {p.Price:C} [SKU: {p.Sku}]</li>\n";
        }
        html += "  </ul>\n</body>\n</html>";
        return html;
    }

    private string RenderFormView(string viewPath, CreateProductViewModel vm, SimulatedModelState state)
    {
        string html = $"<!-- Rendered: {viewPath} (Form Validation Failure) -->\n" +
                      "<form method='post'>\n";
        foreach (var (field, errors) in state.Errors)
        {
            foreach (var err in errors)
            {
                html += $"  <span class='text-danger'>[{field}] {err}</span>\n";
            }
        }
        html += $"  <input name='Title' value='{vm.Title}' />\n" +
                $"  <input name='Price' value='{vm.Price}' />\n" +
                "</form>";
        return html;
    }
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class MvcDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🎭 MODULE 08: MVC ARCHITECTURE & THE PRG PATTERN      ");
        Console.WriteLine("=======================================================\n");

        var sharedTempData = new SimulatedTempData();
        var controller = new SimulatedProductsController { TempData = sharedTempData };

        // 1. Initial GET Request: Products/Index
        Console.WriteLine("[1] GET /Products/Index (Initial Page Render):");
        string indexHtml = controller.Index();
        Console.WriteLine(indexHtml);
        Console.WriteLine();

        // 2. POST Request with Validation Failure
        Console.WriteLine("[2] POST /Products/Create (Invalid Data - Demonstrating ModelState):");
        var invalidInput = new CreateProductViewModel
        {
            Title = "A", // Violation: Minimum length is 3
            Price = -10.0m // Violation: Price must be > 0
        };
        var (failed, failureHtml) = controller.Create(invalidInput);
        Console.WriteLine($"    Success: {failed}");
        Console.WriteLine(failureHtml);
        Console.WriteLine();

        // 3. POST Request with Valid Data -> PRG (Post-Redirect-Get) Pattern
        Console.WriteLine("[3] POST /Products/Create (Valid Data - Demonstrating PRG & TempData):");
        var freshController = new SimulatedProductsController { TempData = sharedTempData };
        var validInput = new CreateProductViewModel
        {
            Title = "Noise-Cancelling Headphones",
            Price = 199.99m,
            Category = "Electronics"
        };
        var (success, redirectResult) = freshController.Create(validInput);
        Console.WriteLine($"    Action Result: {redirectResult}\n");

        // 4. Client follows redirect to GET /Products/Index (TempData consumed)
        Console.WriteLine("[4] Client follows 302 Redirect -> GET /Products/Index:");
        sharedTempData.Rotate(); // Simulating browser making the subsequent GET request
        var redirectController = new SimulatedProductsController { TempData = sharedTempData };
        string postRedirectHtml = redirectController.Index();
        Console.WriteLine(postRedirectHtml);
        Console.WriteLine();

        // 5. User refreshes the page (TempData should now be wiped cleanly)
        Console.WriteLine("[5] User Refreshes the Page (F5) - TempData Flash Message is Expired:");
        sharedTempData.Rotate();
        var refreshController = new SimulatedProductsController { TempData = sharedTempData };
        string refreshHtml = refreshController.Index();
        Console.WriteLine(refreshHtml);
    }
}
