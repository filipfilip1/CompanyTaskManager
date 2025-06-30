


using AutoMapper;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.Users;

public class UserService(UserManager<ApplicationUser> _userManager,
    IMapper _mapper,
    ILogger<UserService> _logger) : IUserService
{


    public async Task<List<UserViewModel>> GetAllUsersAsync(string searchString = null)
    {
        _logger.LogInformation("Fetching all users with search string: {SearchString}", searchString ?? "(none)");
        var usersQuery = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            usersQuery = usersQuery.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString));
        }

        var users = await usersQuery.ToListAsync();

        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles.ToList(),
                IsLockedOut = user.LockoutEnd != null && user.LockoutEnd > DateTime.Now
            });
        }

        _logger.LogDebug("Found {UserCount} users", userViewModels.Count);
        return userViewModels;
    }

    public async Task<UserViewModel> GetUserByIdAsync(string userId)
    {
        _logger.LogInformation("Fetching user details for user {UserId}", userId);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", userId);
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            IsLockedOut = await _userManager.IsLockedOutAsync(user)
        };

        _logger.LogDebug("User {UserId} details retrieved successfully", userId);
        return userViewModel;
    }

    public async Task BlockUserAsync(string userId)
    {
        _logger.LogInformation("Blocking user {UserId}", userId);
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found when attempting to block", userId);
            throw new Exception("User not found");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains(Roles.Administrator))
        {
            _logger.LogWarning("Attempt to block administrator user {UserId} was denied", userId);
            throw new Exception("Cannot block administrator");
        }

        user.LockoutEnd = DateTimeOffset.MaxValue;
        await _userManager.UpdateAsync(user);
        
        _logger.LogInformation("User {UserId} blocked successfully", userId);
    }
    public async Task UnblockUserAsync(string userId)
    {
        _logger.LogInformation("Unblocking user {UserId}", userId);
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found when attempting to unblock", userId);
            throw new Exception("User not found");
        }

        user.LockoutEnd = null;
        await _userManager.UpdateAsync(user);
        
        _logger.LogInformation("User {UserId} unblocked successfully", userId);
    }
}
