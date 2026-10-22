using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AspNetCoreBasics.Authentication;

// ============================================================================
// Secure Salted Password Hashing Utility (PBKDF2)
// ============================================================================

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 50_000;

    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        // Pack: Salt (16 bytes) + Hash (32 bytes)
        byte[] combined = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, combined, SaltSize, HashSize);

        return Convert.ToBase64String(combined);
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        byte[] combined = Convert.FromBase64String(storedHash);
        if (combined.Length != SaltSize + HashSize) return false;

        byte[] salt = new byte[SaltSize];
        byte[] expectedHash = new byte[HashSize];
        Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(combined, SaltSize, expectedHash, 0, HashSize);

        byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        // Constant-time comparison to prevent timing attacks
        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}

// ============================================================================
// Simulated JSON Web Token (JWT) Issuer & Validator
// ============================================================================

public static class SimulatedJwtService
{
    private const string SecretKey = "SuperSecretSigningKeyForEducationalPurposesOnly!";

    public static string GenerateToken(string userId, string name, string role, string department)
    {
        var header = new { alg = "HS256", typ = "JWT" };
        var payload = new Dictionary<string, object>
        {
            ["sub"] = userId,
            ["name"] = name,
            ["role"] = role,
            ["dept"] = department,
            ["exp"] = DateTimeOffset.UtcNow.AddMinutes(15).ToUnixTimeSeconds()
        };

        string headerB64 = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
        string payloadB64 = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));

        string rawSignature = ComputeHmac(headerB64 + "." + payloadB64, SecretKey);

        return $"{headerB64}.{payloadB64}.{rawSignature}";
    }

    public static (bool IsValid, ClaimsPrincipal? Principal, string Error) ValidateToken(string token)
    {
        var parts = token.Split('.');
        if (parts.Length != 3)
            return (false, null, "Malformed token structure (must have 3 dot-delimited segments)");

        string headerB64 = parts[0];
        string payloadB64 = parts[1];
        string signature = parts[2];

        // 1. Verify Cryptographic Signature
        string expectedSignature = ComputeHmac(headerB64 + "." + payloadB64, SecretKey);
        if (!CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(signature),
            Encoding.UTF8.GetBytes(expectedSignature)))
        {
            return (false, null, "Signature verification failed! Token has been tampered with.");
        }

        // 2. Decode payload & extract claims
        try
        {
            byte[] payloadBytes = Base64UrlDecode(payloadB64);
            var claimsDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(payloadBytes);
            if (claimsDict == null) return (false, null, "Invalid JSON payload");

            var claims = new List<Claim>();
            foreach (var (k, v) in claimsDict)
            {
                if (k == "role")
                    claims.Add(new Claim(ClaimTypes.Role, v.GetString() ?? ""));
                else if (k == "name")
                    claims.Add(new Claim(ClaimTypes.Name, v.GetString() ?? ""));
                else
                    claims.Add(new Claim(k, v.ToString()));
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);

            return (true, principal, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, null, $"Payload parsing error: {ex.Message}");
        }
    }

    private static string ComputeHmac(string input, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Base64UrlEncode(hash);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static byte[] Base64UrlDecode(string input)
    {
        string padded = input.Replace('-', '+').Replace('_', '/');
        switch (padded.Length % 4)
        {
            case 2: padded += "=="; break;
            case 3: padded += "="; break;
        }
        return Convert.FromBase64String(padded);
    }
}

// ============================================================================
// Module Demonstration Runner
// ============================================================================

public static class AuthDemo
{
    public static void Run()
    {
        Console.WriteLine("=======================================================");
        Console.WriteLine(" 🔐 MODULE 10: AUTHENTICATION & AUTHORIZATION PIPELINE ");
        Console.WriteLine("=======================================================\n");

        // 1. Password Hashing with Salt & PBKDF2
        Console.WriteLine("[1] Password Security: Salted PBKDF2 Hashing:");
        string plainPassword = "SuperSecurePassword#2026";
        string hashedPassword = PasswordHasher.HashPassword(plainPassword);

        Console.WriteLine($"    • Plaintext Password:  {plainPassword}");
        Console.WriteLine($"    • Salted Stored Hash:  {hashedPassword[..32]}... (Length: {hashedPassword.Length})");
        Console.WriteLine($"    • Correct Pass Match:  {PasswordHasher.VerifyPassword(plainPassword, hashedPassword)}");
        Console.WriteLine($"    • Wrong Pass Match:    {PasswordHasher.VerifyPassword("WrongPassword123", hashedPassword)}\n");

        // 2. Generating JSON Web Token (JWT)
        Console.WriteLine("[2] Issuing Cryptographic JWT Bearer Token:");
        string token = SimulatedJwtService.GenerateToken(
            userId: "usr_9981",
            name: "Alice Montgomery",
            role: "Manager",
            department: "Finance");

        Console.WriteLine($"    • Generated Token:\n      {token}\n");

        // 3. Validating Valid Token & Building ClaimsPrincipal
        Console.WriteLine("[3] Validating Authentic Token (Authorization Middleware):");
        var (isValid, principal, error) = SimulatedJwtService.ValidateToken(token);
        Console.WriteLine($"    • Token Valid:  {isValid}");
        if (isValid && principal != null)
        {
            Console.WriteLine($"    • Identity:     {principal.Identity?.Name} (Authenticated: {principal.Identity?.IsAuthenticated})");
            Console.WriteLine($"    • Has 'Manager' Role: {principal.IsInRole("Manager")}");
            Console.WriteLine($"    • Has 'Admin' Role:   {principal.IsInRole("Admin")}");
            Console.WriteLine($"    • Department Claim:   {principal.FindFirst("dept")?.Value}\n");
        }

        // 4. Demonstrating Tamper Detection
        Console.WriteLine("[4] Tamper Detection (Attempting Privilege Escalation):");
        // Attacker attempts to change payload role to "Admin" without knowing the secret key
        var parts = token.Split('.');
        string tamperedPayload = SimulatedJwtService.GenerateToken("usr_9981", "Alice", "Admin", "Finance").Split('.')[1];
        string hackedToken = $"{parts[0]}.{tamperedPayload}.{parts[2]}";

        var (hackedValid, _, hackError) = SimulatedJwtService.ValidateToken(hackedToken);
        Console.WriteLine($"    • Tampered Token Valid? {hackedValid}");
        Console.WriteLine($"    • Security Trap Caught: {hackError}");
    }
}
