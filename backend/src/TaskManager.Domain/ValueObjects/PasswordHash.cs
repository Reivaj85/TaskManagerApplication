using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

public readonly record struct PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static implicit operator string(PasswordHash passwordHash) => passwordHash.Value;

    public override string ToString() => "***";

    public static Result<PasswordHash> Create(Password value)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(value);
        if (string.IsNullOrWhiteSpace(passwordHash))
            return "Failed to hash password.";

        return new PasswordHash(passwordHash);
    }

    /// <summary>
    /// Creates a PasswordHash from an existing hash string (for loading from database)
    /// </summary>
    /// <param name="hashedValue">The already hashed password value</param>
    /// <returns>A Result containing the PasswordHash or error message</returns>
    public static Result<PasswordHash> CreateFromHash(string hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            return "Password hash cannot be null or empty.";

        return new PasswordHash(hashedValue);
    }
}