using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMastery.Topics.Topic22;

/// <summary>
/// Practical demonstrations for Topic 22: Database Development & Repository Pattern
/// </summary>
public static class DatabaseDemo
{
    // 1. Domain Entity
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    // 2. Repository Pattern (Decoupling storage from business logic)
    public interface ICustomerRepository
    {
        Customer? GetById(int id);
        void Add(Customer customer);
        IEnumerable<Customer> GetAll();
    }

    // In-memory implementation simulating an EF Core / Database backed store
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public Customer? GetById(int id) => _customers.FirstOrDefault(c => c.Id == id);

        public void Add(Customer customer) => _customers.Add(customer);

        public IEnumerable<Customer> GetAll() => _customers.AsReadOnly();
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 22: DATABASE CONCEPTS & REPOSITORY PATTERN");
        Console.WriteLine("=======================================================");

        ICustomerRepository repo = new InMemoryCustomerRepository();
        repo.Add(new Customer { Id = 1, Name = "Kunal Singh", Email = "kunal@example.com" });
        repo.Add(new Customer { Id = 2, Name = "Jane Doe", Email = "jane@example.com" });

        var customer = repo.GetById(1);
        Console.WriteLine($"Retrieved from repository: {customer?.Name} ({customer?.Email})");
        Console.WriteLine($"Total records: {repo.GetAll().Count()}");
    }
}
