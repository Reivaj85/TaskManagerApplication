using TaskManager.Application.DTOs.Users;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappings;

/// <summary>
/// Extension methods for mapping User entities to DTOs
/// </summary>
public static class UserMappingExtensions
{
    /// <summary>
    /// Maps a User entity to a UserDto
    /// </summary>
    /// <param name="user">The user entity</param>
    /// <returns>The user DTO</returns>
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName.Value,
            CreatedAt = user.CreatedAt
        };
    }
}
