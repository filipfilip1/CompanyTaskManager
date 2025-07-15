using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.Services.TaskItems;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Data.Models;
using CompanyTaskManager.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace CompanyTaskManager.IntegrationTests.Services;

public class ProjectTaskServiceTests : ServiceTestBase
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly ProjectTaskService _service;

    public ProjectTaskServiceTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _service = new ProjectTaskService(
            Context,
            _notificationServiceMock.Object,
            Mapper,
            UserManagerMock.Object,
            CreateLoggerMock<ProjectTaskService>().Object
        );
    }

    [Fact]
    public async Task GetEmployeeTaskDetailsAsync_ShouldReturnTask_WhenUserIsAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var leader = await DataBuilder.CreateUserAsync(userName: "leader");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        var project = await DataBuilder.CreateProjectAsync(manager.Id, leaderId: leader.Id, teamId: team.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id);

        // Act
        var result = await _service.GetEmployeeTaskDetailsAsync(task.Id, employee.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(task.Id);
        result.Title.ShouldBe("Test Task");
        result.AssignedUserName.ShouldBe("employee");
        result.CanSendForApproval.ShouldBeTrue();
    }

    [Fact]
    public async Task GetEmployeeTaskDetailsAsync_ShouldReturnNull_WhenUserNotAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee1 = await DataBuilder.CreateUserAsync(userName: "employee1");
        var employee2 = await DataBuilder.CreateUserAsync(userName: "employee2");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee1.Id, "Test Task", project.Id);

        // Act
        var result = await _service.GetEmployeeTaskDetailsAsync(task.Id, employee2.Id);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldUpdateTaskStatus_WhenValid()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var leader = await DataBuilder.CreateUserAsync(userName: "leader");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        var project = await DataBuilder.CreateProjectAsync(manager.Id, leaderId: leader.Id, teamId: team.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id, workStatusId: 1);

        // Act
        await _service.SendForApprovalAsync(task.Id, employee.Id);

        // Assert
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.WorkStatusId.ShouldBe(3); // Completion Pending
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                leader.Id,
                It.Is<string>(msg => msg.Contains("employee") && msg.Contains("Test Task")),
                11
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldThrowUnauthorized_WhenUserNotAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee1 = await DataBuilder.CreateUserAsync(userName: "employee1");
        var employee2 = await DataBuilder.CreateUserAsync(userName: "employee2");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee1.Id, "Test Task", project.Id);

        // Act & Assert
        await Should.ThrowAsync<Application.Exceptions.UnauthorizedAccessException>(
            async () => await _service.SendForApprovalAsync(task.Id, employee2.Id)
        );
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldThrowValidation_WhenTaskNotActive()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id, workStatusId: 4); // Completed

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(
            async () => await _service.SendForApprovalAsync(task.Id, employee.Id)
        );
    }

    [Fact]
    public async Task ApproveProjectTaskAsync_ShouldCompleteTask_WhenValidLeader()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var leader = await DataBuilder.CreateUserAsync(userName: "leader");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id, leaderId: leader.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id, workStatusId: 3);

        // Act
        var result = await _service.ApproveProjectTaskAsync(task.Id, leader.Id);

        // Assert
        result.ShouldBe(project.Id);
        
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.WorkStatusId.ShouldBe(4); // Completed
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                employee.Id,
                It.Is<string>(msg => msg.Contains("approved")),
                12
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task ApproveProjectTaskAsync_ShouldThrowUnauthorized_WhenNotLeader()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var notLeader = await DataBuilder.CreateUserAsync(userName: "notleader");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id, workStatusId: 3);

        // Act & Assert
        await Should.ThrowAsync<Application.Exceptions.UnauthorizedAccessException>(
            async () => await _service.ApproveProjectTaskAsync(task.Id, notLeader.Id)
        );
    }

    [Fact]
    public async Task RejectProjectTaskAsync_ShouldRejectTask_WhenValidLeader()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var leader = await DataBuilder.CreateUserAsync(userName: "leader");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id, leaderId: leader.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id, workStatusId: 3);

        // Act
        var result = await _service.RejectProjectTaskAsync(task.Id, leader.Id);

        // Assert
        result.ShouldBe(project.Id);
        
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.WorkStatusId.ShouldBe(5); // Rejected
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                employee.Id,
                It.Is<string>(msg => msg.Contains("rejected")),
                13
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateProjectTaskAsync_ShouldCreateTask_WhenValidManager()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        
        var model = new CreateTaskItemViewModel
        {
            Title = "New Task",
            Description = "Task description",
            AssignedUserId = employee.Id,
            ProjectId = project.Id,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7)
        };

        // Act
        await _service.CreateProjectTaskAsync(model, manager.Id);

        // Assert
        var createdTask = await Context.TaskItems
            .FirstOrDefaultAsync(t => t.Title == "New Task" && t.ProjectId == project.Id);
        
        createdTask.ShouldNotBeNull();
        createdTask.AssignedUserId.ShouldBe(employee.Id);
        createdTask.WorkStatusId.ShouldBe(1); // Active
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                employee.Id,
                It.Is<string>(msg => msg.Contains("assigned") && msg.Contains("New Task")),
                6
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateProjectTaskAsync_ShouldThrowNotFound_WhenUserNotExists()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        
        var model = new CreateTaskItemViewModel
        {
            Title = "New Task",
            AssignedUserId = "non-existent-user",
            ProjectId = project.Id,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7)
        };

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(
            async () => await _service.CreateProjectTaskAsync(model, manager.Id)
        );
    }

    [Fact]
    public async Task UpdateSubmissionTextAsync_ShouldUpdateText_WhenUserAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", project.Id);

        // Act
        await _service.UpdateSubmissionTextAsync(task.Id, employee.Id, "New submission text");

        // Assert
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.SubmissionText.ShouldBe("New submission text");
    }
}