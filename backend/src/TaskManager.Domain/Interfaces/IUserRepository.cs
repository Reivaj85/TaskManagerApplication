using System;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<User?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets a user by their username
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>
    /// Adds a new user to the repository
    /// </summary>
    /// <param name="user">The user to add</param>
    Task AddAsync(User user);

    /// <summary>
    /// Updates an existing user in the repository
    /// </summary>
    /// <param name="user">The user to update</param>
    Task UpdateAsync(User user);

    /// <summary>
    /// Checks if a user with the specified username already exists
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <returns>True if the username exists, false otherwise</returns>
    Task<bool> ExistsAsync(string username);
}
