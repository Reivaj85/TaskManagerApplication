using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public ProjectName Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsDefault { get; private set; }
    
    private Project() { }

    private Project(Guid id, Guid userId, ProjectName name, DateTime createdAt, bool isDefault)
    {
        Id = id;
        UserId = userId;
        Name = name;
        CreatedAt = createdAt;
        IsDefault = isDefault;
    }

    /// <summary>
    /// Factory method to create a new project
    /// </summary>
    /// <param name="userId">The ID of the user who owns this project</param>
    /// <param name="name">The name of the project</param>
    /// <param name="isDefault">Whether this is the user's default project</param>
    /// <returns>A Result containing the new Project instance or error message</returns>
    public static Result<Project> New(Guid userId, string name, bool isDefault = false)
    {
        if (userId == Guid.Empty)
            return "User ID cannot be empty.";

        var nameResult = ProjectName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        return new Project(
            id: Guid.NewGuid(),
            userId: userId,
            name: nameResult.Value,
            createdAt: DateTime.UtcNow,
            isDefault: isDefault
        );
    }
    
    public static Result<Project> Create(Guid id, Guid userId, string name, DateTime createdAt, bool isDefault)
    {
        if (id == Guid.Empty)
            return "Project ID cannot be empty.";
        if (userId == Guid.Empty)
            return "User ID cannot be empty.";

        var nameResult = ProjectName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        return new Project(
            id: id,
            userId: userId,
            name: nameResult.Value,
            createdAt: createdAt,
            isDefault: isDefault
        );
    }

    /// <summary>
    /// Factory method to create a default personal project for a new user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>A Result containing the new default Project instance or error message</returns>
    public static Result<Project> CreateDefault(Guid userId)
    {
        return New(userId, "Personal", isDefault: true);
    }

    /// <summary>
    /// Renames the project
    /// </summary>
    /// <param name="newName">The new name for the project</param>
    /// <returns>A Result indicating success or failure</returns>
    public Result Rename(string newName)
    {
        var nameResult = ProjectName.Create(newName);
        if (nameResult.IsFailure)
            return nameResult.Error;

        Name = nameResult.Value;
        return Result.Success();
    }

    /// <summary>
    /// Marks this project as the default project for the user
    /// </summary>
    public void MarkAsDefault()
    {
        IsDefault = true;
    }

    /// <summary>
    /// Unmarks this project as the default project
    /// </summary>
    public void UnmarkAsDefault()
    {
        if (IsDefault)
        {
            IsDefault = false;
        }
    }
}
