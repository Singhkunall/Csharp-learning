using System;

namespace CSharpMastery.Topics.Topic03;

/// <summary>
/// Practical demonstrations for Topic 03: Operators
/// </summary>
public static class OperatorsDemo
{
    [Flags]
    public enum AccessPrivileges
    {
        None = 0,
        Read = 1 << 0,       // 0001 (1)
        Write = 1 << 1,      // 0010 (2)
        Execute = 1 << 2,    // 0100 (4)
        Admin = 1 << 3       // 1000 (8)
    }

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" ⚙️ TOPIC 03: OPERATORS, PRECEDENCE & TYPE TESTING");
        Console.WriteLine("=======================================================");

        // 1. The Short-Circuiting Bouncer Trap
        Console.WriteLine($"[1] Short-Circuiting Evaluation (&& vs &):");
        int evaluationCounter = 0;
        bool conditionFalse = false;

        // In &&, the right side is SKIPPED because the left side is false
        bool shortCircuitResult = conditionFalse && (++evaluationCounter > 0);
        Console.WriteLine($"    • With '&&': condition was false -> evaluationCounter = {evaluationCounter} (Unchanged!)");

        // In &, both sides are evaluated unconditionally
        bool nonShortCircuitResult = conditionFalse & (++evaluationCounter > 0);
        Console.WriteLine($"    • With '&' : condition was false -> evaluationCounter = {evaluationCounter} (Incremented!)");

        // 2. Prefix (++i) vs Postfix (i++) In Expressions
        Console.WriteLine($"\n[2] Prefix (++x) vs Postfix (x++) Mechanics:");
        int a = 5;
        int b = 5;
        int postfixResult = 10 * a++; // Evaluates 10 * 5 = 50, THEN increments a to 6
        int prefixResult = 10 * ++b;  // Increments b to 6 FIRST, THEN evaluates 10 * 6 = 60

        Console.WriteLine($"    • 10 * a++ (Postfix): Result = {postfixResult}, final a = {a}");
        Console.WriteLine($"    • 10 * ++b (Prefix) : Result = {prefixResult}, final b = {b}");

        // 3. Bitwise Operators & Flag Masking
        Console.WriteLine($"\n[3] Bitwise Operations & Flag Masks:");
        var userPerms = AccessPrivileges.Read | AccessPrivileges.Write; // Combined: 0011 (3)
        Console.WriteLine($"    • Combined Permissions: {userPerms} (Raw integer: {(int)userPerms})");

        bool hasWriteAccess = (userPerms & AccessPrivileges.Write) == AccessPrivileges.Write;
        bool hasAdminAccess = (userPerms & AccessPrivileges.Admin) == AccessPrivileges.Admin;
        Console.WriteLine($"    • Has Write permission: {hasWriteAccess}");
        Console.WriteLine($"    • Has Admin permission: {hasAdminAccess}");

        // 4. Null-Coalescing (?? and ??=)
        Console.WriteLine($"\n[4] Null-Coalescing Operators:");
        string? databaseConfig = null;
        string effectiveConfig = databaseConfig ?? "Standard_Default_Configuration";
        Console.WriteLine($"    • Fallback (??): {effectiveConfig}");

        databaseConfig ??= "Custom_Server_Port_5432";
        Console.WriteLine($"    • Assigned via (??=): {databaseConfig}");
        databaseConfig ??= "Ignored_Second_Assignment";
        Console.WriteLine($"    • Second (??=) call skipped: {databaseConfig}");

        // 5. Type Testing: is, as, and typeof
        Console.WriteLine($"\n[5] Type Inspection (is, as, typeof):");
        object payload = "Enterprise C# Architecture";

        if (payload is string strData)
        {
            Console.WriteLine($"    • 'is' verified string payload length: {strData.Length}");
        }

        object numericBox = 999;
        string? safeCast = numericBox as string; // Will evaluate to null safely without throwing
        Console.WriteLine($"    • 'as' cast from int to string: {(safeCast == null ? "null (Safe cast, 0 exceptions)" : safeCast)}");
        Console.WriteLine($"    • typeof(AccessPrivileges) token: {typeof(AccessPrivileges).FullName}");
    }
}
