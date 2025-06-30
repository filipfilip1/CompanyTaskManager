

using AutoMapper;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.RoleRequests;

public class RoleRequestService(ApplicationDbContext _context,
    UserManager<ApplicationUser> _userManager,
    IMapper _mapper,
    INotificationService _notificationService,
    ITeamService _teamService,
    ILogger<RoleRequestService> _logger) : IRoleRequestsService
{


    public Task CreateRoleRequestAsync(string userId, string requestedRole)
    {
        _logger.LogInformation("Creating role request for user {UserId} to role {RequestedRole}", userId, requestedRole);
        var roleRequest = new RoleRequest
        {
            UserId = userId,
            RequestedRole = requestedRole,
            RequestDate = DateTime.Now,
            IsApproved = false,
            RequestStatusId = 1 // Pending
        };

        _context.RoleRequests.Add(roleRequest);
        _logger.LogDebug("Role request created for user {UserId} to role {RequestedRole}", userId, requestedRole);
        return _context.SaveChangesAsync();
    }
    public async Task ApproveRoleRequestAsync(int roleRequestId)
    {
        _logger.LogInformation("Approving role request {RoleRequestId}", roleRequestId);
        
        var roleRequest = await _context.RoleRequests.FindAsync(roleRequestId);
        if (roleRequest == null)
        {
            _logger.LogWarning("Role request {RoleRequestId} not found", roleRequestId);
            throw new Exception("Role request not found");
        }
        

        var user = await _userManager.FindByIdAsync(roleRequest.UserId);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found for role request {RoleRequestId}", roleRequest.UserId, roleRequestId);
            throw new Exception("User not found");
        }

        try
        {
            var result = await _userManager.AddToRoleAsync(user, roleRequest.RequestedRole);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to add role {Role} to user {UserId}: {Errors}", 
                    roleRequest.RequestedRole, roleRequest.UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Failed to add role to user");
            }

            if (roleRequest.RequestedRole == "Manager")
            {
                var teamName = $"{user.FirstName}'s Team";
                await _teamService.CreateTeamAsync(user.Id, teamName);
            }
        } catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to add manager role to user {UserId} or create a team", roleRequest.UserId);
            throw new Exception("Failed to add manager role to user or create a team");
        }



        roleRequest.IsApproved = true;
        roleRequest.ApprovalDate = DateTime.Now;
        roleRequest.RequestStatusId = 2; // Approved

        _context.Update(roleRequest);

        await _notificationService.CreateNotificationAsync(roleRequest.UserId, "Your role request has been approved", 1);

        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Role request {RoleRequestId} approved successfully for user {UserId} to role {Role}", 
            roleRequestId, roleRequest.UserId, roleRequest.RequestedRole);
        
    }


    public async Task<List<RoleRequestViewModel>> GetAllRoleRequestsAsync()
    {
        _logger.LogInformation("Fetching all role requests");
        
        var roleRequests = await _context.RoleRequests
            .Include(q => q.User)
            .Include(q => q.RequestStatus)
            .ToListAsync();

        var result = _mapper.Map<List<RoleRequestViewModel>>(roleRequests);
        _logger.LogDebug("Found {RequestCount} role requests", result.Count);
        return result;
    }

    public async Task RejectRoleRequestAsync(int roleRequestId)
    {
        _logger.LogInformation("Rejecting role request {RoleRequestId}", roleRequestId);
        
        var roleRequest = await _context.RoleRequests.FindAsync(roleRequestId);
        if (roleRequest == null)
        {
            _logger.LogWarning("Role request {RoleRequestId} not found", roleRequestId);
            throw new Exception("Role request not found");
        }

        roleRequest.IsApproved = false;
        roleRequest.ApprovalDate = DateTime.Now;
        roleRequest.RequestStatusId = 3; // Rejected

        _context.Update(roleRequest);

        await _notificationService.CreateNotificationAsync(roleRequest.UserId, "Your role request has been rejected", 2);

        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Role request {RoleRequestId} rejected successfully for user {UserId}", roleRequestId, roleRequest.UserId);
    }

    public async Task<RoleRequestSummaryViewModel> GetRoleRequestSummaryAsync(int? statusId = null)
    {
        _logger.LogInformation("Fetching role request summary with status filter {StatusId}", statusId);
        var roleRequestsQuery = _context.RoleRequests
            .Include(q => q.User)
            .Include(q => q.RequestStatus)
            .AsQueryable();

        if (statusId.HasValue)
        {
            roleRequestsQuery = roleRequestsQuery.Where(q => q.RequestStatusId == statusId);
        }

        var roleRequests = await roleRequestsQuery.ToListAsync();

        var roleRequestViewModels = _mapper.Map<List<RoleRequestViewModel>>(roleRequests);

        var totalRequests = await _context.RoleRequests.CountAsync();
        var pendingRequests = await _context.RoleRequests.CountAsync(q => q.RequestStatusId == 1);
        var approvedRequests = await _context.RoleRequests.CountAsync(q => q.RequestStatusId == 2);
        var rejectedRequests = await _context.RoleRequests.CountAsync(q => q.RequestStatusId == 3);

        var summary = new RoleRequestSummaryViewModel
        {
            TotalRequests = totalRequests,
            PendingRequests = pendingRequests,
            ApprovedRequests = approvedRequests,
            RejectedRequests = rejectedRequests,
            RoleRequests = roleRequestViewModels
        };

        _logger.LogDebug("Role request summary: {Total} total, {Pending} pending, {Approved} approved, {Rejected} rejected", 
            totalRequests, pendingRequests, approvedRequests, rejectedRequests);
            
        return summary;
    }
}
