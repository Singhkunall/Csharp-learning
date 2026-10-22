using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNetCoreBasics.Middleware;

/// <summary>
/// Practical demonstration of the ASP.NET Core Request Pipeline:
/// Simulating the Russian Doll / Onion model, RequestDelegate chaining,
/// short-circuiting, and upstream/downstream execution flow.
/// </summary>
public static class MiddlewareDemo
{
    public static void Run() => RunAsync().GetAwaiter().GetResult();

    public static async Task RunAsync()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🧅 MODULE 03: REQUEST PIPELINE & MIDDLEWARE INTERNALS ");
        Console.WriteLine("=======================================================\n");

        var pipeline = new SimulatedPipeline();

        // 1. Exception Handling Middleware (Outermost shell)
        pipeline.Use(async (ctx, next) =>
        {
            try
            {
                Console.WriteLine("  [1:IN]  ExceptionHandlerMiddleware: Guarding pipeline...");
                await next(ctx);
                Console.WriteLine("  [1:OUT] ExceptionHandlerMiddleware: Request completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [1:RECOVERY] ExceptionHandlerMiddleware caught: {ex.Message}");
                ctx.StatusCode = 500;
                ctx.ResponseBody = "{ \"error\": \"Internal Server Error\", \"code\": 500 }";
            }
        });

        // 2. Performance Profiler Middleware
        pipeline.Use(async (ctx, next) =>
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("  [2:IN]  ProfilerMiddleware: Starting stopwatch...");
            await next(ctx);
            sw.Stop();
            Console.WriteLine($"  [2:OUT] ProfilerMiddleware: Completed in {sw.ElapsedMilliseconds}ms with status {ctx.StatusCode}");
        });

        // 3. API Key Authentication Guard (Demonstrating Short-Circuiting)
        pipeline.Use(async (ctx, next) =>
        {
            Console.WriteLine("  [3:IN]  AuthGuardMiddleware: Checking X-Api-Key...");
            if (!ctx.Headers.TryGetValue("X-Api-Key", out var key) || key != "secret-123")
            {
                Console.WriteLine("  [3:BLOCK] AuthGuardMiddleware: Invalid API key! Short-circuiting pipeline.");
                ctx.StatusCode = 401;
                ctx.ResponseBody = "{ \"error\": \"Unauthorized: Missing or invalid X-Api-Key\" }";
                return; // Do NOT call next() -> Short-circuits here!
            }

            ctx.User = "AuthorizedClient";
            await next(ctx);
            Console.WriteLine("  [3:OUT] AuthGuardMiddleware: Exiting auth context.");
        });

        // 4. Terminal Endpoint Handler
        pipeline.SetTerminalEndpoint(async ctx =>
        {
            Console.WriteLine($"  [4:EXEC] Endpoint Handler: Serving resource for {ctx.User} at {ctx.Path}...");
            await Task.Delay(10); // Simulate work

            if (ctx.Path == "/api/crash")
            {
                throw new InvalidOperationException("Simulated catastrophic database failure!");
            }

            ctx.StatusCode = 200;
            ctx.ResponseBody = "{ \"message\": \"Welcome to the secure resource\", \"data\": [1, 2, 3] }";
        });

        // --- Test Scenario A: Unauthorized Request (Short-circuit test) ---
        Console.WriteLine("\n--- [SCENARIO A: Request with Missing API Key] ---");
        var reqA = new MockHttpContext { Path = "/api/data" };
        await pipeline.ExecuteAsync(reqA);
        Console.WriteLine($"Result: HTTP {reqA.StatusCode} -> {reqA.ResponseBody}");

        // --- Test Scenario B: Authorized Successful Request ---
        Console.WriteLine("\n--- [SCENARIO B: Authorized Valid Request] ---");
        var reqB = new MockHttpContext { Path = "/api/data" };
        reqB.Headers["X-Api-Key"] = "secret-123";
        await pipeline.ExecuteAsync(reqB);
        Console.WriteLine($"Result: HTTP {reqB.StatusCode} -> {reqB.ResponseBody}");

        // --- Test Scenario C: Internal Server Exception (Caught by Error Middleware) ---
        Console.WriteLine("\n--- [SCENARIO C: Unhandled Exception Thrown in Endpoint] ---");
        var reqC = new MockHttpContext { Path = "/api/crash" };
        reqC.Headers["X-Api-Key"] = "secret-123";
        await pipeline.ExecuteAsync(reqC);
        Console.WriteLine($"Result: HTTP {reqC.StatusCode} -> {reqC.ResponseBody}");
    }
}

#region Pipeline Simulation Infrastructure
public class MockHttpContext
{
    public string Path { get; set; } = "/";
    public Dictionary<string, string> Headers { get; } = new(StringComparer.OrdinalIgnoreCase);
    public string? User { get; set; }
    public int StatusCode { get; set; } = 200;
    public string ResponseBody { get; set; } = string.Empty;
}

public delegate Task MockRequestDelegate(MockHttpContext context);
public delegate Task MockMiddlewareDelegate(MockHttpContext context, MockRequestDelegate next);

public class SimulatedPipeline
{
    private readonly List<MockMiddlewareDelegate> _components = new();
    private MockRequestDelegate _terminal = ctx => Task.CompletedTask;

    public void Use(MockMiddlewareDelegate middleware) => _components.Add(middleware);
    public void SetTerminalEndpoint(MockRequestDelegate endpoint) => _terminal = endpoint;

    public async Task ExecuteAsync(MockHttpContext context)
    {
        // Build the Russian Doll chain backwards from terminal to first
        MockRequestDelegate chain = _terminal;
        for (int i = _components.Count - 1; i >= 0; i--)
        {
            var current = _components[i];
            var next = chain;
            chain = ctx => current(ctx, next);
        }

        await chain(context);
    }
}
#endregion
