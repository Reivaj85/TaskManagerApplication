using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Authentication;
using TaskManager.Application.UseCases.Authentication;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authenticationService.RegisterAsync(request);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return BadRequest(new { Error = result.Error });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authenticationService.LoginAsync(request);
        
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return Unauthorized(new { Error = result.Error });
    }
}
