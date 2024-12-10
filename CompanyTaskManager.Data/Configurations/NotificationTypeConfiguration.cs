﻿

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
            }
            );
    }
}

