using TaskManager.Application.DTOs.Authentication;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.UseCases.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> GetUserByIdAsync(Guid userId);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthenticationService(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<Result<LoginResponse>> GetUserByIdAsync(Guid userId) {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return "User not found.";
        
        var token = _tokenService.GenerateToken(user.Id, user.UserName.Value);
        var expiresAt = _tokenService.GetTokenExpiration();

        return new LoginResponse
        {
            Token = token,
            User = user.ToDto(),
            ExpiresAt = expiresAt
        };
    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (user == null)
            return "Invalid username or password.";
        
        if (!user.ValidatePassword(request.Password))
            return "Invalid username or password.";
        
        var token = _tokenService.GenerateToken(user.Id, user.UserName.Value);
        var expiresAt = _tokenService.GetTokenExpiration();

        return new LoginResponse
        {
            Token = token,
            User = user.ToDto(),
            ExpiresAt = expiresAt
        };
    }

    public async Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
        if (existingUser != null)
            return "Username already exists.";
        
        var userResult = User.Register(request.Username, request.Password);
        if (userResult.IsFailure)
            return userResult.Error;

        var user = userResult.Value;
        
        await _unitOfWork.Users.AddAsync(user);
        
        var defaultProjectResult = Project.New(user.Id, "Personal", isDefault: true);
        if (defaultProjectResult.IsFailure)
        {
            return defaultProjectResult.Error;
        }

        await _unitOfWork.Projects.AddAsync(defaultProjectResult.Value);
        
        await _unitOfWork.SaveChangesAsync();
        
        var token = _tokenService.GenerateToken(user.Id, user.UserName.Value);
        var expiresAt = _tokenService.GetTokenExpiration();
        
        return new RegisterResponse
        {
            Token = token,
            User = user.ToDto(),
            ExpiresAt = expiresAt
        };
    }
}