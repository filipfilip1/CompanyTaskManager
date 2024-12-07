
namespace CompanyTaskManager.Application.ViewModels.RoleRequest;

public class RoleRequestSummaryViewModel
{
    public int TotalRequests { get; set; }
    public int PendingRequests { get; set; }
    public int ApprovedRequests { get; set; }
    public int RejectedRequests { get; set; }

    public List<RoleRequestViewModel> RoleRequests { get; set; } = new List<RoleRequestViewModel>();
}
