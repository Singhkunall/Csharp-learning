using System;

namespace CSharpLearning;

/// <summary>
/// Practical Calculator implementation for demonstrating unit testing.
/// </summary>
public class Calculator
{
    public int Add(int a, int b) => a + b;

    public int Subtract(int a, int b) => a - b;

    public int Multiply(int a, int b) => a * b;

    public double Divide(int a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Division by zero is not allowed.");
        }
        return (double)a / b;
    }
}
