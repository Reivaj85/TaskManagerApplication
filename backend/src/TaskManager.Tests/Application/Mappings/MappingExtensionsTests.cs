using TaskManager.Application.Mappings;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.Tests.Application.Mappings;

public class UserMappingExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapUserToUserDto()
    {
        // Arrange
        var userResult = User.Register("testuser", "Password123!");
        Assert.True(userResult.IsSuccess);
        var user = userResult.Value;

        // Act
        var dto = user.ToDto();

        // Assert
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.UserName.Value, dto.Username);
        Assert.Equal(user.CreatedAt, dto.CreatedAt);
    }
}

public class ProjectMappingExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapProjectToProjectDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectResult = Project.New(userId, "Test Project");
        Assert.True(projectResult.IsSuccess);
        var project = projectResult.Value;

        // Act
        var dto = project.ToDto(5);

        // Assert
        Assert.Equal(project.Id, dto.Id);
        Assert.Equal(project.Name.Value, dto.Name);
        Assert.Equal(project.IsDefault, dto.IsDefault);
        Assert.Equal(project.CreatedAt, dto.CreatedAt);
        Assert.Equal(5, dto.TaskCount);
    }

    [Fact]
    public void ToSummaryDto_ShouldMapProjectToProjectSummaryDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectResult = Project.New(userId, "Test Project");
        Assert.True(projectResult.IsSuccess);
        var project = projectResult.Value;

        // Act
        var dto = project.ToSummaryDto(3);

        // Assert
        Assert.Equal(project.Id, dto.Id);
        Assert.Equal(project.Name.Value, dto.Name);
        Assert.Equal(project.IsDefault, dto.IsDefault);
        Assert.Equal(3, dto.TaskCount);
    }
}

public class TaskMappingExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapTaskToTaskDto()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskResult = TaskItem.New(projectId, "Test Task", "Test Description");
        Assert.True(taskResult.IsSuccess);
        var task = taskResult.Value;

        // Act
        var dto = task.ToDto();

        // Assert
        Assert.Equal(task.Id, dto.Id);
        Assert.Equal(task.ProjectId, dto.ProjectId);
        Assert.Equal(task.Title.Value, dto.Title);
        Assert.Equal(task.Description.Value, dto.Description);
        Assert.Equal(task.IsCompleted, dto.IsCompleted);
        Assert.Equal(task.CreatedAt, dto.CreatedAt);
    }

    [Fact]
    public void ToSummaryDto_ShouldMapTaskToTaskSummaryDto()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var taskResult = TaskItem.New(projectId, "Test Task", "Test Description");
        Assert.True(taskResult.IsSuccess);
        var task = taskResult.Value;

        // Act
        var dto = task.ToSummaryDto();

        // Assert
        Assert.Equal(task.Id, dto.Id);
        Assert.Equal(task.Title.Value, dto.Title);
        Assert.Equal(task.IsCompleted, dto.IsCompleted);
        Assert.Equal(task.CreatedAt, dto.CreatedAt);
    }
}
