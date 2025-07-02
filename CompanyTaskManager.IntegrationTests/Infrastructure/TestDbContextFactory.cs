using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.IntegrationTests.Infrastructure;

public static class TestDbContextFactory
{
    public static ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }

    public static async Task<ApplicationDbContext> CreateContextWithSeedDataAsync()
    {
        var context = CreateInMemoryContext();
        await SeedBasicDataAsync(context);
        return context;
    }

    private static async Task SeedBasicDataAsync(ApplicationDbContext context)
    {
        await SeedWorkStatusesAsync(context);
        await SeedNotificationTypesAsync(context);
        await SeedRequestStatusesAsync(context);
        await context.SaveChangesAsync();
    }

    private static async Task SeedWorkStatusesAsync(ApplicationDbContext context)
    {
        var workStatuses = new List<WorkStatus>
        {
            new() { Id = 1, Name = "Active" },
            new() { Id = 2, Name = "Inactive" },
            new() { Id = 3, Name = "Completion Pending" },
            new() { Id = 4, Name = "Completed" },
            new() { Id = 5, Name = "Rejected" }
        };
        
        context.WorkStatuses.AddRange(workStatuses);
    }

    private static async Task SeedNotificationTypesAsync(ApplicationDbContext context)
    {
        var notificationTypes = new List<NotificationType>
        {
            new() { Id = 1, Name = "Role Request Approved" },
            new() { Id = 2, Name = "Role Request Rejected" },
            new() { Id = 3, Name = "Added To Team" },
            new() { Id = 4, Name = "Removed From Team" },
            new() { Id = 5, Name = "Added As Project Leader" },
            new() { Id = 6, Name = "Added To Task" },
            new() { Id = 7, Name = "Added To Project" },
            new() { Id = 8, Name = "Project Waiting For Approve" },
            new() { Id = 9, Name = "Project Completion Approve" },
            new() { Id = 10, Name = "Project Completion Rejected" },
            new() { Id = 11, Name = "Task Waiting For Approve" },
            new() { Id = 12, Name = "Task Completion Approve" },
            new() { Id = 13, Name = "Task Completion Rejected" }
        };
        
        context.NotificationTypes.AddRange(notificationTypes);
    }

    private static async Task SeedRequestStatusesAsync(ApplicationDbContext context)
    {
        var requestStatuses = new List<RequestStatus>
        {
            new() { Id = 1, Name = "Pending" },
            new() { Id = 2, Name = "Approved" },
            new() { Id = 3, Name = "Rejected" }
        };
        
        context.RequestStatuses.AddRange(requestStatuses);
    }
}