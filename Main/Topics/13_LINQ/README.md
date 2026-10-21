# Topic 13: LINQ (Language Integrated Query)

## 📖 Theoretical Overview

### 1. What is LINQ?
LINQ brings declarative, SQL-like querying capabilities directly into C#. It supports two syntaxes:
- **Method Syntax (Fluent API):** Uses extension methods on `IEnumerable<T>` with lambda expressions (most common).
- **Query Syntax:** Uses SQL-like declarative keywords (`from x in list where ... select ...`).

### 2. Deferred Execution vs. Immediate Execution
- **Deferred Execution:** Methods like `Where()`, `Select()`, `Take()` return an iterator (`IEnumerable<T>`) that **does not execute** when defined. Execution occurs only when the sequence is enumerated (e.g. `foreach`, `ToList()`, `Count()`).
- **Immediate Execution:** Methods like `ToList()`, `ToArray()`, `First()`, `Count()`, `Sum()` evaluate the query immediately and materialize results into memory.

### 3. `IEnumerable<T>` vs. `IQueryable<T>`
- **`IEnumerable<T>`:** In-memory querying. Filtering and sorting execute on the client machine via .NET delegates.
- **`IQueryable<T>`:** Remote/database querying (e.g. Entity Framework Core). Generates an **Expression Tree** translated into native SQL and executed on the database server.

---

## 📊 Deferred Execution Pipeline

```mermaid
flowchart LR
    Data["In-Memory Source / Database"] --> Query["Define: query = source.Where(...)"]
    Query -.->|"Deferred (Query not executed yet)"| Dormant["Enumerator Dormant"]
    Dormant --> Trigger["Trigger: query.ToList() or foreach"]
    Trigger ==>|"Immediate Execution Pipeline"| Results["Materialized In-Memory Collection"]
```
