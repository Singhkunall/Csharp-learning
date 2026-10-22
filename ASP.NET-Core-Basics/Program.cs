using System;
using System.Threading.Tasks;
using AspNetCoreBasics.Introduction;
using AspNetCoreBasics.FirstProject;
using AspNetCoreBasics.Middleware;
using AspNetCoreBasics.Routing;
using AspNetCoreBasics.RestApis;
using AspNetCoreBasics.DependencyInjection;
using AspNetCoreBasics.Configuration;
using AspNetCoreBasics.Mvc;
using AspNetCoreBasics.DatabaseEfCore;
using AspNetCoreBasics.Authentication;
using AspNetCoreBasics.Testing;
using AspNetCoreBasics.Deployment;

Console.WriteLine("===============================================================");
Console.WriteLine("       🌐 ASP.NET CORE BASICS MASTERCLASS CURRICULUM           ");
Console.WriteLine("===============================================================");
Console.WriteLine("Phase 1: Foundations & Architecture");
Console.WriteLine("  [01] Introduction to ASP.NET Core Internals");
Console.WriteLine("  [02] Project Lifecycle & WebApplicationBuilder");
Console.WriteLine("  [03] Request Pipeline & Middleware Interception");
Console.WriteLine("  [04] Routing Engine & Endpoint Dispatching");
Console.WriteLine("Phase 2: Modern Web APIs & Services");
Console.WriteLine("  [05] REST API Architecture & DTO Patterns");
Console.WriteLine("  [06] Dependency Injection & Service Lifetimes");
Console.WriteLine("  [07] Configuration Providers, Environments & Options");
Console.WriteLine("Phase 3: Web Apps & Data Access");
Console.WriteLine("  [08] MVC Architecture & The PRG Pattern");
Console.WriteLine("  [09] Database Integration & EF Core Change Tracking");
Console.WriteLine("  [10] Authentication & Authorization Pipeline");
Console.WriteLine("Phase 4: Quality & Cloud Operations");
Console.WriteLine("  [11] Testing, Mocking & Logging Diagnostics");
Console.WriteLine("  [12] Deployment, Rate Limiting & Performance Optimization");
Console.WriteLine("---------------------------------------------------------------");
Console.WriteLine("Enter module number (1 to 12), or press Enter for ALL modules:");
Console.Write("Choice: ");

string? input = Console.ReadLine()?.Trim();

if (string.IsNullOrEmpty(input) || input.Equals("ALL", StringComparison.OrdinalIgnoreCase))
{
    await RunAllModulesAsync();
}
else if (int.TryParse(input, out int moduleNumber))
{
    await RunSingleModuleAsync(moduleNumber);
}
else
{
    Console.WriteLine("Invalid selection. Exiting.");
}

static async Task RunSingleModuleAsync(int module)
{
    switch (module)
    {
        case 1: IntroductionDemo.Run(); break;
        case 2: FirstProjectDemo.Run(); break;
        case 3: await MiddlewareDemo.RunAsync(); break;
        case 4: RoutingDemo.Run(); break;
        case 5: RestApiDemo.Run(); break;
        case 6: DependencyInjectionDemo.Run(); break;
        case 7: ConfigurationDemo.Run(); break;
        case 8: MvcDemo.Run(); break;
        case 9: await DatabaseEfCoreDemo.RunAsync(); break;
        case 10: AuthDemo.Run(); break;
        case 11: TestingDemo.Run(); break;
        case 12: DeploymentDemo.Run(); break;
        default:
            Console.WriteLine($"Module {module} not recognized. Choose between 1 and 12.");
            break;
    }
}

static async Task RunAllModulesAsync()
{
    IntroductionDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 02...\n");
    FirstProjectDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 03...\n");
    await MiddlewareDemo.RunAsync();
    Console.WriteLine("\nPress any key to continue to Module 04...\n");
    RoutingDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 05...\n");
    RestApiDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 06...\n");
    DependencyInjectionDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 07...\n");
    ConfigurationDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 08...\n");
    MvcDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 09...\n");
    await DatabaseEfCoreDemo.RunAsync();
    Console.WriteLine("\nPress any key to continue to Module 10...\n");
    AuthDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 11...\n");
    TestingDemo.Run();
    Console.WriteLine("\nPress any key to continue to Module 12...\n");
    DeploymentDemo.Run();
    Console.WriteLine("\n✅ All 12 ASP.NET Core Basics modules executed successfully!");
}
