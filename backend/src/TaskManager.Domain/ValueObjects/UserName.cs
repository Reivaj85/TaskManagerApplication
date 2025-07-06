using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Value object representing a username with validation
/// </summary>
public readonly record struct UserName
{
    public string Value { get; }

    private UserName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a Username from a string with validation
    /// </summary>
    public static Result<UserName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Username cannot be null or empty.";

        var trimmedValue = value.Trim();

        if (trimmedValue.Length < 3)
            return "Username must be at least 3 characters long.";

        if (trimmedValue.Length > 50)
            return "Username cannot exceed 50 characters.";

        return new UserName(trimmedValue);
    }

    public static implicit operator string(UserName userName) => userName.Value;

    public override string ToString() => Value;
}