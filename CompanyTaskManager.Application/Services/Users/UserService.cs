


using AutoMapper;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.Users;

public class UserService(UserManager<ApplicationUser> _userManager,
    IMapper _mapper) : IUserService
{


    public async Task<List<UserViewModel>> GetAllUsersAsync(string searchString = null)
    {
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

        return userViewModels;
    }

    public async Task<UserViewModel> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
            IsLockedOut = await _userManager.IsLockedOutAsync(user)
        };

        return userViewModel;
    }

    public async Task BlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains(Roles.Administrator))
            throw new Exception("Cannot block administrator");

        user.LockoutEnd = DateTimeOffset.MaxValue;
        await _userManager.UpdateAsync(user);
    }
    public async Task UnblockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new Exception("User not found");

        user.LockoutEnd = null;
        await _userManager.UpdateAsync(user);
    }
}
