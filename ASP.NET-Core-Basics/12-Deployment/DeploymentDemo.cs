using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

namespace AspNetCoreBasics.Deployment;

// ============================================================================
// Health Checks Diagnostic Engine Simulation
// ============================================================================

public enum HealthStatus
{
    Healthy,
    Degraded,
    Unhealthy
}

public class HealthCheckResult
{
    public HealthStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
}

public class SimulatedHealthEngine
{
    private readonly Dictionary<string, Func<HealthCheckResult>> _checks = new();

    public void RegisterCheck(string name, Func<HealthCheckResult> check)
    {
        _checks[name] = check;
    }

    public (int StatusCode, string ReportJson) RunReport()
    {
        var entries = new Dictionary<string, object>();
        var overallStatus = HealthStatus.Healthy;

        foreach (var (name, check) in _checks)
        {
            var result = check();
            entries[name] = new
            {
                status = result.Status.ToString(),
                description = result.Description,
                durationMs = result.Duration.TotalMilliseconds
            };

            if (result.Status == HealthStatus.Unhealthy)
                overallStatus = HealthStatus.Unhealthy;
            else if (result.Status == HealthStatus.Degraded && overallStatus != HealthStatus.Unhealthy)
                overallStatus = HealthStatus.Degraded;
        }

        int httpStatusCode = overallStatus switch
        {
            HealthStatus.Healthy => 200,
            HealthStatus.Degraded => 200,
            HealthStatus.Unhealthy => 503,
            _ => 500
        };

        var report = new
        {
            status = overallStatus.ToString(),
            totalChecks = _checks.Count,
            results = entries
        };

        return (httpStatusCode, JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
    }
}

// ============================================================================
// Token-Bucket Rate Limiter Simulation
// ============================================================================

public class SimulatedTokenBucketLimiter
{
    private readonly int _capacity;
    private int _tokens;
    private readonly object _lock = new();

    public SimulatedTokenBucketLimiter(int capacity)
    {
        _capacity = capacity;
        _tokens = capacity;
    }

    public bool TryAcquire(out int remainingTokens)
    {
        lock (_lock)
        {
            if (_tokens > 0)
            {
                _tokens--;
                remainingTokens = _tokens;
                return true;
            }
            remainingTokens = 0;
            return false;
        }
    }

    public void Refill(int count)
    {
        lock (_lock)
        {
            _tokens = Math.Min(_capacity, _tokens + count);
        }
    }
}

// ============================================================================
// Cache-Aside Pattern Simulation
// ============================================================================

public class SimulatedCacheAsideService
{
    private readonly Dictionary<string, string> _distributedRedisCache = new();

    public string GetProductDetails(int productId)
    {
        string cacheKey = $"product:{productId}";

        // 1. Check Cache
        if (_distributedRedisCache.TryGetValue(cacheKey, out var cachedData))
        {
            Console.WriteLine($"      [CACHE HIT  ⚡ ~1ms] Key '{cacheKey}' retrieved from Redis: {cachedData}");
            return cachedData;
        }

        // 2. Cache Miss: Fetch from slow SQL Server
        Console.WriteLine($"      [CACHE MISS 🐢 ~120ms] Key '{cacheKey}' not in Redis. Querying SQL Server...");
        Thread.Sleep(20); // Simulated DB query latency

        string dbResult = $"{{\"id\": {productId}, \"name\": \"Cloud Native C# Book\", \"price\": 49.99}}";

        // 3. Populate Cache
        _distributedRedisCache[cacheKey] = dbResult;
        Console.WriteLine($"      [CACHE SET  💾] Cached '{cacheKey}' into Redis with 1-hour expiration.");

        return dbResult;
    }
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class DeploymentDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🚀 MODULE 12: DEPLOYMENT, RATE LIMITING & PERFORMANCE ");
        Console.WriteLine("=======================================================\n");

        // 1. Health Checks Diagnostic Endpoint (/healthz)
        Console.WriteLine("[1] Production Health Check Pipeline (/healthz):");
        var health = new SimulatedHealthEngine();
        health.RegisterCheck("sql_server_primary", () => new HealthCheckResult
        {
            Status = HealthStatus.Healthy,
            Description = "Connected to SQL Server cluster (Latency: 2ms)",
            Duration = TimeSpan.FromMilliseconds(2)
        });
        health.RegisterCheck("redis_distributed_cache", () => new HealthCheckResult
        {
            Status = HealthStatus.Healthy,
            Description = "Redis replica active (Memory: 38% used)",
            Duration = TimeSpan.FromMilliseconds(1)
        });
        health.RegisterCheck("disk_storage_temp", () => new HealthCheckResult
        {
            Status = HealthStatus.Degraded,
            Description = "Disk space below 20% remaining threshold",
            Duration = TimeSpan.FromMilliseconds(5)
        });

        var (statusCode, reportJson) = health.RunReport();
        Console.WriteLine($"    HTTP Response Code: {statusCode} OK (Overall Status: Degraded)");
        Console.WriteLine(reportJson);
        Console.WriteLine();

        // 2. Token-Bucket Rate Limiter Simulation
        Console.WriteLine("[2] Rate Limiting Protection (Token Bucket Algorithm - Capacity = 3):");
        var limiter = new SimulatedTokenBucketLimiter(capacity: 3);

        for (int i = 1; i <= 5; i++)
        {
            if (limiter.TryAcquire(out int remaining))
            {
                Console.WriteLine($"    Request #{i}: HTTP 200 OK (Allowed - Remaining Tokens: {remaining})");
            }
            else
            {
                Console.WriteLine($"    Request #{i}: ⚠️ HTTP 429 Too Many Requests (Rate limit exceeded! Retry-After: 10s)");
            }
        }
        Console.WriteLine();

        // 3. Cache-Aside Optimization Simulation
        Console.WriteLine("[3] Cache-Aside Pattern with Simulated Redis:");
        var cacheService = new SimulatedCacheAsideService();

        Console.WriteLine("    Client 1 requests Product #42:");
        cacheService.GetProductDetails(42);

        Console.WriteLine("\n    Client 2 requests identical Product #42 (Instantaneous Cache Hit):");
        cacheService.GetProductDetails(42);
        Console.WriteLine();

        // 4. Multi-Stage Container & AOT Benefits
        Console.WriteLine("[4] Modern Cloud Deployment Metrics:");
        Console.WriteLine("    • Standard .NET SDK Image:      ~850 MB");
        Console.WriteLine("    • Chiseled ASP.NET Runtime:      ~110 MB (Zero shell, non-root user)");
        Console.WriteLine("    • Native AOT Binary:             ~28 MB (Zero JIT, instantaneous 15ms startup)");
    }
}
