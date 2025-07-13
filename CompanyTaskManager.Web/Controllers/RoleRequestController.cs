using CompanyTaskManager.Application.Services.RoleRequests;
using CompanyTaskManager.Common.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyTaskManager.Web.Controllers;

[Authorize]
public class RoleRequestController(IRoleRequestsService _roleRequestsService,
    ILogger<RoleRequestController> _logger) : Controller
{
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Index(int? statusId = null)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        _logger.LogInformation("Administrator {UserName} is accessing role requests with status filter: {StatusId}", 
            userName, statusId?.ToString() ?? "All");
            
        var summary = await _roleRequestsService.GetRoleRequestSummaryAsync(statusId);
        
        _logger.LogInformation("Successfully retrieved role request summary for administrator {UserName}", 
            userName);
            
        return View(summary);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        try
        {
            _logger.LogInformation("Administrator {UserName} is approving role request {RequestId}", 
                userName, id);
                
            await _roleRequestsService.ApproveRoleRequestAsync(id);
            
            _logger.LogInformation("Role request {RequestId} successfully approved by administrator {UserName}", 
                id, userName);
                
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving role request {RequestId} by administrator {UserName}", 
                id, userName);
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var userName = User?.Identity?.Name ?? "Unknown";
        
        try
        {
            _logger.LogInformation("Administrator {UserName} is rejecting role request {RequestId}", 
                userName, id);
                
            await _roleRequestsService.RejectRoleRequestAsync(id);
            
            _logger.LogInformation("Role request {RequestId} successfully rejected by administrator {UserName}", 
                id, userName);
                
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting role request {RequestId} by administrator {UserName}", 
                id, userName);
            ModelState.AddModelError("", ex.Message);
            return RedirectToAction("Error");
        }
    }
}
