namespace CompanyTaskManager.Data.Models;

public class RoleRequest : BaseEntity
{
    public ApplicationUser? User { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    public string? RequestedRole { get; set; }

    public bool IsApproved { get; set; } = false;

    public DateTime ApprovalDate { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;

    public RequestStatus? RequestStatus { get; set; }
    public int RequestStatusId { get; set; }

}
