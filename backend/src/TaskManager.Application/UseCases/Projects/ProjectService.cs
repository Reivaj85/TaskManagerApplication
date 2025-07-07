using TaskManager.Application.DTOs.Projects;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.UseCases.Projects;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectSummaryDto>>> GetUserProjectsAsync(Guid userId);
    Task<Result<ProjectDto>> GetProjectByIdAsync(Guid projectId, Guid userId);
    Task<Result<ProjectDto>> CreateProjectAsync(Guid userId, CreateProjectRequest request);
    Task<Result<ProjectDto>> UpdateProjectAsync(Guid projectId, Guid userId, UpdateProjectRequest request);
    Task<Result> DeleteProjectAsync(Guid projectId, Guid userId);
}

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<ProjectSummaryDto>>> GetUserProjectsAsync(Guid userId)
    {
        var projectDtos = new List<ProjectSummaryDto>();
        
        var projects = await _unitOfWork.Projects.ListAsync(userId);
        foreach (var project in projects)
        {
            var tasks = await _unitOfWork.Tasks.ListAsync(project.Id);
            var taskCount = tasks.Count();
            projectDtos.Add(project.ToSummaryDto(taskCount));
        }

        return projectDtos;
    }

    public async Task<Result<ProjectDto>> GetProjectByIdAsync(Guid projectId, Guid userId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            return "Project not found.";

        if (project.UserId != userId)
            return "Access denied.";

        var tasks = await _unitOfWork.Tasks.ListAsync(projectId);
        return project.ToDto(tasks.Count());
    }

    public async Task<Result<ProjectDto>> CreateProjectAsync(Guid userId, CreateProjectRequest request)
    {
        var projectResult = Project.New(userId, request.Name);
        if (projectResult.IsFailure)
            return projectResult.Error;

        var project = projectResult.Value;
        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        return project.ToDto(0);
    }

    public async Task<Result<ProjectDto>> UpdateProjectAsync(Guid projectId, Guid userId, UpdateProjectRequest request)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            return "Project not found.";

        if (project.UserId != userId)
            return "Access denied.";

        var updateResult = project.Rename(request.Name);
        if (updateResult.IsFailure)
            return updateResult.Error;

        await _unitOfWork.Projects.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();

        var tasks = await _unitOfWork.Tasks.ListAsync(projectId);
        return project.ToDto(tasks.Count());
    }

    public async Task<Result> DeleteProjectAsync(Guid projectId, Guid userId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(projectId);
        if (project == null)
            return "Project not found.";

        if (project.UserId != userId)
            return "Access denied.";

        if (project.IsDefault)
            return "Cannot delete default project.";

        // Delete all tasks in this project first
        var tasks = await _unitOfWork.Tasks.ListAsync(projectId);
        foreach (var task in tasks)
        {
            await _unitOfWork.Tasks.DeleteAsync(task.Id);
        }

        await _unitOfWork.Projects.DeleteAsync(projectId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}