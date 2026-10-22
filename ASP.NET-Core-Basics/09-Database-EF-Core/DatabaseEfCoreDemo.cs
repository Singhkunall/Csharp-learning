using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreBasics.DatabaseEfCore;

// ============================================================================
// Entity Domain Model (1 Author : N Books)
// ============================================================================

public class Author
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = new();
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }
}

public enum SimulatedEntityState
{
    Detached,
    Unchanged,
    Added,
    Modified,
    Deleted
}

public class EntityEntry<T> where T : class
{
    public T Entity { get; }
    public SimulatedEntityState State { get; set; }
    public Dictionary<string, object?> OriginalValues { get; } = new();

    public EntityEntry(T entity, SimulatedEntityState state)
    {
        Entity = entity;
        State = state;
    }
}

// ============================================================================
// Simulated In-Memory DbContext & Change Tracker Engine
// ============================================================================

public class SimulatedDbContext
{
    private readonly List<EntityEntry<object>> _tracker = new();
    private static readonly List<Author> _sqlStorageAuthors = new();
    private static readonly List<Book> _sqlStorageBooks = new();
    private static int _authorIdSeq = 1;
    private static int _bookIdSeq = 1;

    static SimulatedDbContext()
    {
        // Seed database
        var author = new Author { Id = _authorIdSeq++, FullName = "Andrew Troelsen" };
        _sqlStorageAuthors.Add(author);
        _sqlStorageBooks.Add(new Book { Id = _bookIdSeq++, Title = "Pro C# 10 with .NET 6", Price = 49.99m, AuthorId = author.Id });
        _sqlStorageBooks.Add(new Book { Id = _bookIdSeq++, Title = "C# and the .NET Platform", Price = 39.50m, AuthorId = author.Id });
    }

    public List<EntityEntry<object>> ChangeTracker => _tracker;

    public void Add<T>(T entity) where T : class
    {
        _tracker.Add(new EntityEntry<object>(entity, SimulatedEntityState.Added));
        Console.WriteLine($"    [ChangeTracker] Attached {typeof(T).Name} as State = Added");
    }

    public void Update<T>(T entity) where T : class
    {
        var entry = _tracker.FirstOrDefault(e => ReferenceEquals(e.Entity, entity));
        if (entry != null)
        {
            entry.State = SimulatedEntityState.Modified;
        }
        else
        {
            _tracker.Add(new EntityEntry<object>(entity, SimulatedEntityState.Modified));
        }
        Console.WriteLine($"    [ChangeTracker] Marked {typeof(T).Name} as State = Modified");
    }

    public void Remove<T>(T entity) where T : class
    {
        var entry = _tracker.FirstOrDefault(e => ReferenceEquals(e.Entity, entity));
        if (entry != null)
        {
            entry.State = SimulatedEntityState.Deleted;
        }
        else
        {
            _tracker.Add(new EntityEntry<object>(entity, SimulatedEntityState.Deleted));
        }
        Console.WriteLine($"    [ChangeTracker] Marked {typeof(T).Name} as State = Deleted");
    }

    public Task<List<Book>> GetBooksQueryAsync(bool asNoTracking = false)
    {
        Console.WriteLine($"    [SQL Engine] Executing: SELECT [Id], [Title], [Price], [AuthorId] FROM [Books]");
        var results = _sqlStorageBooks.Select(b => new Book
        {
            Id = b.Id,
            Title = b.Title,
            Price = b.Price,
            AuthorId = b.AuthorId
        }).ToList();

        if (!asNoTracking)
        {
            foreach (var b in results)
            {
                _tracker.Add(new EntityEntry<object>(b, SimulatedEntityState.Unchanged));
            }
            Console.WriteLine($"    [ChangeTracker] Tracked {results.Count} entities as 'Unchanged' in memory.");
        }
        else
        {
            Console.WriteLine($"    [ChangeTracker] AsNoTracking(): 0 entities snapshot allocated in memory.");
        }

        return Task.FromResult(results);
    }

