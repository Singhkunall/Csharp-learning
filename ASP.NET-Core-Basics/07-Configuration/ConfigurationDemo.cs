using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreBasics.Configuration;

// ============================================================================
// Strongly-Typed Options Pattern Definition
// ============================================================================

public class DatabaseOptions
{
    public const string Section = "Database";

    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Range(1, 100)]
    public int MaxPoolSize { get; set; } = 20;

    public bool EnableSensitiveDataLogging { get; set; } = false;
}

public class PaymentOptions
{
    public const string Section = "PaymentGateway";

    [Required]
    public string BaseUrl { get; set; } = string.Empty;

    [Range(1, 60)]
    public int TimeoutSeconds { get; set; } = 30;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}

// ============================================================================
// Simulated Configuration Engine & Hierarchy
// ============================================================================

public class SimulatedConfigurationRoot
{
    private readonly List<Dictionary<string, string>> _layers = new();
    private readonly Dictionary<string, string> _merged = new(StringComparer.OrdinalIgnoreCase);

    public void AddLayer(string layerName, Dictionary<string, string> values)
    {
        Console.WriteLine($"    [Layer Added] {layerName} ({values.Count} keys)");
        _layers.Add(values);
        Rebuild();
    }

    private void Rebuild()
    {
        _merged.Clear();
        foreach (var layer in _layers)
        {
            foreach (var kvp in layer)
            {
                // Normalize environment variable double underscores to colons
                string normalizedKey = kvp.Key.Replace("__", ":");
                _merged[normalizedKey] = kvp.Value;
            }
        }
    }

    public string? this[string key]
    {
        get => _merged.TryGetValue(key, out var val) ? val : null;
    }

    public T Bind<T>(string sectionPrefix) where T : new()
    {
        var instance = new T();
        var props = typeof(T).GetProperties();

        foreach (var prop in props)
        {
            string lookupKey = $"{sectionPrefix}:{prop.Name}";
            if (_merged.TryGetValue(lookupKey, out var rawVal))
            {
                object convertedVal = Convert.ChangeType(rawVal, prop.PropertyType);
                prop.SetValue(instance, convertedVal);
            }
        }

        // Validate Data Annotations
        var context = new ValidationContext(instance);
        Validator.ValidateObject(instance, context, validateAllProperties: true);

        return instance;
    }
}

// ============================================================================
// Options Pattern Lifetimes Simulation
// ============================================================================

public interface IOptions<out T> where T : class
{
    T Value { get; }
}

public class OptionsWrapper<T> : IOptions<T> where T : class
{
    public T Value { get; }
    public OptionsWrapper(T value) => Value = value;
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class ConfigurationDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🎛️ MODULE 07: CONFIGURATION, ENVIRONMENTS & OPTIONS   ");
        Console.WriteLine("=======================================================\n");

        var config = new SimulatedConfigurationRoot();

        // 1. Simulating Provider Layers
        Console.WriteLine("[1] Loading Hierarchical Configuration Sources:");

        // Base layer: appsettings.json
        config.AddLayer("1. appsettings.json (Base)", new Dictionary<string, string>
        {
            ["Database:ConnectionString"] = "Server=sql.prod;Database=AppDb;",
            ["Database:MaxPoolSize"] = "50",
            ["Database:EnableSensitiveDataLogging"] = "false",
            ["PaymentGateway:BaseUrl"] = "https://api.stripe.com/v1",
            ["PaymentGateway:TimeoutSeconds"] = "30",
            ["PaymentGateway:ApiKey"] = "REPLACE_IN_SECRETS"
        });

        // Environment override: appsettings.Development.json
        config.AddLayer("2. appsettings.Development.json (Environment Override)", new Dictionary<string, string>
        {
            ["Database:ConnectionString"] = "Server=localhost;Database=DevDb;",
            ["Database:EnableSensitiveDataLogging"] = "true",
            ["PaymentGateway:BaseUrl"] = "https://sandbox.api.stripe.com/v1"
        });

        // User Secrets (Local Dev only)
        config.AddLayer("3. User Secrets (Secure Local Secret Storage)", new Dictionary<string, string>
        {
            ["PaymentGateway:ApiKey"] = "sk_test_51MockSecretKey_xyz789"
        });

        // Environment Variables (Container / OS Overrides with double underscore syntax)
        config.AddLayer("4. Environment Variables (Host OS / Docker / K8s)", new Dictionary<string, string>
        {
            ["PaymentGateway__TimeoutSeconds"] = "45"
        });

        Console.WriteLine("\n[2] Resolving Merged Keys with Priority Hierarchy:");
        Console.WriteLine($"    • Database:ConnectionString = {config["Database:ConnectionString"]} (Overridden by Dev)");
        Console.WriteLine($"    • Database:MaxPoolSize      = {config["Database:MaxPoolSize"]} (Preserved from Base)");
        Console.WriteLine($"    • Database:EnableLogging    = {config["Database:EnableSensitiveDataLogging"]} (Overridden by Dev)");
        Console.WriteLine($"    • Payment:BaseUrl           = {config["PaymentGateway:BaseUrl"]} (Overridden by Dev Sandbox)");
        Console.WriteLine($"    • Payment:TimeoutSeconds    = {config["PaymentGateway:TimeoutSeconds"]} (Overridden by Env Var)");
        Console.WriteLine($"    • Payment:ApiKey            = {config["PaymentGateway:ApiKey"]} (Supplied by User Secrets)\n");

        // 3. Strongly-Typed Options Pattern Binding & Validation
        Console.WriteLine("[3] Binding Configuration to Strongly-Typed Options Classes:");
        var dbOptions = config.Bind<DatabaseOptions>(DatabaseOptions.Section);
        var payOptions = config.Bind<PaymentOptions>(PaymentOptions.Section);

        Console.WriteLine($"    ✓ DatabaseOptions:  Max Pool = {dbOptions.MaxPoolSize}, SensitiveLog = {dbOptions.EnableSensitiveDataLogging}");
        Console.WriteLine($"    ✓ PaymentOptions:   URL = {payOptions.BaseUrl}, Timeout = {payOptions.TimeoutSeconds}s, Key = {payOptions.ApiKey[..7]}...\n");

        // 4. Demonstrating Validation Enforcement
        Console.WriteLine("[4] Validation Guard Demonstration (Catching Misconfigurations):");
        try
        {
            var invalidConfig = new SimulatedConfigurationRoot();
            invalidConfig.AddLayer("InvalidConfig", new Dictionary<string, string>
            {
                ["PaymentGateway:BaseUrl"] = "", // Empty required string!
                ["PaymentGateway:TimeoutSeconds"] = "999" // Exceeds [Range(1, 60)]!
            });
            invalidConfig.Bind<PaymentOptions>("PaymentGateway");
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"    ⚠️ Validation Trap Successfully Caught at Startup:");
            Console.WriteLine($"       Exception: {ex.Message}");
        }
    }
}
