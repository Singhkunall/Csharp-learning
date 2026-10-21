# Topic 21: .NET Platform Architecture & Dependency Injection

## 📖 Theoretical Overview

### 1. Inversion of Control (IoC) & Dependency Injection (DI)
Dependency Injection is a design pattern where an object receives its dependencies from an external assembler (the IoC container) rather than instantiating them directly with `new`.

### 2. Service Lifetimes in .NET
| Lifetime | Resolution Behavior | Best Suited For |
|---|---|---|
| **Transient** (`AddTransient`) | A new instance is created every single time it is requested | Lightweight, stateless services |
| **Scoped** (`AddScoped`) | A single instance is created once per HTTP request / scope | Entity Framework `DbContext`, user context |
| **Singleton** (`AddSingleton`) | Created once on first request; shared across entire application lifetime | In-memory cache, background workers |

---

## 📊 Dependency Injection Lifetimes

```mermaid
flowchart TD
    Req1["HTTP Request 1"] --> Scope1["Scope 1"]
    Req2["HTTP Request 2"] --> Scope2["Scope 2"]

    Scope1 --> T1["Transient Service: New Instance A"]
    Scope1 --> T2["Transient Service: New Instance B"]
    Scope1 --> S1["Scoped Service: Shared Instance 1"]

    Scope2 --> T3["Transient Service: New Instance C"]
    Scope2 --> S2["Scoped Service: Shared Instance 2"]

    Scope1 -.-> Sing["Singleton Service (App-wide single instance)"]
    Scope2 -.-> Sing
```