    public Task<int> SaveChangesAsync()
    {
        Console.WriteLine("\n    --- Executing SaveChangesAsync() Transaction ---");
        int changes = 0;

        foreach (var entry in _tracker.ToList())
        {
            switch (entry.State)
            {
                case SimulatedEntityState.Added:
                    if (entry.Entity is Book b)
                    {
                        b.Id = _bookIdSeq++;
                        _sqlStorageBooks.Add(b);
                        Console.WriteLine($"    [SQL] INSERT INTO [Books] ([Title], [Price], [AuthorId]) VALUES ('{b.Title}', {b.Price}, {b.AuthorId});");
                    }
                    entry.State = SimulatedEntityState.Unchanged;
                    changes++;
                    break;

                case SimulatedEntityState.Modified:
                    if (entry.Entity is Book modBook)
                    {
                        var target = _sqlStorageBooks.First(x => x.Id == modBook.Id);
                        target.Price = modBook.Price;
                        Console.WriteLine($"    [SQL] UPDATE [Books] SET [Price] = {modBook.Price} WHERE [Id] = {modBook.Id};");
                    }
                    entry.State = SimulatedEntityState.Unchanged;
                    changes++;
                    break;

                case SimulatedEntityState.Deleted:
                    if (entry.Entity is Book delBook)
                    {
                        _sqlStorageBooks.RemoveAll(x => x.Id == delBook.Id);
                        Console.WriteLine($"    [SQL] DELETE FROM [Books] WHERE [Id] = {delBook.Id};");
                    }
                    entry.State = SimulatedEntityState.Detached;
                    _tracker.Remove(entry);
                    changes++;
                    break;
            }
        }
        Console.WriteLine($"    ✓ Transaction committed. {changes} row(s) affected.\n");
        return Task.FromResult(changes);
    }
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class DatabaseEfCoreDemo
{
    public static void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public static async Task RunAsync()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🗄️ MODULE 09: DATABASE INTEGRATION & EF CORE TRACKING ");
        Console.WriteLine("=======================================================\n");

        var db = new SimulatedDbContext();

        // 1. Asynchronous Read with AsNoTracking vs Tracked
        Console.WriteLine("[1] Executing Read Query (Tracked):");
        var trackedBooks = await db.GetBooksQueryAsync(asNoTracking: false);
        foreach (var b in trackedBooks)
        {
            Console.WriteLine($"    • Book #{b.Id}: '{b.Title}' - ${b.Price}");
        }

        Console.WriteLine("\n[2] Executing High-Throughput Read Query (AsNoTracking):");
        var readOnlyBooks = await db.GetBooksQueryAsync(asNoTracking: true);

        // 3. Creating a new Entity (State: Added)
        Console.WriteLine("\n[3] Adding a New Entity to DbContext:");
        var newBook = new Book
        {
            Title = "C# 13 in a Nutshell",
            Price = 54.99m,
            AuthorId = 1
        };
        db.Add(newBook);
        Console.WriteLine($"    Before SaveChanges: Entity ID = {newBook.Id} (Pending key generation)");
        await db.SaveChangesAsync();
        Console.WriteLine($"    After SaveChanges:  Entity ID = {newBook.Id} (Generated by SQL Server)\n");

        // 4. Updating an Entity (State: Modified)
        Console.WriteLine("[4] Updating an Existing Tracked Entity:");
        var bookToUpdate = trackedBooks.First();
        Console.WriteLine($"    Original Price: ${bookToUpdate.Price}");
        bookToUpdate.Price = 44.95m; // Mutating tracked object
        db.Update(bookToUpdate);
        await db.SaveChangesAsync();

        // 5. Deleting an Entity (State: Deleted)
        Console.WriteLine("[5] Deleting an Entity:");
        db.Remove(newBook);
        await db.SaveChangesAsync();

        // 6. Demonstrating the N+1 Query Trap vs Eager Loading
        Console.WriteLine("[6] Architectural Concept: The N+1 Query Anti-Pattern:");
        Console.WriteLine("    ❌ N+1 Pattern: 1 query for 50 books + 50 separate queries for Author = 51 database roundtrips!");
        Console.WriteLine("    ✅ Eager Loading: context.Books.Include(b => b.Author).ToListAsync()");
        Console.WriteLine("       Generates 1 single SQL JOIN query: SELECT b.*, a.* FROM Books b INNER JOIN Authors a ON b.AuthorId = a.Id;");
    }
}
