namespace TaskManager.Application.DTOs.Users;

/// <summary>
/// User data transfer object
/// </summary>
public record UserDto
{
    /// <summary>
    /// The user's unique identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// When the user was created
    /// </summary>
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Request DTO for changing username
/// </summary>
public record ChangeUsernameRequest
{
    /// <summary>
    /// The new username
    /// </summary>
    public string NewUsername { get; init; } = string.Empty;
}

/// <summary>
/// Request DTO for changing password
/// </summary>
public record ChangePasswordRequest
{
    /// <summary>
    /// The current password
    /// </summary>
    public string CurrentPassword { get; init; } = string.Empty;

    /// <summary>
    /// The new password
    /// </summary>
    public string NewPassword { get; init; } = string.Empty;
}

/// <summary>
/// Request DTO for updating user information
/// </summary>
public record UpdateUserRequest
{
    /// <summary>
    /// The new username (optional)
    /// </summary>
    public string? Username { get; init; }

    /// <summary>
    /// The current password (required when changing username)
    /// </summary>
    public string? CurrentPassword { get; init; }

    /// <summary>
    /// The new password (optional)
    /// </summary>
    public string? NewPassword { get; init; }
}
