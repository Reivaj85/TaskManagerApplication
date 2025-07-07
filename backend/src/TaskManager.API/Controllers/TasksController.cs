using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.UseCases.Tasks;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserTasks()
    {
        var userId = GetUserId();
        var result = await _taskService.GetUserTasksAsync(userId);
        
        if (result.IsSuccess)
            return Ok(result);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetProjectTasks(Guid projectId)
    {
        var userId = GetUserId();
        var result = await _taskService.GetProjectTasksAsync(projectId, userId);
        
        if (result.IsSuccess)
            return Ok(result);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpGet("{taskId:guid}")]
    public async Task<IActionResult> GetTask(Guid taskId)
    {
        var userId = GetUserId();
        var result = await _taskService.GetTaskByIdAsync(taskId, userId);
        
        if (result.IsSuccess)
            return Ok(result);
        
        return NotFound(new { Error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var result = await _taskService.CreateTaskAsync(request.ProjectId, userId, request);
        
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetTask), new { taskId = result.Value.Id }, result.Value);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpPut("{taskId:guid}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var result = await _taskService.UpdateTaskAsync(taskId, userId, request);
        
        if (result.IsSuccess)
            return Ok(result);
        
        return NotFound(new { Error = result.Error });
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        var userId = GetUserId();
        var result = await _taskService.DeleteTaskAsync(taskId, userId);
        
        if (result.IsSuccess)
            return NoContent();
        
        return NotFound(new { Error = result.Error });
    }
}
