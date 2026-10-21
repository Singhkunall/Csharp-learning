# 🚀 Modern C# Mastery: Theory & Practical Guide

[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/Language-C%23%2012%2F13-blue.svg)](https://learn.microsoft.com/dotnet/csharp/)
[![Testing](https://img.shields.io/badge/Tested%20with-xUnit-green.svg)](https://xunit.net/)
[![Build](https://img.shields.io/badge/Build-Passing-brightgreen.svg)]()
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)]()

Welcome to the **Modern C# Mastery Guide**! This repository is an all-in-one, highly structured resource that pairs **in-depth theory, memory architecture, and interview fundamentals** with **clean, working, production-grade practical code**.

Whether you are preparing for technical interviews, revising core concepts, or building strong foundational knowledge, this repository is designed to be read, executed, and referenced.

---

## 📑 Table of Contents

1. [Project Architecture](#-project-architecture)
2. [Quickstart: Build & Run](#-quickstart-build--run)
3. [Theory & Practical Curriculum](#-theory--practical-curriculum)
   - [01. Variables, Data Types & Type System](#01-variables-data-types--type-system)
   - [02. Records & Equality Semantics](#02-records--equality-semantics)
   - [03. Pattern Matching & Switch Expressions](#03-pattern-matching--switch-expressions)
   - [04. Memory Architecture: Stack vs Heap & Struct vs Class](#04-memory-architecture-stack-vs-heap--struct-vs-class)
   - [05. Robust Exception Handling & Stack Tracing](#05-robust-exception-handling--stack-tracing)
   - [06. OOP: Classes, Encapsulation & Mutability](#06-oop-classes-encapsulation--mutability)
   - [07. Constructors, Overloading & Initialization](#07-constructors-overloading--initialization)
   - [08. OOP: Abstraction, Inheritance & Polymorphism](#08-oop-abstraction-inheritance--polymorphism)
   - [09. Generics & Generic Collections](#09-generics--generic-collections)
   - [10. LINQ (Language Integrated Query)](#10-linq-language-integrated-query)
   - [11. Asynchronous Programming (async / await)](#11-asynchronous-programming-async--await)
   - [12. Unit Testing Architecture with xUnit](#12-unit-testing-architecture-with-xunit)
4. [Step-by-Step GitHub Push Guide](#-step-by-step-github-push-guide)
5. [License](#-license)

---

## 🏗 Project Architecture

This repository uses a multi-project .NET Solution architecture separating application code from automated tests:

```text
CSharpLearning.sln
│
├── Main/                                      # 🚀 Console Application (Runner)
│   ├── Main.csproj                            # Target: net10.0, Exe
│   ├── Program.cs                             # Interactive Menu & Topic Runner
│   ├── Calculator.cs                          # Domain Class (Target of Tests)
│   └── Topics/                                # 📚 In-Depth Topic Modules
│       ├── 01_BasicsAndDataTypes.cs
│       ├── 02_RecordsAndEquality.cs
│       ├── 03_PatternMatchingAndSwitch.cs
│       ├── 04_ValueVsReferenceTypes.cs
│       ├── 05_ExceptionHandling.cs
│       ├── 06_ClassesAndEncapsulation.cs
│       ├── 07_Constructors.cs
│       ├── 08_Polymorphism.cs
│       ├── 09_GenericsAndCollections.cs
│       ├── 10_LINQ.cs
│       └── 11_AsyncAwait.cs
│
├── main.Tests/                                # 🧪 Automated Test Suite
│   ├── main.Tests.csproj                      # xUnit, Microsoft.NET.Test.Sdk
│   └── CalculatorTests.cs                     # [Fact] & [Theory] Test Implementations
│
├── WebApplication1/                           # 🌐 ASP.NET Core Starter Web API
├── .gitignore                                 # Clean ignore rules for .NET
└── README.md                                  # Complete textbook & reference guide
```

---

## ⚡ Quickstart: Build & Run

### 1. Build the Entire Solution
From the root repository directory:
```powershell
dotnet build
```

### 2. Run the Interactive Topic Runner
Launch the application to explore any topic interactively:
```powershell
dotnet run --project Main
```
*Press `1` through `12` to run an isolated concept, or press `Enter` to run the entire curriculum in sequence.*

### 3. Run Automated Unit Tests
Execute the xUnit test suite across the solution:
```powershell
dotnet test
```
To view detailed test name reporting:
```powershell
dotnet test --logger "console;verbosity=normal"
```

---

## 📚 Theory & Practical Curriculum

---

### 01. Variables, Data Types & Type System
📁 **Code:** [`Main/Topics/01_BasicsAndDataTypes.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/01_BasicsAndDataTypes.cs)

#### 💡 The Theory
- **Common Type System (CTS):** Every type in C# ultimately derives from `System.Object`.
- **Static & Strong Typing:** Types are verified at compile-time, preventing illegal memory operations.
- **Value Types vs Reference Types:** Value types directly store data in memory (Stack), while reference types store memory addresses pointing to objects on the managed Heap.
- **String Immutability:** Strings in C# are reference types that cannot be changed once created. Any string manipulation creates a new object in memory.

#### 💻 Key Concepts in Action
```csharp
string name = "GFG";                  // Immutable reference type
bool isCsharpEasy = true;             // 1-byte value type
int count = 10;                       // 32-bit signed integer
double price = 99.99;                 // 64-bit IEEE floating point
Console.WriteLine($"Name: {name}, Easy: {isCsharpEasy}"); // String interpolation
```

---

### 02. Records & Equality Semantics
📁 **Code:** [`Main/Topics/02_RecordsAndEquality.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/02_RecordsAndEquality.cs)

#### 💡 The Theory
- **Reference Equality:** By default, standard classes compare object identities (memory addresses). Two distinct class objects with identical property values evaluate to `false` when checked with `==`.
- **Value Equality (`record`):** Introduced in C# 9, `record` types synthesize compiler-generated equality methods (`Equals`, `GetHashCode`, `==`, `!=`) based on property values rather than pointer references.
- **Immutability & `with` Expression:** Records encourage non-destructive mutation (`var p2 = p1 with { Age = 31 };`), which is vital for thread-safety and modern architectural patterns (CQRS, DDD).

```csharp
public record Person(string Name, int Age);

var p1 = new Person("Alice", 30);
var p2 = new Person("Alice", 30);

p1 == p2;                 // TRUE  -> Value equality compares data
ReferenceEquals(p1, p2);  // FALSE -> Different memory addresses on heap
```

---

### 03. Pattern Matching & Switch Expressions
📁 **Code:** [`Main/Topics/03_PatternMatchingAndSwitch.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/03_PatternMatchingAndSwitch.cs)

#### 💡 The Theory
- Modern C# replaced verbose, error-prone `switch` statements with concise, functional **Switch Expressions**.
- **Relational Patterns (`>=`, `<`):** Directly match values against numeric ranges without repeated `if-else` blocks.
- **Logical Patterns (`and`, `or`, `not`):** Combine conditions cleanly.
- **The Discard Pattern (`_`):** Acts as the mandatory fallback (analogous to `default`).

```csharp
public static string GetDayType(int day) => day switch
{
    0 or 6        => "Weekend",
    >= 1 and <= 5 => "Weekday",
    _             => "Invalid"
};
```

---

### 04. Memory Architecture: Stack vs Heap & Struct vs Class
📁 **Code:** [`Main/Topics/04_ValueVsReferenceTypes.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/04_ValueVsReferenceTypes.cs)

#### 💡 The Theory
| Feature | `struct` (Value Type) | `class` (Reference Type) |
|---|---|---|
| **Memory Allocation** | Stack (or inlined within containing object) | Heap (pointer on Stack) |
| **Assignment Behavior** | **Deep Copy:** Copies the actual data | **Shallow Copy:** Copies the reference pointer |
| **Garbage Collector** | Cleaned up immediately when out of scope | Collected asynchronously by the .NET GC |
| **Inheritance** | Does not support inheritance (can implement interfaces) | Full class inheritance hierarchy |

#### ⚠️ The Classic Trap
```csharp
// CLASS (Reference)
var c1 = new PersonClass { Age = 40 };
var c2 = c1;
c2.Age = 45; // Mutates BOTH c1 and c2! c1.Age is now 45.

// STRUCT (Value)
var s1 = new PointStruct(10, 20);
var s2 = s1;
s2.X = 99;   // Modifies ONLY s2! s1.X remains 10.
```

---

### 05. Robust Exception Handling & Stack Tracing
📁 **Code:** [`Main/Topics/05_ExceptionHandling.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/05_ExceptionHandling.cs)

#### 💡 The Theory
- **Exception Hierarchy:** All runtime errors derive from `System.Exception`.
- **StackTrace:** Tracks the exact call chain from where an error originated through caller methods.
- **InnerException Pattern:** When catching low-level exceptions (e.g. `FormatException`), wrap them inside meaningful business-domain exceptions (`InvalidOperationException`) using `throw new MyException("reason", innerEx);` to preserve root-cause telemetry.
- **Exception Ordering:** Always catch specific exceptions (`IndexOutOfRangeException`) before general exceptions (`Exception`).

```csharp
try
{
    int.Parse("abc");
}
catch (FormatException ex)
{
    // Wrap and preserve original cause
    throw new InvalidOperationException("Failed to load settings.", ex);
}
```

---

### 06. OOP: Classes, Encapsulation & Mutability
📁 **Code:** [`Main/Topics/06_ClassesAndEncapsulation.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/06_ClassesAndEncapsulation.cs)

#### 💡 The Theory
- **Encapsulation:** Protect internal state by making fields `private` and exposing controlled access via `public` methods and properties.
- **`const` vs `readonly`:**
  - `const`: Evaluated at **compile-time**. Value is baked into call sites.
  - `readonly`: Evaluated at **runtime**. Can be assigned dynamically during declaration or inside the constructor.

```csharp
public class BankAccount
{
    private double _balance; // Encapsulated private field

    public void Deposit(double amount)
    {
        if (amount > 0) _balance += amount; // Validated state modification
    }
}
```

---

### 07. Constructors, Overloading & Initialization
📁 **Code:** [`Main/Topics/07_Constructors.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/07_Constructors.cs)

#### 💡 The Theory
- **Constructors** guarantee that an object enters memory in a valid state.
- **Constructor Overloading:** Providing multiple constructor signatures allows flexibility (default values vs explicit configuration).
- **Constructor Chaining (`this()`):** Forwards initialization logic to a master constructor to eliminate code duplication.

```csharp
public class Car
{
    public string Brand { get; }
    public double Price { get; }

    public Car() : this("Unknown", 0) { } // Chaining to parameterized constructor

    public Car(string brand, double price)
    {
        Brand = brand;
        Price = price;
    }
}
```

---

### 08. OOP: Abstraction, Inheritance & Polymorphism
📁 **Code:** [`Main/Topics/08_Polymorphism.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/08_Polymorphism.cs)

#### 💡 The Theory
- **Abstraction:** Hides implementation details and exposes only an interface contract (`abstract class Animal`).
- **Polymorphism:** The ability of different derived types (`Dog`, `Cat`, `Cow`) to be treated uniformly through a base type reference, while executing their specific behavior via dynamic method dispatch (`override`).

```csharp
public abstract class Animal
{
    public abstract string Speak();
}

public class Dog : Animal { public override string Speak() => "Woof!"; }
public class Cat : Animal { public override string Speak() => "Meow!"; }

// Polymorphic execution:
List<Animal> animals = [new Dog(), new Cat()];
foreach (var animal in animals)
{
    Console.WriteLine(animal.Speak()); // Dispatches to correct subclass at runtime
}
```

---

### 09. Generics & Generic Collections
📁 **Code:** [`Main/Topics/09_GenericsAndCollections.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/09_GenericsAndCollections.cs)

#### 💡 The Theory
- **Generics (`<T>`):** Enable classes and methods to work with any data type while maintaining full compile-time type safety.
- **No Boxing/Unboxing Overhead:** Unlike legacy non-generic collections (`ArrayList`), generic collections (`List<T>`) store value types directly without heap allocation.
- **Generic Constraints:** Use `where T : class`, `where T : new()`, or `where T : IComparable` to enforce capabilities.

```csharp
public class Repository<T> where T : class
{
    private readonly List<T> _items = new();
    public void Add(T item) => _items.Add(item);
}
```

---

### 10. LINQ (Language Integrated Query)
📁 **Code:** [`Main/Topics/10_LINQ.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/10_LINQ.cs)

#### 💡 The Theory
- **Declarative vs Imperative:** Express *what* data you need rather than *how* to loop through it.
- **Deferred Execution:** LINQ query definitions (`Where`, `Select`) do **not** execute immediately. They only run when the collection is iterated (e.g. `foreach`, `.ToList()`, `.Count()`).

```csharp
var topPerformers = employees
    .Where(e => e.YearsExperience >= 5)
    .OrderByDescending(e => e.Salary)
    .Select(e => e.Name)
    .ToList(); // Materialized immediately
```

---

### 11. Asynchronous Programming (async / await)
📁 **Code:** [`Main/Topics/11_AsyncAwait.cs`](file:///C:/Users/TE-164/Desktop/k/Main/Topics/11_AsyncAwait.cs)

#### 💡 The Theory
- **Non-blocking I/O:** `async` and `await` yield the calling thread back to the thread pool while waiting for external operations (network, disk, database), preventing UI freezes and maximizing web server throughput.
- **`Task` vs `void`:** Always return `Task` or `Task<T>`. Never write `async void` except in UI event handlers, as `async void` exceptions cannot be caught and crash the process.
- **Concurrency:** `Task.WhenAll(...)` allows running independent tasks concurrently.

```csharp
public async Task<string> FetchDataAsync()
{
    await Task.Delay(500); // Non-blocking asynchronous delay
    return "Data loaded";
}
```

---

### 12. Unit Testing Architecture with xUnit
📁 **Code:** [`main.Tests/CalculatorTests.cs`](file:///C:/Users/TE-164/Desktop/k/main.Tests/CalculatorTests.cs)

#### 💡 The Theory
- **AAA Pattern (Arrange, Act, Assert):** Standard structure for predictable, readable tests.
- **`[Fact]`:** Tests a single deterministic scenario without arguments.
- **`[Theory]` & `[InlineData]`:** Data-driven testing that runs the same test logic across multiple input/output permutations.
- **Exception Testing:** Verifies that code fails safely using `Assert.Throws<TException>(...)`.

```csharp
[Theory]
[InlineData(1, 2, 3)]
[InlineData(10, 20, 30)]
public void Add_ValidNumbers_ReturnsCorrectSum(int a, int b, int expected)
{
    // Arrange
    var calc = new Calculator();

    // Act
    int result = calc.Add(a, b);

    // Assert
    Assert.Equal(expected, result);
}
```

---

## 🐙 Step-by-Step GitHub Push Guide

To push this entire structured project to your GitHub account:

### 1. Initialize Git in the project root
Open PowerShell or your terminal in `C:\Users\TE-164\Desktop\k`:
```powershell
git init
```

### 2. Stage all clean files
*(The included `.gitignore` will automatically prevent compiled binaries like `bin/` and `obj/` from being staged)*
```powershell
git add .
```

### 3. Verify staged files
```powershell
git status
```

### 4. Create your initial commit
```powershell
git commit -m "Initial commit: Complete C# Theory & Practical Learning Roadmap"
```

### 5. Create a new repository on GitHub
1. Go to [github.com/new](https://github.com/new).
2. Name your repository (e.g. `csharp-mastery-guide` or `csharp-learning`).
3. Set visibility to **Public** (or **Private**).
4. Do **not** check "Add a README" or ".gitignore" (we already created them locally).
5. Click **Create repository**.

### 6. Link and push to GitHub
Run the following commands in your terminal (replace `YOUR_USERNAME` and `YOUR_REPO` with your GitHub info):
```powershell
git branch -M main
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO.git
git push -u origin main
```

---

## 📜 License

This project is licensed under the MIT License - feel free to use it for learning, teaching, and interview preparation.
