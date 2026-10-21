using System;

namespace CSharpMastery.Topics.Topic07;

/// <summary>
/// Practical demonstrations for Topic 07: Essential Built-in Types
/// </summary>
public static class BuiltInTypesDemo
{
    [Flags]
    public enum DeviceState : byte
    {
        Offline = 0,
        Connected = 1 << 0,  // 1
        Encrypted = 1 << 1,  // 2
        Syncing = 1 << 2,    // 4
        BatteryLow = 1 << 3  // 8
    }

    // Demonstrating lightweight ValueTuple and records
    public record struct GeoCoordinate(double Latitude, double Longitude);

    public static void Run()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 💎 TOPIC 07: BUILT-IN TYPES, DATES, ENUMS & TUPLES");
        Console.WriteLine("=======================================================");

        // 1. The Banker's Reality: Double vs Decimal
        Console.WriteLine("[1] Financial Accuracy (Double vs Decimal):");
        double doubleA = 0.1;
        double doubleB = 0.2;
        Console.WriteLine($"    • Double binary drift (0.1 + 0.2): {doubleA + doubleB} (NOT exactly 0.3!)");

        decimal decA = 0.1m;
        decimal decB = 0.2m;
        Console.WriteLine($"    • Decimal base-10 exact sum       : {decA + decB} (Pure financial accuracy)");

        // 2. Nullable Value Types (int?)
        Console.WriteLine("\n[2] Nullable Value Types (Nullable<T>):");
        int? remoteSensorTemp = null;
        Console.WriteLine($"    • HasValue: {remoteSensorTemp.HasValue}");
        Console.WriteLine($"    • GetValueOrDefault(-999): {remoteSensorTemp.GetValueOrDefault(-999)}");

        remoteSensorTemp = 28;
        if (remoteSensorTemp.HasValue)
        {
            Console.WriteLine($"    • Sensor online with value: {remoteSensorTemp.Value}°C");
        }

        // 3. Modern Temporal Types: DateOnly, TimeOnly, DateTimeOffset
        Console.WriteLine("\n[3] Temporal Types (DateOnly, TimeOnly, DateTimeOffset, TimeSpan):");
        DateOnly eventDate = new DateOnly(2026, 10, 21);
        TimeOnly eventTime = new TimeOnly(14, 30, 00);
        DateTimeOffset preciseEvent = new DateTimeOffset(2026, 10, 21, 14, 30, 0, TimeSpan.FromHours(5.5)); // IST offset
        TimeSpan sessionDuration = TimeSpan.FromMinutes(90);

        Console.WriteLine($"    • Pure DateOnly:       {eventDate:yyyy-MM-dd} (Zero timezone pollution)");
        Console.WriteLine($"    • Pure TimeOnly:       {eventTime:HH:mm} (Daily recurring time)");
        Console.WriteLine($"    • DateTimeOffset:      {preciseEvent:yyyy-MM-dd HH:mm:ss zzz} (Global event log)");
        Console.WriteLine($"    • TimeSpan Duration:   {sessionDuration.TotalHours} hours ({sessionDuration.TotalSeconds} seconds)");

        // 4. Guid (Globally Unique Identifier)
        Console.WriteLine("\n[4] 128-bit Collision-Proof Identifiers (Guid):");
        Guid transactionId = Guid.NewGuid();
        Console.WriteLine($"    • Generated Guid:      {transactionId}");
        Console.WriteLine($"    • Guid byte array size: {transactionId.ToByteArray().Length} bytes (128 bits)");

        // 5. Bitwise Flag Enums
        Console.WriteLine("\n[5] Bitwise Flag Enums ([Flags]):");
        DeviceState state = DeviceState.Connected | DeviceState.Encrypted;
        Console.WriteLine($"    • Combined Device State: {state} (Raw value: {(byte)state})");
        bool isEncrypted = state.HasFlag(DeviceState.Encrypted);
        Console.WriteLine($"    • Verified HasFlag(Encrypted): {isEncrypted}");

        // 6. ValueTuple & Clean Deconstruction
        Console.WriteLine("\n[6] ValueTuple & Tuple Deconstruction:");
        var serverStatus = QueryServerHealth();
        Console.WriteLine($"    • Full Tuple: Status = {serverStatus.Code}, Latency = {serverStatus.LatencyMs}ms, Alive = {serverStatus.IsAlive}");

        // Deconstruct tuple into independent local variables:
        var (statusCode, latency, _) = QueryServerHealth();
        Console.WriteLine($"    • Deconstructed values: HTTP {statusCode} ({latency} ms latency)");
    }

    private static (int Code, double LatencyMs, bool IsAlive) QueryServerHealth()
    {
        return (200, 14.8, true);
    }
}
