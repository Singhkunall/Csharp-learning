using System;
using Xunit;
using CSharpLearning;

namespace CSharpLearning.Tests;

/// <summary>
/// Comprehensive xUnit Test Suite for Calculator
/// Demonstrates [Fact] (single test case) and [Theory] (parameterized data-driven tests).
/// </summary>
public class CalculatorTests
{
    private readonly Calculator _calculator = new();

    // ==========================================
    // 1. [Fact] Tests - Single Scenarios
    // ==========================================

    [Fact]
    public void Add_WithTwoPositiveNumbers_ReturnsTheirSum()
    {
        // Arrange
        int a = 4;
        int b = 5;

        // Act
        int result = _calculator.Add(a, b);

        // Assert
        Assert.Equal(9, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        int numerator = 10;
        int denominator = 0;

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => _calculator.Divide(numerator, denominator));
    }

    // ==========================================
    // 2. [Theory] Tests - Data-Driven Tests
    // ==========================================

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(-5, 5, 0)]
    [InlineData(100, 250, 350)]
    public void Add_VariousInputs_ReturnsCorrectSum(int a, int b, int expected)
    {
        int result = _calculator.Add(a, b);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, 2, 5.0)]
    [InlineData(7, 2, 3.5)]
    [InlineData(-9, 3, -3.0)]
    public void Divide_ValidInputs_ReturnsCorrectQuotient(int a, int b, double expected)
    {
        double result = _calculator.Divide(a, b);
        Assert.Equal(expected, result);
    }
}
