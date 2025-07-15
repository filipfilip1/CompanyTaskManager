using AutoMapper;
using CompanyTaskManager.Application.MappingProfiles;
using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace CompanyTaskManager.IntegrationTests.Infrastructure;

public abstract class ServiceTestBase : IDisposable
{
    protected ApplicationDbContext Context { get; private set; }
    protected TestDataBuilder DataBuilder { get; private set; }
    protected IMapper Mapper { get; private set; }
    protected Mock<UserManager<ApplicationUser>> UserManagerMock { get; private set; }

    protected ServiceTestBase()
    {
        Context = TestDbContextFactory.CreateInMemoryContext();
        DataBuilder = new TestDataBuilder(Context);
        Mapper = CreateMapper();
        UserManagerMock = CreateUserManagerMock();
    }

    protected async Task InitializeAsync()
    {
        await TestDbContextFactory.CreateContextWithSeedDataAsync();
        await SeedBasicTestDataAsync();
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        return config.CreateMapper();
    }

    private static Mock<UserManager<ApplicationUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());
        return mgr;
    }

    protected virtual async Task SeedBasicTestDataAsync()
    {
        // Seed WorkStatuses
        if (!Context.WorkStatuses.Any())
        {
            var workStatuses = new List<WorkStatus>
            {
                new() { Id = 1, Name = "Active" },
                new() { Id = 2, Name = "Inactive" },
                new() { Id = 3, Name = "Completion Pending" },
                new() { Id = 4, Name = "Completed" },
                new() { Id = 5, Name = "Rejected" }
            };
            Context.WorkStatuses.AddRange(workStatuses);
        }

        // Seed NotificationTypes
        if (!Context.NotificationTypes.Any())
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
            Context.NotificationTypes.AddRange(notificationTypes);
        }

        await Context.SaveChangesAsync();
    }

    protected static Mock<ILogger<T>> CreateLoggerMock<T>()
    {
        return new Mock<ILogger<T>>();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}