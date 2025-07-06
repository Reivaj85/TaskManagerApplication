using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public UserName UserName { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Private constructor for deserialization
    private User() { }

    private User(Guid id, UserName username, PasswordHash passwordHash, DateTime createdAt)
    {
        Id = id;
        UserName = username;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Factory method to create a new user with hashed password
    /// </summary>
    /// <param name="username">The username for the new user</param>
    /// <param name="password">The plain text password that will be hashed</param>
    /// <returns>A Result containing the new User instance or error message</returns>
    public static Result<User> Register(string username, string password)
    {
        var usernameResult = UserName.Create(username);
        if (usernameResult.IsFailure)
            return usernameResult.Error;

        var passwordResult = Password.Create(password);
        if (passwordResult.IsFailure)
            return passwordResult.Error;

        var passwordHash = PasswordHash.Create(passwordResult.Value);
        if(passwordHash.IsFailure)
            return passwordHash.Error;

        return new User(
            id: Guid.NewGuid(),
            username: usernameResult.Value,
            passwordHash: passwordHash.Value,
            createdAt: DateTime.UtcNow
        );
    }
    
    public static Result<User> Create(Guid id, UserName username, PasswordHash passwordHash, DateTime createdAt) {
        if ( id == Guid.Empty )
            return "Id cannot be empty.";

        if ( username == null )
            return "Username cannot be null.";

        if ( passwordHash == null )
            return "Password hash cannot be null.";
        
        if ( createdAt == default )
            return "CreatedAt cannot be default value.";

        return new User(id, username, passwordHash, createdAt);
    }

    /// <summary>
    /// Validates if the provided password matches the user's password
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <returns>True if the password is valid, false otherwise</returns>
    public bool ValidatePassword(string password) {
        return !string.IsNullOrWhiteSpace(password) && BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }

    /// <summary>
    /// Updates the user's password with a new hashed password
    /// </summary>
    /// <param name="newPassword">The new password to set</param>
    /// <returns>A Result indicating success or failure</returns>
    public Result ChangePassword(string newPassword)
    {
        var passwordResult = Password.Create(newPassword);
        if (passwordResult.IsFailure)
            return passwordResult.Error;

        var passwordHashResult = PasswordHash.Create(passwordResult.Value);
        if (passwordHashResult.IsFailure)
            return passwordHashResult.Error;

        PasswordHash = passwordHashResult.Value;
        return Result.Success();
    }
}
