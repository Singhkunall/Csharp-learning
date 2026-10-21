using System;
using System.Collections.Generic;

namespace CSharpMastery.Topics.Topic09;

/// <summary>
/// Practical demonstrations for Topic 09: OOP Inheritance and Polymorphism
/// </summary>
public static class OOPPolymorphismDemo
{
    public interface INotificationService
    {
        void Send(string message);
    }

    public abstract class NotificationBase : INotificationService
    {
        public string Sender { get; }

        protected NotificationBase(string sender)
        {
            Sender = sender;
        }

        public abstract void Send(string message);

        public virtual void Log(string message)
        {
            Console.WriteLine($"[Audit] Logged delivery for {Sender}: \"{message}\"");
        }
    }

    public class EmailNotification : NotificationBase
    {
        public EmailNotification(string sender) : base(sender) { }

        public override void Send(string message)
        {
            Console.WriteLine($"📧 [Email from {Sender}]: {message}");
            Log(message);
        }
    }

    public class SmsNotification : NotificationBase
    {
        public SmsNotification(string sender) : base(sender) { }

        public override void Send(string message)
        {
            Console.WriteLine($"📱 [SMS from {Sender}]: {message}");
            Log(message);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 09: INHERITANCE, ABSTRACTION & POLYMORPHISM");
        Console.WriteLine("=======================================================");

        var dispatchers = new List<INotificationService>
        {
            new EmailNotification("admin@system.com"),
            new SmsNotification("+1-555-0199")
        };

        foreach (var service in dispatchers)
        {
            service.Send("Critical system security update scheduled.");
        }
    }
}
