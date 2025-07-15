using CompanyTaskManager.Data;
using CompanyTaskManager.Data.Models;

namespace CompanyTaskManager.IntegrationTests.Infrastructure;

public class TestDataBuilder
{
    private readonly ApplicationDbContext _context;

    public TestDataBuilder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> CreateUserAsync(
        string id = null, 
        string userName = "testuser", 
        string email = "test@example.com")
    {
        var user = new ApplicationUser
        {
            Id = id ?? Guid.NewGuid().ToString(),
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            NormalizedUserName = userName.ToUpper(),
            NormalizedEmail = email.ToUpper()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Team> CreateTeamAsync(
        string managerId, 
        string name = "Test Team")
    {
        var team = new Team
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            ManagerId = managerId
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
        return team;
    }

    public async Task<Project> CreateProjectAsync(
        string managerId,
        string name = "Test Project",
        string leaderId = null,
        string teamId = null,
        int workStatusId = 1)
    {
        var project = new Project
        {
            Name = name,
            Description = "Test project description",
            ManagerId = managerId,
            LeaderId = leaderId ?? string.Empty,
            TeamId = teamId ?? string.Empty,
            WorkStatusId = workStatusId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(30)
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<TaskItem> CreateTaskItemAsync(
        string assignedUserId,
        string title = "Test Task",
        int? projectId = null,
        int workStatusId = 1,
        string submissionText = null)
    {
        var task = new TaskItem
        {
            Title = title,
            Description = "Test task description",
            AssignedUserId = assignedUserId,
            ProjectId = projectId,
            WorkStatusId = workStatusId,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7),
            SubmissionText = submissionText
        };

        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<ProjectUser> CreateProjectUserAsync(string userId, int projectId)
    {
        var projectUser = new ProjectUser
        {
            UserId = userId,
            ProjectId = projectId
        };

        _context.ProjectUsers.Add(projectUser);
        await _context.SaveChangesAsync();
        return projectUser;
    }

    public async Task<Notification> CreateNotificationAsync(
        string userId,
        string message = "Test notification",
        int notificationTypeId = 1,
        bool isRead = false)
    {
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            NotificationTypeId = notificationTypeId,
            IsRead = isRead,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task AssignUserToTeamAsync(string userId, string teamId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.TeamId = teamId;
            await _context.SaveChangesAsync();
        }
    }
}