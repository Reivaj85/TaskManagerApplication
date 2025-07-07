using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.UseCases.Tasks;

public interface ITaskService
{
    Task<Result<IEnumerable<TaskSummaryDto>>> GetProjectTasksAsync(Guid projectId, Guid userId);
    Task<Result<IEnumerable<TaskSummaryDto>>> GetUserTasksAsync(Guid userId);
    Task<Result<TaskDto>> GetTaskByIdAsync(Guid taskId, Guid userId);
    Task<Result<TaskDto>> CreateTaskAsync(Guid projectId, Guid userId, CreateTaskRequest request);
    Task<Result<TaskDto>> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest request);
    Task<Result<TaskDto>> CompleteTaskAsync(Guid taskId, Guid userId);
    Task<Result> DeleteTaskAsync(Guid taskId, Guid userId);
}

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<TaskSummaryDto>>> GetProjectTasksAsync(Guid projectId, Guid userId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            return "Project not found.";

        if (project.UserId != userId)
            return "Access denied.";

        var tasks = await _unitOfWork.Tasks.ListAsync(projectId);
        return tasks.Select(t => t.ToSummaryDto()).ToList();
    }

    public async Task<Result<IEnumerable<TaskSummaryDto>>> GetUserTasksAsync(Guid userId)
    {
        var tasks = await _unitOfWork.Tasks.ListByUserAsync(userId);
        return tasks.Select(t => t.ToSummaryDto()).ToList();
    }

    public async Task<Result<TaskDto>> GetTaskByIdAsync(Guid taskId, Guid userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (task == null)
            return "Task not found.";

        // Verify the task belongs to a project owned by the user
        var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return "Access denied.";

        return task.ToDto();
    }

    public async Task<Result<TaskDto>> CreateTaskAsync(Guid projectId, Guid userId, CreateTaskRequest request)
    {
        // Verify the project belongs to the user
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            return "Project not found.";

        if (project.UserId != userId)
            return "Access denied.";

        var taskResult = TaskItem.New(projectId, request.Title, request.Description);
        if (taskResult.IsFailure)
            return taskResult.Error;

        var task = taskResult.Value;
        await _unitOfWork.Tasks.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return task.ToDto();
    }

    public async Task<Result<TaskDto>> UpdateTaskAsync(Guid taskId, Guid userId, UpdateTaskRequest request)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (task == null)
            return "Task not found.";

        // Verify the task belongs to a project owned by the user
        var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return "Access denied.";

        var updateResult = task.Update(request.Title, request.Description);
        if (updateResult.IsFailure)
            return updateResult.Error;

        await _unitOfWork.Tasks.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return task.ToDto();
    }

    public async Task<Result<TaskDto>> CompleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (task == null)
            return "Task not found.";

        // Verify the task belongs to a project owned by the user
        var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return "Access denied.";

        task.MarkAsCompleted();
        await _unitOfWork.Tasks.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return task.ToDto();
    }

    public async Task<Result> DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _unitOfWork.Tasks.GetByIdAsync(taskId);
        if (task == null)
            return "Task not found.";

        // Verify the task belongs to a project owned by the user
        var project = await _unitOfWork.Projects.GetByIdAsync(task.ProjectId);
        if (project == null || project.UserId != userId)
            return "Access denied.";

        await _unitOfWork.Tasks.DeleteAsync(taskId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
