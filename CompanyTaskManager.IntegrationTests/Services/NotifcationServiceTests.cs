

using AutoMapper;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Data.Models;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Moq;
using CompanyTaskManager.Application.ViewModels.Notification;

namespace CompanyTaskManager.IntegrationTests.Services;
public class NotificationServiceTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;

    public NotificationServiceTests()
    {
        // Initialize the mapper mock 
        _mapperMock = new Mock<IMapper>();
        _mapperMock
            .Setup(m => m.Map<List<NotificationViewModel>>(It.IsAny<List<Notification>>()))
            .Returns((List<Notification> source) =>
                source.Select(n => new NotificationViewModel
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    NotificationTypeName = n.NotificationType?.Name ?? string.Empty
                }).ToList());
        _mapper = _mapperMock.Object;
    }

    // Creates a new in-memory database context
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    // Helper: Seed realistic notification types into the database
    private async Task SeedTestNotificationTypes(ApplicationDbContext context)
    {
        var types = new List<NotificationType>
        {
            new NotificationType { Id = 1,  Name = "Role Request Approved" },
            new NotificationType { Id = 2,  Name = "Role Request Rejected" },
            new NotificationType { Id = 3,  Name = "Added To Team" },
            new NotificationType { Id = 4,  Name = "Removed From Team" },
            new NotificationType { Id = 5,  Name = "Added As Project Leader" },
            new NotificationType { Id = 6,  Name = "Added To Task" },
            new NotificationType { Id = 7,  Name = "Added To Project" },
            new NotificationType { Id = 8,  Name = "Project Waiting For Approve" },
            new NotificationType { Id = 9,  Name = "Project Completion Approve" },
            new NotificationType { Id = 10, Name = "Project Completion Rejected" },
            new NotificationType { Id = 11, Name = "Task Waiting For Approve" },
            new NotificationType { Id = 12, Name = "Task Completion Approve" },
            new NotificationType { Id = 13, Name = "Task Completion Rejected" }
        };
        context.NotificationTypes.AddRange(types);
        await context.SaveChangesAsync();
    }

    // Seed notifications for a given user with hardcoded types.
    private async Task SeedNotifications(ApplicationDbContext context, string userId)
    {
        if (!context.NotificationTypes.Any())
        {
            await SeedTestNotificationTypes(context);
        }

        var notifications = new List<Notification>
        {
            new Notification
            {
                Id = 1,
                UserId = userId,
                NotificationTypeId = 1,
                NotificationType = await context.NotificationTypes.FindAsync(1),
                Message = "Message 1",
                CreatedAt = new DateTime(2025, 1, 3),
                IsRead = false
            },
            new Notification
            {
                Id = 2,
                UserId = userId,
                NotificationTypeId = 2,
                NotificationType = await context.NotificationTypes.FindAsync(2),
                Message = "Message 2",
                CreatedAt = new DateTime(2025, 1, 2),
                IsRead = true
            },
            new Notification
            {
                Id = 3,
                UserId = userId,
                NotificationTypeId = 1,
                NotificationType = await context.NotificationTypes.FindAsync(1),
                Message = "Message 3",
                CreatedAt = new DateTime(2025, 1, 1),
                IsRead = false
            },
            new Notification
            {
                Id = 4,
                UserId = userId,
                NotificationTypeId = 2,
                NotificationType = await context.NotificationTypes.FindAsync(2),
                Message = "Message 4",
                CreatedAt = new DateTime(2025, 1, 4),
                IsRead = true
            }
        };

        context.Notifications.AddRange(notifications);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetUnreadNotificationsAsync_ShouldReturnOnlyUnread_ForGivenUser()
    {
        // ARRANGE
        var testUserId = "user123";
        using var context = CreateContext();
        await SeedTestNotificationTypes(context);

        // Seed notifications for testUser (2 unread, 1 read) and one notification for another user.
        context.Notifications.Add(new Notification
        {
            Id = 101,
            UserId = testUserId,
            IsRead = false,
            Message = "Unread #1",
            NotificationTypeId = 1,
            NotificationType = await context.NotificationTypes.FindAsync(1),
            CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0)
        });
        context.Notifications.Add(new Notification
        {
            Id = 102,
            UserId = testUserId,
            IsRead = true,  
            Message = "Read #2",
            NotificationTypeId = 2,
            NotificationType = await context.NotificationTypes.FindAsync(2),
            CreatedAt = new DateTime(2025, 1, 2, 10, 0, 0)
        });
        context.Notifications.Add(new Notification
        {
            Id = 103,
            UserId = testUserId,
            IsRead = false,
            Message = "Unread #3",
            NotificationTypeId = 2,
            NotificationType = await context.NotificationTypes.FindAsync(2),
            CreatedAt = new DateTime(2025, 1, 3, 9, 0, 0)
        });
        context.Notifications.Add(new Notification
        {
            Id = 999,
            UserId = "someOtherUser",
            IsRead = false,
            Message = "Another user",
            NotificationTypeId = 1,
            NotificationType = await context.NotificationTypes.FindAsync(1),
            CreatedAt = new DateTime(2025, 1, 5)
        });
        await context.SaveChangesAsync();

        var service = new NotificationService(context, _mapper);

        // ACT
        var unreadNotifications = await service.GetUnreadNotificationsAsync(testUserId);

        // ASSERT
        unreadNotifications.Count.ShouldBe(2);
        unreadNotifications[0].Id.ShouldBe(103); // newer (CreatedAt 2025-01-03 09:00)
        unreadNotifications[1].Id.ShouldBe(101); // (CreatedAt 2025-01-01 10:00)
        unreadNotifications[0].NotificationTypeName.ShouldBe("Role Request Rejected");
        unreadNotifications[1].NotificationTypeName.ShouldBe("Role Request Approved");

        // Verify that mapping was called exactly once
        _mapperMock.Verify(m => m.Map<List<NotificationViewModel>>(It.IsAny<List<Notification>>()), Times.Once);
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldMarkNotificationAsRead_WhenItExists()
    {
        // ARRANGE
        using var context = CreateContext();
        var notification = new Notification
        {
            Id = 200,
            UserId = "userX",
            IsRead = false,
            Message = "Test notification",
            CreatedAt = DateTime.UtcNow,
            NotificationTypeId = 1,
            NotificationType = new NotificationType { Id = 1, Name = "Role Request Approved" }
        };
        context.Notifications.Add(notification);
        await context.SaveChangesAsync();

        var service = new NotificationService(context, _mapper);

        // ACT
        await service.MarkAsReadAsync(notificationId: 200);

        // ASSERT
        var updatedNotifaction = await context.Notifications.FindAsync(200);
        updatedNotifaction.ShouldNotBeNull();
        updatedNotifaction.IsRead.ShouldBeTrue();
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldDoNothing_WhenNotificationDoesNotExist()
    {
        // ARRANGE
        using var context = CreateContext();
        var service = new NotificationService(context, _mapper);

        // ACT
        await service.MarkAsReadAsync(notificationId: 999);

        // ASSERT - the database should remain unchanged (empty)
        var anyNotification = await context.Notifications.CountAsync();
        anyNotification.ShouldBe(0);
    }

    [Fact]
    public async Task CreateNotificationAsync_ShouldCreateNewNotification()
    {
        // ARRANGE
        using var context = CreateContext();
        await SeedTestNotificationTypes(context); 

        var service = new NotificationService(context, _mapper);
        var userId = "user123";
        var message = "Welcome";
        var notificationTypeId = 1;

        // ACT
        await service.CreateNotificationAsync(userId, message, notificationTypeId);

        // ASSERT
        var notifications = await context.Notifications.ToListAsync();
        notifications.Count.ShouldBe(1);
        var created = notifications.First();
        created.UserId.ShouldBe("user123");
        created.Message.ShouldBe("Welcome");
        created.NotificationTypeId.ShouldBe(1);
        created.IsRead.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllNotificationsForUserAsync_ShouldReturnAllOrdered_UnreadFirst()
    {
        // ARRANGE
        var userId = "user1";
        using var context = CreateContext();

        await SeedNotifications(context, userId);

        var service = new NotificationService(context, _mapper);

        // ACT
        var result = await service.GetAllNotificationsForUserAsync(userId);

        // ASSERT
        result.Count.ShouldBe(4);
        // First two should be unread
        result[0].IsRead.ShouldBeFalse();
        result[1].IsRead.ShouldBeFalse();
        // Next two should be read
        result[2].IsRead.ShouldBeTrue();
        result[3].IsRead.ShouldBeTrue();

        // Verify ordering: within unread group, sort descending by CreatedAt
        result[0].CreatedAt.ShouldBeGreaterThan(result[1].CreatedAt);
        result[2].CreatedAt.ShouldBeGreaterThan(result[3].CreatedAt);

        // Verify that mapping was called exactly once
        _mapperMock.Verify(m => m.Map<List<NotificationViewModel>>(It.IsAny<List<Notification>>()), Times.Once);
    }


    [Fact]
    public async Task GetAllNotificationsForUserAsync_ShouldReturnEmptyList_WhenNoNotificationsExist()
    {
        // ARRANGE
        var userId = "userWithoutNotifications";
        using var context = CreateContext();
        await SeedTestNotificationTypes(context);

        var service = new NotificationService(context, _mapper);

        // ACT
        var result = await service.GetAllNotificationsForUserAsync(userId);

        // ASSERT
        result.ShouldBeEmpty();

        // Verify that mapping was called exactly once
        _mapperMock.Verify(m => m.Map<List<NotificationViewModel>>(It.IsAny<List<Notification>>()), Times.Once);
    }
}