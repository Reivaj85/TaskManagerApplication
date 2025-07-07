using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Application.DTOs.Projects;
using TaskManager.Application.UseCases.Projects;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
    }

    [HttpGet]
    public async Task<IActionResult> GetUserProjects()
    {
        var userId = GetUserId();
        var result = await _projectService.GetUserProjectsAsync(userId);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpGet("{projectId:guid}")]
    public async Task<IActionResult> GetProject(Guid projectId)
    {
        var userId = GetUserId();
        var result = await _projectService.GetProjectByIdAsync(projectId, userId);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return NotFound(new { Error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var result = await _projectService.CreateProjectAsync(userId, request);
        
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetProject), new { projectId = result.Value.Id }, result.Value);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpPut("{projectId:guid}")]
    public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] UpdateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        var result = await _projectService.UpdateProjectAsync(projectId, userId, request);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return NotFound(new { Error = result.Error });
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var userId = GetUserId();
        var result = await _projectService.DeleteProjectAsync(projectId, userId);
        
        if (result.IsSuccess)
            return NoContent();
        
        return NotFound(new { Error = result.Error });
    }
}
