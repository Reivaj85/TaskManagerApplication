using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Value object representing a password with validation
/// </summary>
public readonly record struct Password
{
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a Password from a string with validation
    /// </summary>
    public static Result<Password> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<Password>.Failure("Password cannot be empty.");

        return value.Length switch {
            < 6 => Result<Password>.Failure("Password must be at least 6 characters long.")
          , > 100 => Result<Password>.Failure("Password must be at most 100 characters long.")
          , _ => Result<Password>.Success(new Password(value))
        };
    }

    public static implicit operator string(Password password) => password.Value;

    public override string ToString() => "***";
}