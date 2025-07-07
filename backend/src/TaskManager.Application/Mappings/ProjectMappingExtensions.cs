using TaskManager.Application.DTOs.Projects;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappings;

public static class ProjectMappingExtensions
{
    public static ProjectDto ToDto(this Project project, int taskCount = 0)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name.Value,
            IsDefault = project.IsDefault,
            CreatedAt = project.CreatedAt,
            TaskCount = taskCount
        };
    }

    public static ProjectSummaryDto ToSummaryDto(this Project project, int taskCount = 0)
    {
        return new ProjectSummaryDto
        {
            Id = project.Id,
            Name = project.Name.Value,
            IsDefault = project.IsDefault,
            TaskCount = taskCount
        };
    }
}