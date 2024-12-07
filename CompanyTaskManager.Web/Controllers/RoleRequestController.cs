using CompanyTaskManager.Application.Services.RoleRequests;
using CompanyTaskManager.Common.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class RoleRequestController(IRoleRequestsService _roleRequestsService) : Controller
{
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Index(int? statusId = null)
    {
        var summary = await _roleRequestsService.GetRoleRequestSummaryAsync(statusId);
        return View(summary);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        try
        {
            await _roleRequestsService.ApproveRoleRequestAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        try
        {
            await _roleRequestsService.RejectRoleRequestAsync(id);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction("Error");
        }
    }
}
