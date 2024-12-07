

using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Data.Models;

namespace CompanyTaskManager.Application.Services.RoleRequests;

public interface IRoleRequestsService
{
    Task CreateRoleRequestAsync(string userId, string requestedRole);
    Task<List<RoleRequestViewModel>> GetAllRoleRequestsAsync();
    Task ApproveRoleRequestAsync(int roleRequestId);
    Task RejectRoleRequestAsync(int roleRequestId);
    Task<RoleRequestSummaryViewModel> GetRoleRequestSummaryAsync(int? statusId = null);
}
