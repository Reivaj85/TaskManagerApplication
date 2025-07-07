using TaskManager.Application.DTOs.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappings;

public static class TaskMappingExtensions
{
    public static TaskDto ToDto(this TaskItem task)
    {
        return new TaskDto
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Title = task.Title.Value,
            Description = task.Description.Value,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt
        };
    }

    public static TaskSummaryDto ToSummaryDto(this TaskItem task)
    {
        return new TaskSummaryDto
        {
            Id = task.Id,
            Title = task.Title.Value,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt
        };
    }
}