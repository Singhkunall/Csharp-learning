using System;

namespace CSharpMastery.Topics.Topic16;

/// <summary>
/// Practical demonstrations for Topic 16: Delegates, Events and Lambdas
/// </summary>
public static class DelegatesEventsDemo
{
    // Custom Event Arguments
    public class OrderEventArgs : EventArgs
    {
        public int OrderId { get; }
        public double TotalAmount { get; }

        public OrderEventArgs(int orderId, double total)
        {
            OrderId = orderId;
            TotalAmount = total;
        }
    }

    public class OrderProcessor
    {
        // Standard event declaration
        public event EventHandler<OrderEventArgs>? OrderCompleted;

        public void ProcessOrder(int orderId, double amount)
        {
            Console.WriteLine($"Processing Order #{orderId} for {amount:C}...");
            // Thread-safe event invocation
            OrderCompleted?.Invoke(this, new OrderEventArgs(orderId, amount));
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 16: DELEGATES, EVENTS & LAMBDAS");
        Console.WriteLine("=======================================================");

        // 1. Built-in Action and Func delegates
        Action<string> logMessage = msg => Console.WriteLine($"[Action Log]: {msg}");
        Func<int, int, int> multiply = (a, b) => a * b;

        logMessage($"Func result of 7 * 8 = {multiply(7, 8)}");

        // 2. Events & Subscriptions
        var processor = new OrderProcessor();
        processor.OrderCompleted += (sender, e) =>
        {
            Console.WriteLine($"-> Notification received: Order #{e.OrderId} finished successfully for {e.TotalAmount:C}");
        };

        processor.ProcessOrder(101, 249.99);
    }
}
