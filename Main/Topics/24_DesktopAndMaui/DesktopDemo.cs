using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpMastery.Topics.Topic24;

/// <summary>
/// Practical demonstrations for Topic 24: Desktop & MVVM Pattern
/// </summary>
public static class DesktopDemo
{
    // ViewModel implementing INotifyPropertyChanged
    public class CounterViewModel : INotifyPropertyChanged
    {
        private int _count;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Increment() => Count++;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" TOPIC 24: MVVM ARCHITECTURE & DATA BINDING NOTIFICATIONS");
        Console.WriteLine("=======================================================");

        var vm = new CounterViewModel();
        vm.PropertyChanged += (sender, e) =>
        {
            Console.WriteLine($"[UI DataBinding Triggered]: Property '{e.PropertyName}' changed to {vm.Count}");
        };

        vm.Increment();
        vm.Increment();
    }
}
