using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    public Guid ProjectId { get; private set; }
    public TaskTitle Title { get; private set; }
    public TaskDescription Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsCompleted { get; private set; }
    
    private TaskItem() { }

    private TaskItem(Guid id, Guid projectId, TaskTitle title, TaskDescription description, DateTime createdAt, bool isCompleted)
    {
        Id = id;
        ProjectId = projectId;
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        IsCompleted = isCompleted;
    }

    /// <summary>
    /// Factory method to create a new task
    /// </summary>
    /// <param name="projectId">The ID of the project this task belongs to</param>
    /// <param name="title">The title of the task</param>
    /// <param name="description">The description of the task</param>
    /// <returns>A Result containing the new TaskItem instance or error message</returns>
    public static Result<TaskItem> New(Guid projectId, string title, string description = "")
    {
        if (projectId == Guid.Empty)
            return Result<TaskItem>.Failure("Project ID cannot be empty.");

        var titleResult = TaskTitle.Create(title);
        if (titleResult.IsFailure)
            return Result<TaskItem>.Failure(titleResult.Error);

        var descriptionResult = TaskDescription.Create(description);
        if (descriptionResult.IsFailure)
            return Result<TaskItem>.Failure(descriptionResult.Error);

        return Result<TaskItem>.Success( new TaskItem(
            id: Guid.NewGuid(),
            projectId: projectId,
            title: titleResult.Value,
            description: descriptionResult.Value,
            createdAt: DateTime.UtcNow,
            isCompleted: false
        ));
    }
    
    public static Result<TaskItem> Create(
        Guid id
      , Guid projectId
      , TaskTitle title
      , TaskDescription description
      , DateTime createdAt
      , bool isCompleted)
    {
        if (id == Guid.Empty)
            return Result<TaskItem>.Failure("Task ID cannot be empty.");
    
        if (projectId == Guid.Empty)
            return Result<TaskItem>.Failure("Project ID cannot be empty.");
    
        if (string.IsNullOrEmpty(title))
            return Result<TaskItem>.Failure("Title cannot be null or empty.");
        
        if(createdAt == DateTime.MinValue)
            return Result<TaskItem>.Failure("CreatedAt cannot be the default value.");

        return Result<TaskItem>.Success(new TaskItem(id: id, projectId: projectId, title: title
                                                   , description: description, createdAt: createdAt
                                                   , isCompleted: isCompleted));
    }

    /// <summary>
    /// Updates the task's title and description
    /// </summary>
    /// <param name="title">The new title</param>
    /// <param name="description">The new description</param>
    /// <returns>A Result indicating success or failure</returns>
    public Result Update(string title, string description = "")
    {
        var titleResult = TaskTitle.Create(title);
        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error);

        var descriptionResult = TaskDescription.Create(description);
        if (descriptionResult.IsFailure)
            return Result.Failure(descriptionResult.Error);

        Title = titleResult.Value;
        Description = descriptionResult.Value;
        return Result.Success();
    }

    /// <summary>
    /// Marks the task as completed
    /// </summary>
    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    /// <summary>
    /// Marks the task as incomplete
    /// </summary>
    public void MarkAsIncomplete()
    {
        IsCompleted = false;
    }

    /// <summary>
    /// Toggles the completion status of the task
    /// </summary>
    public void ToggleCompletion()
    {
        IsCompleted = !IsCompleted;
    }

    /// <summary>
    /// Moves the task to a different project
    /// </summary>
    /// <param name="newProjectId">The ID of the new project</param>
    /// <returns>A Result indicating success or failure</returns>
    public Result MoveToProject(Guid newProjectId)
    {
        if (newProjectId == Guid.Empty)
            return Result.Failure("New project ID cannot be empty.");

        ProjectId = newProjectId;
        return Result.Success();
    }
}