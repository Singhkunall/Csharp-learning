using System;

namespace CSharpMastery.Topics.Topic14;

/// <summary>
/// Practical demonstrations for Topic 14: Exception Handling
/// </summary>
public static class ExceptionHandlingDemo
{
    public class InsufficientFundsException : Exception
    {
        public double AttemptedAmount { get; }
        public double CurrentBalance { get; }

        public InsufficientFundsException(double attempted, double balance)
            : base($"Attempted to withdraw {attempted:C}, but balance is only {balance:C}.")
        {
            AttemptedAmount = attempted;
            CurrentBalance = balance;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 14: EXCEPTION HANDLING & EXCEPTION FILTERS");
        Console.WriteLine("=======================================================");

        // 1. Exception filter with 'when'
        int httpErrorCode = 404;
        try
        {
            SimulateNetworkCall(httpErrorCode);
        }
        catch (InvalidOperationException ex) when (httpErrorCode == 404)
        {
            Console.WriteLine($"Handled via Filter (when code == 404): {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Generic fallback: {ex.Message}");
        }

        // 2. Custom Business Exception
        try
        {
            Withdraw(500, 200);
        }
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Custom Exception Caught: {ex.Message}");
        }
    }

    private static void SimulateNetworkCall(int statusCode)
    {
        if (statusCode >= 400)
            throw new InvalidOperationException($"Remote service returned status {statusCode}");
    }

    private static void Withdraw(double amount, double balance)
    {
        if (amount > balance)
            throw new InsufficientFundsException(amount, balance);
    }
}
