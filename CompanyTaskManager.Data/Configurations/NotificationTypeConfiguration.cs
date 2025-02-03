

using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
{
    public void Configure(EntityTypeBuilder<NotificationType> builder)
    {
        builder.HasData(
            new NotificationType
            {
                Id = 1,
                Name = "Role Request Approved"
            },
            new NotificationType
            {
                Id = 2,
                Name = "Role Request Rejected"
            },
            new NotificationType
            {
                Id = 3,
                Name = "Added To Team"
            },
            new NotificationType
            {
                Id = 4,
                Name = "Removed From Team"
            },
            new NotificationType
            {
                Id = 5,
                Name = "Added As Project Leader"
            },
            new NotificationType
            {
                Id = 6,
                Name = "Added To Task"
            },
            new NotificationType
            {
                Id = 7,
                Name = "Added To Project"
            },
            new NotificationType
            {
                Id = 8,
                Name = "Project Waiting For Approve"
            },
            new NotificationType
            {
                Id = 9,
                Name = "Project Completion Approve"
            },
            new NotificationType
            {
                Id = 10,
                Name = "Project Completion Rejected"
            },
            new NotificationType
            {
                Id = 11,
                Name = "Task Waiting For Approve"
            },
            new NotificationType
            {
                Id = 12,
                Name = "Task Completion Approve"
            },
            new NotificationType
            {
                Id = 13,
                Name = "Task Completion Rejected"
            }
            );
    }
}

