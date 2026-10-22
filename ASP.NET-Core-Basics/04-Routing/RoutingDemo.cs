using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AspNetCoreBasics.Routing;

/// <summary>
/// Practical demonstration of the ASP.NET Core Routing Engine:
/// Route template parsing, route constraints, query string binding,
/// and endpoint dispatching.
/// </summary>
public static class RoutingDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🗺️ MODULE 04: ROUTING ENGINE & ENDPOINT DISPATCHING   ");
        Console.WriteLine("=======================================================\n");

        var router = new SimulatedEndpointRouter();

        // 1. Registering Endpoints with Constraints & HTTP Verbs
        router.Map("GET", "/api/v1/products", ctx => 
            "HTTP 200 OK: Listing all products [Filter: category=" + (ctx.QueryParams.GetValueOrDefault("category") ?? "ALL") + "]");

        router.Map("GET", "/api/v1/products/{id:int}", ctx =>
            $"HTTP 200 OK: Returning Product Details for ID #{ctx.RouteParams["id"]}");

        router.Map("POST", "/api/v1/products", ctx =>
            "HTTP 201 Created: New product created with ID #99");

        router.Map("PUT", "/api/v1/products/{id:int}", ctx =>
            $"HTTP 200 OK: Product #{ctx.RouteParams["id"]} completely updated");

        router.Map("DELETE", "/api/v1/products/{id:int}", ctx =>
            $"HTTP 204 No Content: Product #{ctx.RouteParams["id"]} deleted successfully");

        // 2. Simulating incoming HTTP requests
        Console.WriteLine("[2] Dispatching Incoming Requests against Route Table:\n");

        router.Dispatch("GET", "/api/v1/products");
        router.Dispatch("GET", "/api/v1/products?category=electronics&sort=asc");
        router.Dispatch("GET", "/api/v1/products/42");
        router.Dispatch("POST", "/api/v1/products");
        router.Dispatch("DELETE", "/api/v1/products/42");

        // 3. Demonstrating Route Constraint Failures (e.g. non-integer ID)
        Console.WriteLine("\n[3] Testing Route Constraint Guard (id:int):");
        router.Dispatch("GET", "/api/v1/products/not-a-number");

        // 4. Testing Unmapped Route (404)
        Console.WriteLine("\n[4] Testing Unknown Route (404):");
        router.Dispatch("GET", "/api/v1/customers");
    }
}

#region Routing Infrastructure Simulation
public class RouteContext
{
    public string Method { get; set; } = "GET";
    public string RawUrl { get; set; } = "/";
    public Dictionary<string, string> RouteParams { get; } = new();
    public Dictionary<string, string> QueryParams { get; } = new();
}

public class RouteEntry
{
    public string Method { get; set; } = "GET";
    public string Template { get; set; } = "";
    public Regex PatternRegex { get; set; } = null!;
    public List<string> ParameterNames { get; set; } = new();
    public Func<RouteContext, string> Handler { get; set; } = null!;
}

public class SimulatedEndpointRouter
{
    private readonly List<RouteEntry> _routes = new();

    public void Map(string method, string template, Func<RouteContext, string> handler)
    {
        var paramNames = new List<string>();

        // Convert template like /api/v1/products/{id:int} into regex ^/api/v1/products/(?<id>\d+)$
        string regexPattern = "^" + Regex.Replace(template, @"\{([a-zA-Z0-9_]+)(:int)?\}", match =>
        {
            string paramName = match.Groups[1].Value;
            bool isInt = match.Groups[2].Success;
            paramNames.Add(paramName);
            return isInt ? $"(?<{paramName}>\\d+)" : $"(?<{paramName}>[^/]+)";
        }) + "$";

        _routes.Add(new RouteEntry
        {
            Method = method.ToUpperInvariant(),
            Template = template,
            PatternRegex = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled),
            ParameterNames = paramNames,
            Handler = handler
        });

        Console.WriteLine($"  ✓ Registered: {method.PadRight(6)} {template}");
    }

    public void Dispatch(string method, string rawUrl)
    {
        var context = new RouteContext { Method = method.ToUpperInvariant(), RawUrl = rawUrl };

        // Parse query string if present
        string path = rawUrl;
        int queryIndex = rawUrl.IndexOf('?');
        if (queryIndex >= 0)
        {
            path = rawUrl[..queryIndex];
            string queryString = rawUrl[(queryIndex + 1)..];
            foreach (var pair in queryString.Split('&'))
            {
                var kvp = pair.Split('=');
                if (kvp.Length == 2) context.QueryParams[kvp[0]] = kvp[1];
            }
        }

        Console.WriteLine($"--> Incoming Request: {context.Method} {context.RawUrl}");

        foreach (var route in _routes)
        {
            if (route.Method != context.Method) continue;

            var match = route.PatternRegex.Match(path);
            if (match.Success)
            {
                foreach (var param in route.ParameterNames)
                {
                    context.RouteParams[param] = match.Groups[param].Value;
                }

                string response = route.Handler(context);
                Console.WriteLine($"    <-- Matched Endpoint [{route.Template}]: {response}\n");
                return;
            }
        }

        Console.WriteLine("    <-- HTTP 404 Not Found: No matching endpoint found.\n");
    }
}
#endregion
