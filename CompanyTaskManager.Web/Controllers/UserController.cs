using CompanyTaskManager.Application.Services.Users;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize(Roles = Roles.Administrator)]
public class UserController(IUserService _userService,
    RoleManager<IdentityRole> _roleManager,
    ILogger<UserController> _logger) : Controller
{
    public async Task<IActionResult> Index(string searchString)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        try
        {
            _logger.LogInformation("Administrator {UserName} is accessing users index with search string: {SearchString}", 
                userName, searchString ?? "All");
                
            var users = await _userService.GetAllUsersAsync(searchString);
            
            _logger.LogInformation("Successfully retrieved {UserCount} users for administrator {UserName}", 
                users.Count(), userName);
                
            return View(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users for administrator {UserName} with search string: {SearchString}", 
                userName, searchString ?? "All");
            throw;
        }
    }


    [HttpPost]
    public async Task<IActionResult> BlockUser(string id)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        try
        {
            _logger.LogInformation("Administrator {UserName} is blocking user {UserId}", 
                userName, id);
                
            await _userService.BlockUserAsync(id);
            
            _logger.LogInformation("User {UserId} successfully blocked by administrator {UserName}", 
                id, userName);
                
            return RedirectToAction("Index");
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking user {UserId} by administrator {UserName}", 
                id, userName);
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UnblockUser(string id)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        try
        {
            _logger.LogInformation("Administrator {UserName} is unblocking user {UserId}", 
                userName, id);
                
            await _userService.UnblockUserAsync(id);
            
            _logger.LogInformation("User {UserId} successfully unblocked by administrator {UserName}", 
                id, userName);
                
            return RedirectToAction("Index");
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unblocking user {UserId} by administrator {UserName}", 
                id, userName);
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Error");
        }
    }

}


