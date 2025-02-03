using CompanyTaskManager.Application.Services.Users;
using CompanyTaskManager.Common.Static;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize(Roles = Roles.Administrator)]
public class UserController(IUserService _userService,
    RoleManager<IdentityRole> _roleManager) : Controller
{
    public IActionResult Index(string searchString)
    {
        var users = _userService.GetAllUsersAsync(searchString).Result;
        return View(users);
    }


    [HttpPost]
    public async Task<IActionResult> BlockUser(string id)
    {
        try
        {
            await _userService.BlockUserAsync(id);
            return RedirectToAction("Index");
        } catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UnblockUser(string id)
    {
        try
        {
            await _userService.UnblockUserAsync(id);
            return RedirectToAction("Index");
        } 
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return RedirectToAction("Error");
        }
    }

}


