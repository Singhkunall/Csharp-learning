using System;

namespace CSharpMastery.Topics.Topic12;

/// <summary>
/// Practical demonstrations for Topic 12: Generics
/// </summary>
public static class GenericsDemo
{
    // Generic Result Wrapper with constraint
    public class OperationResult<T>
    {
        public bool Success { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }

        private OperationResult(bool success, T? data, string? error)
        {
            Success = success;
            Data = data;
            ErrorMessage = error;
        }

        public static OperationResult<T> Ok(T data) => new(true, data, null);
        public static OperationResult<T> Fail(string error) => new(false, default, error);
    }

    // Generic Method with multiple constraints
    public static T CreateAndInitialize<T>() where T : class, new()
    {
        return new T();
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 12: GENERICS & TYPE CONSTRAINTS");
        Console.WriteLine("=======================================================");

        var result = OperationResult<int>.Ok(200);
        Console.WriteLine($"Result Success: {result.Success}, Data: {result.Data}");

        var failResult = OperationResult<string>.Fail("Resource not found");
        Console.WriteLine($"Failed Result: {failResult.ErrorMessage}");
    }
}
