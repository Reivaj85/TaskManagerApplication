using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    /// <summary>
    /// Gets a task by its unique identifier
    /// </summary>
    /// <param name="id">The task's ID</param>
    /// <returns>The task if found, null otherwise</returns>
    Task<TaskItem?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets all tasks belonging to a specific project
    /// </summary>
    /// <param name="projectId">The project's ID</param>
    /// <returns>A collection of tasks</returns>
    Task<IEnumerable<TaskItem>> ListAsync(Guid projectId);

    /// <summary>
    /// Gets all tasks belonging to projects owned by a specific user
    /// </summary>
    /// <param name="userId">The user's ID</param>
    /// <returns>A collection of tasks</returns>
    Task<IEnumerable<TaskItem>> ListByUserAsync(Guid userId);

    /// <summary>
    /// Gets completed tasks for a specific project
    /// </summary>
    /// <param name="projectId">The project's ID</param>
    /// <returns>A collection of completed tasks</returns>
    Task<IEnumerable<TaskItem>> GetCompletedTasksAsync(Guid projectId);

    /// <summary>
    /// Gets incomplete tasks for a specific project
    /// </summary>
    /// <param name="projectId">The project's ID</param>
    /// <returns>A collection of incomplete tasks</returns>
    Task<IEnumerable<TaskItem>> GetIncompleteTasksAsync(Guid projectId);

    /// <summary>
    /// Adds a new task to the repository
    /// </summary>
    /// <param name="task">The task to add</param>
    Task AddAsync(TaskItem task);

    /// <summary>
    /// Updates an existing task in the repository
    /// </summary>
    /// <param name="task">The task to update</param>
    Task UpdateAsync(TaskItem task);

    /// <summary>
    /// Deletes a task from the repository
    /// </summary>
    /// <param name="id">The ID of the task to delete</param>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if a task belongs to a project owned by a specific user
    /// </summary>
    /// <param name="taskId">The task's ID</param>
    /// <param name="userId">The user's ID</param>
    /// <returns>True if the task belongs to the user, false otherwise</returns>
    Task<bool> BelongsToUserAsync(Guid taskId, Guid userId);

    /// <summary>
    /// Gets the count of tasks in a specific project
    /// </summary>
    /// <param name="projectId">The project's ID</param>
    /// <returns>The number of tasks in the project</returns>
    Task<int> GetTaskCountAsync(Guid projectId);
}
