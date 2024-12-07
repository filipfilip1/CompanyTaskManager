
namespace CompanyTaskManager.Application.ViewModels.RoleRequest;

public class RoleRequestViewModel
{
    public int Id { get; set; }
    public string UserEmail { get; set; }
    public string RequestedRole { get; set; }
    public bool IsApproved { get; set; }
    public DateTime ApprovalDate { get; set; }
    public DateTime RequestDate { get; set; }
    public string RequestStatusName { get; set; }
    public int RequestStatusId { get; set; }
}

