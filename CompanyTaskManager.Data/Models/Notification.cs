

namespace CompanyTaskManager.Data.Models;

public class Notification : BaseEntity
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int NotificationTypeId { get; set; }
    public NotificationType NotificationType { get; set; }

    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
}
