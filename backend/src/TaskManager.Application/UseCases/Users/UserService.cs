using TaskManager.Application.DTOs.Users;
using TaskManager.Application.Mappings;
using TaskManager.Domain.Common;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.UseCases.Users;

public interface IUserService
{
    Task<Result<UserDto>> GetUserByIdAsync(Guid userId);
    Task<Result<UserDto>> UpdateUserAsync(Guid userId, UpdateUserRequest request);
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return "User not found.";

        return user.ToDto();
    }

    public async Task<Result<UserDto>> UpdateUserAsync(Guid userId, UpdateUserRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return "User not found.";

        // For now, just handle password changes since username changes are complex
        if ( string.IsNullOrEmpty(request.NewPassword) ) {
            return user.ToDto();
        }

        if (string.IsNullOrEmpty(request.CurrentPassword))
            return "Current password is required to change password.";

        if (!user.ValidatePassword(request.CurrentPassword))
            return "Current password is incorrect.";

        var changeResult = user.ChangePassword(request.NewPassword);
        if (changeResult.IsFailure)
            return changeResult.Error;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user.ToDto();
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return "User not found.";

        if (!user.ValidatePassword(request.CurrentPassword))
            return "Current password is incorrect.";

        var changeResult = user.ChangePassword(request.NewPassword);
        if (changeResult.IsFailure)
            return changeResult.Error;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public Task<Result> DeleteUserAsync(Guid userId)
    {
        // For now, user deletion is not supported since the repository doesn't have DeleteAsync
        // This would need to be implemented in the repository layer first
        return Task.FromResult<Result>("User deletion is not currently supported.");
    }
}