using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IProjectRepository
{
    /// <summary>
    /// Gets a project by its unique identifier
    /// </summary>
    /// <param name="id">The project's ID</param>
    /// <returns>The project if found, null otherwise</returns>
    Task<Project?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all projects belonging to a specific user
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>A collection of projects</returns>
    Task<IEnumerable<Project>> ListAsync(Guid userId);

    /// <summary>
    /// Gets the default project for a specific user
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>The default project if found, null otherwise</returns>
    Task<Project?> GetDefaultProjectAsync(Guid userId);

    /// <summary>
    /// Adds a new project to the repository
    /// </summary>
    /// <param name="project">The project to add</param>
    Task AddAsync(Project project);

    /// <summary>
    /// Updates an existing project in the repository
    /// </summary>
    /// <param name="project">The project to update</param>
    Task UpdateAsync(Project project);

    /// <summary>
    /// Deletes a project from the repository
    /// </summary>
    /// <param name="id">The ID of the project to delete</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a project belongs to a specific user
    /// </summary>
    /// <param name="projectId">The project's ID</param>
    /// <param name="userId">The user's ID</param>
    /// <returns>True if the project belongs to the user, false otherwise</returns>
    Task<bool> BelongsToUserAsync(Guid projectId, Guid userId);
}
