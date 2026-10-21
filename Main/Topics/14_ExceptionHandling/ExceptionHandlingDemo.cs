using System;
using System.Runtime.ExceptionServices;

namespace CSharpMastery.Topics.Topic14;

/// <summary>
/// Practical demonstrations for Topic 14: Exception Handling Architecture
/// </summary>
public static class ExceptionHandlingDemo
{
    // Custom enterprise domain exception
    public class PaymentProcessingException : Exception
    {
        public string TransactionReference { get; }
        public int GatewayErrorCode { get; }

        public PaymentProcessingException(string message, string txRef, int errorCode, Exception? innerException = null)
            : base(message, innerException)
        {
            TransactionReference = txRef;
            GatewayErrorCode = errorCode;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🛡 TOPIC 14: EXCEPTION HANDLING & STACKTRACE RESILIENCE");
        Console.WriteLine("=======================================================");

        // 1. Exception Filters (when keyword)
        Console.WriteLine("[1] Exception Filters (when keyword - zero stack unwinding):");
        int simulatedHttpError = 503;

        try
        {
            SimulateRemoteCall(simulatedHttpError);
        }
        catch (InvalidOperationException ex) when (simulatedHttpError == 404)
        {
            Console.WriteLine($"    • Handled 404 Not Found: {ex.Message}");
        }
        catch (InvalidOperationException) when (simulatedHttpError == 503)
        {
            Console.WriteLine($"    • Handled 503 Service Unavailable: Circuit breaker triggered!");
        }

        // 2. Inner Exception Wrapping (Preserving root cause)
        Console.WriteLine("\n[2] Inner Exception Wrapping & Custom Domain Exceptions:");
        try
        {
            ExecutePaymentWorkflow("TX-998822", 1500.00);
        }
        catch (PaymentProcessingException ex)
        {
            Console.WriteLine($"    • Outer Domain Exception Caught: {ex.Message}");
            Console.WriteLine($"      Transaction Ref: {ex.TransactionReference} | Gateway Code: {ex.GatewayErrorCode}");
            Console.WriteLine($"      Root Cause Inner Exception: {ex.InnerException?.GetType().Name} - '{ex.InnerException?.Message}'");
        }

        // 3. Proving 'throw;' preserves StackTrace vs 'throw ex;'
        Console.WriteLine("\n[3] Forensic StackTrace Analysis (throw; vs throw ex;):");
        try
        {
            TriggerSafeRethrow();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"    • StackTrace contains origin method 'DeepInternalCrash': {ex.StackTrace?.Contains("DeepInternalCrash")}");
        }

        // 4. ExceptionDispatchInfo (Preserving traces across thread boundaries)
        Console.WriteLine("\n[4] ExceptionDispatchInfo (Cross-Boundary Dispatch):");
        ExceptionDispatchInfo? capturedError = null;

        try
        {
            int.Parse("invalid_integer");
        }
        catch (FormatException ex)
        {
            // Capture pristine stack trace without throwing immediately
            capturedError = ExceptionDispatchInfo.Capture(ex);
        }

        if (capturedError != null)
        {
            Console.WriteLine($"    • Successfully captured exception: {capturedError.SourceException.GetType().Name} ready for cross-thread rethrow!");
        }
    }

    private static void SimulateRemoteCall(int code)
    {
        throw new InvalidOperationException($"Gateway responded with error code {code}");
    }

    private static void ExecutePaymentWorkflow(string txRef, double amount)
    {
        try
        {
            // Low-level network crash
            throw new TimeoutException("Database connection socket timed out after 3000ms.");
        }
        catch (TimeoutException networkEx)
        {
            // Wrap low-level fault inside high-level business exception
            throw new PaymentProcessingException("Card payment settlement failed.", txRef, 504, networkEx);
        }
    }

    private static void TriggerSafeRethrow()
    {
        try
        {
            DeepInternalCrash();
        }
        catch (Exception)
        {
            // 'throw;' preserves DeepInternalCrash in the stack trace!
            // 'throw ex;' would wipe DeepInternalCrash out!
            throw;
        }
    }

    private static void DeepInternalCrash()
    {
        throw new DivideByZeroException("Underlying computational unit divided by zero.");
    }
}
