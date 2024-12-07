

using AutoMapper;
using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.RoleRequests;

public class RoleRequestService(ApplicationDbContext _context,
    UserManager<ApplicationUser> _userManager,
    IMapper _mapper) : IRoleRequestsService
{


    public Task CreateRoleRequestAsync(string userId, string requestedRole)
    {
        var roleRequest = new RoleRequest
        {
            UserId = userId,
            RequestedRole = requestedRole,
            RequestDate = DateTime.Now,
            IsApproved = false,
            RequestStatusId = 1 // Pending
        };

        _context.RoleRequests.Add(roleRequest);
        return _context.SaveChangesAsync();
    }
    public async Task ApproveRoleRequestAsync(int roleRequestId)
    {
        var roleRequest = await _context.RoleRequests.FindAsync(roleRequestId);
        if (roleRequest == null)
            throw new Exception("Role request not found");
        

        var user = await _userManager.FindByIdAsync(roleRequest.UserId);
        if (user == null)
            throw new Exception("User not found");

        var result = await _userManager.AddToRoleAsync(user, roleRequest.RequestedRole);
        if (!result.Succeeded)
            throw new Exception("Failed to add role to user");

        roleRequest.IsApproved = true;
        roleRequest.ApprovalDate = DateTime.Now;
        roleRequest.RequestStatusId = 2; // Approved

        _context.Update(roleRequest);
        await _context.SaveChangesAsync();
        
    }


    public async Task<List<RoleRequestViewModel>> GetAllRoleRequestsAsync()
    {
        var roleRequests = await _context.RoleRequests
            .Include(q => q.User)
            .Include(q => q.RequestStatus)
            .ToListAsync();

        return _mapper.Map<List<RoleRequestViewModel>>(roleRequests);
    }

    public async Task RejectRoleRequestAsync(int roleRequestId)
    {
        var roleRequest = await _context.RoleRequests.FindAsync(roleRequestId);
        if (roleRequest == null)
            throw new Exception("Role request not found");

        roleRequest.IsApproved = false;
        roleRequest.ApprovalDate = DateTime.Now;
        roleRequest.RequestStatusId = 3; // Rejected

        _context.Update(roleRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<RoleRequestSummaryViewModel> GetRoleRequestSummaryAsync(int? statusId = null)
    {
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

        return summary;
    }
}
