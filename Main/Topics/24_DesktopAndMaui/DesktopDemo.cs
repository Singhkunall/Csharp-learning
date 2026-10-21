using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CSharpMastery.Topics.Topic24;

/// <summary>
/// Practical demonstrations for Topic 24: Desktop & Cross-Platform MVVM Architecture
/// </summary>
public static class DesktopDemo
{
    // 1. Reusable RelayCommand (ICommand implementation)
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    // 2. ViewModel implementing INotifyPropertyChanged
    public class CheckoutViewModel : INotifyPropertyChanged
    {
        private string _customerName = string.Empty;
        private decimal _totalAmount = 0m;
        private bool _isOrderPlaced = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand PlaceOrderCommand { get; }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;
                    OnPropertyChanged();
                    // Re-evaluate whether the checkout button should be enabled:
                    ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged();
                    ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public CheckoutViewModel()
        {
            // Button is only enabled if customer name is entered AND amount > 0:
            PlaceOrderCommand = new RelayCommand(
                execute: _ => ExecuteOrderPlacement(),
                canExecute: _ => !string.IsNullOrWhiteSpace(CustomerName) && TotalAmount > 0 && !_isOrderPlaced
            );
        }

        private void ExecuteOrderPlacement()
        {
            _isOrderPlaced = true;
            Console.WriteLine($"    • [ViewModel Action] Order successfully submitted for {CustomerName} ({TotalAmount:C})!");
            ((RelayCommand)PlaceOrderCommand).RaiseCanExecuteChanged();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 🖥 TOPIC 24: MVVM PATTERN, ICOMMAND & DATA BINDING");
        Console.WriteLine("=======================================================");

        var vm = new CheckoutViewModel();

        // Simulating the XAML View listening to property changes via data binding:
        vm.PropertyChanged += (sender, e) =>
        {
            Console.WriteLine($"    • [XAML View Notified] Property '{e.PropertyName}' changed on ViewModel.");
        };

        Console.WriteLine("[1] Initial State Evaluation:");
        Console.WriteLine($"    • Can Execute Checkout Command: {vm.PlaceOrderCommand.CanExecute(null)} (False - missing name & amount)");

        Console.WriteLine("\n[2] User Types Name in UI (Two-Way Binding Simulation):");
        vm.CustomerName = "Kunal Singh";
        Console.WriteLine($"    • Can Execute Checkout Command: {vm.PlaceOrderCommand.CanExecute(null)} (False - amount is still $0)");

        Console.WriteLine("\n[3] Shopping Cart Computes Total (Two-Way Binding):");
        vm.TotalAmount = 249.99m;
        Console.WriteLine($"    • Can Execute Checkout Command: {vm.PlaceOrderCommand.CanExecute(null)} (TRUE - Button lights up!)");

        Console.WriteLine("\n[4] User Clicks Checkout Button (Triggering ICommand):");
        if (vm.PlaceOrderCommand.CanExecute(null))
        {
            vm.PlaceOrderCommand.Execute(null);
        }
        Console.WriteLine($"    • Post-Checkout Button Enabled State: {vm.PlaceOrderCommand.CanExecute(null)} (Disabled to prevent double clicks!)");
    }
}
