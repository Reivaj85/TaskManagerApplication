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
}