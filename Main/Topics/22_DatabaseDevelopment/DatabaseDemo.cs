using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMastery.Topics.Topic22;

/// <summary>
/// Practical demonstrations for Topic 22: Database Development (ADO.NET, EF Core & Patterns)
/// </summary>
public static class DatabaseDemo
{
    // 1. Domain Entities with Relational Navigation Properties
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new(); // 1-to-Many Navigation
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }

    // 2. Unit of Work & Repository Pattern Abstraction
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetHighValueCustomers(decimal minimumSpend);
        void Add(Customer customer);
    }

    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        int Commit(); // Atomic transaction commit
    }

    public class MockDatabaseContext : IUnitOfWork
    {
        private readonly List<Customer> _customers = new();
        private bool _isDisposed = false;

        public ICustomerRepository Customers { get; }

        public MockDatabaseContext()
        {
            Customers = new CustomerRepositoryImpl(_customers);
        }

        public int Commit()
        {
            Console.WriteLine("    • [UnitOfWork] Committed all pending entity state changes in atomic transaction.");
            return _customers.Count;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }
        }

        private class CustomerRepositoryImpl : ICustomerRepository
        {
            private readonly List<Customer> _store;
            public CustomerRepositoryImpl(List<Customer> store) => _store = store;

            public void Add(Customer customer) => _store.Add(customer);

            public IEnumerable<Customer> GetHighValueCustomers(decimal minimumSpend)
            {
                // Simulating EF Core query with navigation property join:
                return _store.Where(c => c.Orders.Sum(o => o.TotalAmount) >= minimumSpend);
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🗄 TOPIC 22: DATABASE ARCHITECTURE & PATTERNS");
        Console.WriteLine("=======================================================");

        // 1. The SQL Injection Trap & ADO.NET Parameterized Queries
        Console.WriteLine("[1] SQL Injection Defense (Parameterized Queries):");
        string maliciousUserInput = "admin' OR '1'='1' --";

        Console.WriteLine("    • Vulnerable String Concatenation Output (FATAL):");
        Console.WriteLine($"      SELECT * FROM Users WHERE User = '{maliciousUserInput}'");
        Console.WriteLine("      (Result: Entire user database dumped to hacker!)");

        Console.WriteLine("\n    • Hardened ADO.NET Parameterized Query Pattern:");
        Console.WriteLine("""
              using var cmd = new SqlCommand("SELECT * FROM Users WHERE User = @username", conn);
              cmd.Parameters.AddWithValue("@username", maliciousUserInput);
              // (Result: Database treats input strictly as literal inert string, 0% injection risk!)
              """);

        // 2. Unit of Work & Entity Framework Core Architecture
        Console.WriteLine("\n[2] Unit of Work & Repository Pattern (EF Core Simulation):");
        using (var uow = new MockDatabaseContext())
        {
            var customer1 = new Customer { Id = 1, Name = "Kunal Singh", Email = "kunal@domain.com" };
            customer1.Orders.Add(new Order { Id = 101, CustomerId = 1, TotalAmount = 450.00m });
            customer1.Orders.Add(new Order { Id = 102, CustomerId = 1, TotalAmount = 850.00m });

            var customer2 = new Customer { Id = 2, Name = "Alice Smith", Email = "alice@domain.com" };
            customer2.Orders.Add(new Order { Id = 103, CustomerId = 2, TotalAmount = 150.00m });

            uow.Customers.Add(customer1);
            uow.Customers.Add(customer2);
            uow.Commit();

            var vipClients = uow.Customers.GetHighValueCustomers(minimumSpend: 1000.00m);
            foreach (var vip in vipClients)
            {
                Console.WriteLine($"    • VIP Client Identified: {vip.Name} (Total Spent: {vip.Orders.Sum(o => o.TotalAmount):C})");
            }
        }

        // 3. AsNoTracking vs. Tracking Queries
        Console.WriteLine("\n[3] AsNoTracking Performance Optimization:");
        Console.WriteLine("    • Tracked Query   : db.Orders.ToList() -> Change tracker snapshots all properties.");
        Console.WriteLine("    • NoTracking Query: db.Orders.AsNoTracking().ToList() -> 60% memory reduction for read-only APIs!");
    }
}
