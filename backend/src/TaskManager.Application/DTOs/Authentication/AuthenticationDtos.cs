using TaskManager.Application.DTOs.Users;

namespace TaskManager.Application.DTOs.Authentication;

/// <summary>
/// Request DTO for user registration
/// </summary>
public record RegisterRequest
{
    /// <summary>
    /// The username for the new user
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// The password for the new user
    /// </summary>
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// Alias for RegisterRequest
/// </summary>
public record RegisterUserRequest : RegisterRequest;

/// <summary>
/// Request DTO for user login
/// </summary>
public record LoginRequest
{
    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// The password
    /// </summary>
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// Response DTO for authentication operations
/// </summary>
public record AuthenticationResponse
{
    /// <summary>
    /// The JWT token
    /// </summary>
    public string Token { get; init; } = string.Empty;

    /// <summary>
    /// The user information
    /// </summary>
    public UserDto User { get; init; } = null!;

    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime ExpiresAt { get; init; }
}

/// <summary>
/// Alias for AuthenticationResponse
/// </summary>
public record LoginResponse : AuthenticationResponse;

/// <summary>
/// Alias for AuthenticationResponse
/// </summary>
public record RegisterResponse : AuthenticationResponse;
