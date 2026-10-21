using System;

namespace CSharpMastery.Topics.Topic21;

/// <summary>
/// Practical demonstrations for Topic 21: .NET Platform & Dependency Injection
/// </summary>
public static class PlatformDemo
{
    // 1. Dependency Abstraction
    public interface IMessageService
    {
        void Send(string message);
    }

    public class ConsoleMessageService : IMessageService
    {
        public void Send(string message) => Console.WriteLine($"[DI Resolved Output]: {message}");
    }

    // 2. Consumer receiving dependency via Constructor Injection
    public class OrderNotificationHandler
    {
        private readonly IMessageService _messageService;

        public OrderNotificationHandler(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void NotifyUser()
        {
            _messageService.Send("Your order has been confirmed.");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 21: .NET PLATFORM & CONSTRUCTOR INJECTION");
        Console.WriteLine("=======================================================");

        // Manual composition simulating DI Container resolution
        IMessageService service = new ConsoleMessageService();
        var handler = new OrderNotificationHandler(service);
        handler.NotifyUser();
    }
}
