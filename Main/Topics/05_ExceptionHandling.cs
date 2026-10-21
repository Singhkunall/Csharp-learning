using System;

namespace CSharpLearning.Topics;

/// <summary>
/// Topic 5: Comprehensive Exception Handling
/// Covers try/catch, exception types, StackTrace, InnerException, and throwing exceptions.
/// </summary>
public static class ExceptionHandling
{
    public static void Run()
    {
        Console.WriteLine("\n==========================================");
        Console.WriteLine(" 05. EXCEPTION HANDLING");
        Console.WriteLine("==========================================");

        DemoBasicCatch();
        DemoExceptionDetails();
        DemoStackTrace();
        DemoInnerException();
        DemoThrowingValidation();
    }

    // 1. Basic try/catch
    private static void DemoBasicCatch()
    {
        Console.WriteLine("\n--- 1. Basic Try / Catch (IndexOutOfRange) ---");
        try
        {
            int[] arr = { 1, 2, 3 };
            Console.WriteLine(arr[5]); // Out of bounds
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Caught IndexOutOfRangeException: Index was outside the array bounds.");
        }
    }

    // 2. Exception properties: Message and Type Name
    private static void DemoExceptionDetails()
    {
        Console.WriteLine("\n--- 2. Inspecting Exception Details ---");
        try
        {
            int a = 10;
            int b = 0;
            int result = a / b;
            Console.WriteLine(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"Exception Message: {ex.Message}");
        }
    }

    // 3. StackTrace tracking
    private static void DemoStackTrace()
    {
        Console.WriteLine("\n--- 3. Exploring the StackTrace ---");
        try
        {
            Level1();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }

    private static void Level1() => Level2();
    private static void Level2()
    {
        int x = 5 / int.Parse("0");
    }

    // 4. InnerException wrapping
    private static void DemoInnerException()
    {
        Console.WriteLine("\n--- 4. InnerException & Re-throwing ---");
        try
        {
            LoadSettings();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Outer Exception: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }

    private static void LoadSettings()
    {
        try
        {
            int.Parse("abc"); // Will fail with FormatException
        }
        catch (Exception ex)
        {
            // Wrap the original exception inside an InvalidOperationException
            throw new InvalidOperationException("Could not load configuration settings.", ex);
        }
    }

    // 5. Throwing validation exceptions
    private static void DemoThrowingValidation()
    {
        Console.WriteLine("\n--- 5. Throwing Exceptions for Validation ---");
        try
        {
            Console.WriteLine($"Valid Score 85: {GetGrade(85)}");
            Console.WriteLine($"Invalid Score -5: {GetGrade(-5)}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation Caught: {ex.Message}");
        }
    }

    private static string GetGrade(int score)
    {
        if (score < 0 || score > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100.");
        }
        return score >= 50 ? "Pass" : "Fail";
    }
}
