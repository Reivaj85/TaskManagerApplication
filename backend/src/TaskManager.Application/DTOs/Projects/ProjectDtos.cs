namespace TaskManager.Application.DTOs.Projects;

/// <summary>
/// Project data transfer object
/// </summary>
public record ProjectDto
{
    /// <summary>
    /// The project's unique identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The project name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Whether this is the user's default project
    /// </summary>
    public bool IsDefault { get; init; }

    /// <summary>
    /// When the project was created
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The number of tasks in this project
    /// </summary>
    public int TaskCount { get; init; }
}

/// <summary>
/// Request DTO for creating a new project
/// </summary>
public record CreateProjectRequest
{
    /// <summary>
    /// The project name
    /// </summary>
    public string Name { get; init; } = string.Empty;
}

/// <summary>
/// Request DTO for updating a project
/// </summary>
public record UpdateProjectRequest
{
    /// <summary>
    /// The new project name
    /// </summary>
    public string Name { get; init; } = string.Empty;
}

/// <summary>
/// Summary DTO for projects (lightweight version of ProjectDto)
/// </summary>
public record ProjectSummaryDto
{
    /// <summary>
    /// The project's unique identifier
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The project name
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Whether this is the user's default project
    /// </summary>
    public bool IsDefault { get; init; }

    /// <summary>
    /// The number of tasks in this project
    /// </summary>
    public int TaskCount { get; init; }
}
