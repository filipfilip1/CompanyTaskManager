﻿
namespace CompanyTaskManager.Application.ViewModels.Notification;

public class NotificationViewModel
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string NotificationTypeName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
