namespace TaskManager.Application.DTOs.Tasks;

/// <summary>
/// Task data transfer object
/// </summary>
public record TaskDto
{
    /// <summary>
    /// The task's unique identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The project ID this task belongs to
    /// </summary>
    public Guid ProjectId { get; init; }

    /// <summary>
    /// The task title
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// The task description
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Whether the task is completed
    /// </summary>
    public bool IsCompleted { get; init; }

    /// <summary>
    /// When the task was created
    /// </summary>
    public DateTime CreatedAt { get; init; }
}

/// <summary>
/// Request DTO for creating a new task
/// </summary>
public record CreateTaskRequest
{
    /// <summary>
    /// The project ID this task belongs to
    /// </summary>
    public Guid ProjectId { get; init; }

    /// <summary>
    /// The task title
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// The task description
    /// </summary>
    public string Description { get; init; } = string.Empty;
}

/// <summary>
/// Request DTO for updating a task
/// </summary>
public record UpdateTaskRequest
{
    /// <summary>
    /// The new task title
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// The new task description
    /// </summary>
    public string Description { get; init; } = string.Empty;
}

/// <summary>
/// Summary DTO for tasks (lightweight version of TaskDto)
/// </summary>
public record TaskSummaryDto
{
    /// <summary>
    /// The task's unique identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The task title
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// Whether the task is completed
    /// </summary>
    public bool IsCompleted { get; init; }

    /// <summary>
    /// When the task was created
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
