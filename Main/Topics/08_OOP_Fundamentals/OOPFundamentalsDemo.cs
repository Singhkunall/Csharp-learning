using System;

namespace CSharpMastery.Topics.Topic08;

/// <summary>
/// Practical demonstrations for Topic 08: OOP Fundamentals
/// </summary>
public static class OOPFundamentalsDemo
{
    public class BankAccount
    {
        private double _balance; // Private field (encapsulation)

        public string AccountNumber { get; }
        public string Owner { get; set; }

        public BankAccount(string accountNumber, string owner, double initialDeposit)
        {
            AccountNumber = accountNumber;
            Owner = owner;
            _balance = initialDeposit > 0 ? initialDeposit : 0;
        }

        public void Deposit(double amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit must be positive.");
            _balance += amount;
            Console.WriteLine($"[Account {AccountNumber}] Deposited {amount:C}. New Balance: {_balance:C}");
        }

        public bool Withdraw(double amount)
        {
            if (amount <= 0 || amount > _balance) return false;
            _balance -= amount;
            Console.WriteLine($"[Account {AccountNumber}] Withdrew {amount:C}. Remaining: {_balance:C}");
            return true;
        }

        public double GetBalance() => _balance;
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 08: OOP FUNDAMENTALS & ENCAPSULATION");
        Console.WriteLine("=======================================================");

        var account = new BankAccount("ACC-9821", "Kunal", 1000.00);
        account.Deposit(250.00);
        account.Withdraw(100.00);
        Console.WriteLine($"Final Balance verified: {account.GetBalance():C}");
    }
}
