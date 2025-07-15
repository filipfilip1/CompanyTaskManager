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

public class StandaloneTaskServiceTests : ServiceTestBase
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly StandaloneTaskService _service;

    public StandaloneTaskServiceTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _service = new StandaloneTaskService(
            Context,
            _notificationServiceMock.Object,
            Mapper,
            UserManagerMock.Object,
            CreateLoggerMock<StandaloneTaskService>().Object
        );
    }

    [Fact]
    public async Task GetTasksForEmployeeAsync_ShouldReturnOnlyStandaloneTasks()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var project = await DataBuilder.CreateProjectAsync(manager.Id);
        
        // Create standalone task (ProjectId = null)
        var standaloneTask = await DataBuilder.CreateTaskItemAsync(employee.Id, "Standalone Task", null);
        
        // Create project task (should not be returned)
        var projectTask = await DataBuilder.CreateTaskItemAsync(employee.Id, "Project Task", project.Id);

        // Act
        var result = await _service.GetTasksForEmployeeAsync(employee.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Title.ShouldBe("Standalone Task");
        result[0].Id.ShouldBe(standaloneTask.Id);
    }

    [Fact]
    public async Task GetEmployeeTaskDetailsAsync_ShouldReturnTask_WhenUserAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Standalone Task", null);

        // Act
        var result = await _service.GetEmployeeTaskDetailsAsync(task.Id, employee.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(task.Id);
        result.Title.ShouldBe("Test Standalone Task");
        result.AssignedUserName.ShouldBe("employee");
        result.CanSendForApproval.ShouldBeTrue();
    }

    [Fact]
    public async Task GetEmployeeTaskDetailsAsync_ShouldReturnNull_WhenUserNotAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var employee1 = await DataBuilder.CreateUserAsync(userName: "employee1");
        var employee2 = await DataBuilder.CreateUserAsync(userName: "employee2");
        var task = await DataBuilder.CreateTaskItemAsync(employee1.Id, "Test Task", null);

        // Act
        var result = await _service.GetEmployeeTaskDetailsAsync(task.Id, employee2.Id);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldUpdateTaskAndNotifyManager()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        await DataBuilder.AssignUserToTeamAsync(employee.Id, team.Id);
        
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null, workStatusId: 1);

        // Act
        await _service.SendForApprovalAsync(task.Id, employee.Id);

        // Assert
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.WorkStatusId.ShouldBe(3); // Completion Pending
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                manager.Id,
                It.Is<string>(msg => msg.Contains("employee") && msg.Contains("Test Task")),
                11
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task SendForApprovalAsync_ShouldThrowValidation_WhenTaskNotActive()
    {
        // Arrange
        await InitializeAsync();
        
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null, workStatusId: 4); // Completed

        // Act & Assert
        await Should.ThrowAsync<ValidationException>(
            async () => await _service.SendForApprovalAsync(task.Id, employee.Id)
        );
    }

    [Fact]
    public async Task GetTasksForManagerAsync_ShouldReturnTeamStandaloneTasks()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var otherManager = await DataBuilder.CreateUserAsync(userName: "othermanager");
        var otherEmployee = await DataBuilder.CreateUserAsync(userName: "otheremployee");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        var otherTeam = await DataBuilder.CreateTeamAsync(otherManager.Id);
        
        await DataBuilder.AssignUserToTeamAsync(employee.Id, team.Id);
        await DataBuilder.AssignUserToTeamAsync(otherEmployee.Id, otherTeam.Id);
        
        var teamTask = await DataBuilder.CreateTaskItemAsync(employee.Id, "Team Task", null);
        var otherTeamTask = await DataBuilder.CreateTaskItemAsync(otherEmployee.Id, "Other Team Task", null);

        // Act
        var result = await _service.GetTasksForManagerAsync(manager.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Title.ShouldBe("Team Task");
    }

    [Fact]
    public async Task ApproveTaskAsync_ShouldCompleteTask_WhenValidManager()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        await DataBuilder.AssignUserToTeamAsync(employee.Id, team.Id);
        
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null, workStatusId: 3);

        // Act
        await _service.ApproveTaskAsync(task.Id, manager.Id);

        // Assert
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
    public async Task ApproveTaskAsync_ShouldThrowUnauthorized_WhenNotTeamManager()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var otherManager = await DataBuilder.CreateUserAsync(userName: "othermanager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        await DataBuilder.AssignUserToTeamAsync(employee.Id, team.Id);
        
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null, workStatusId: 3);

        // Act & Assert
        await Should.ThrowAsync<Application.Exceptions.UnauthorizedAccessException>(
            async () => await _service.ApproveTaskAsync(task.Id, otherManager.Id)
        );
    }

    [Fact]
    public async Task RejectTaskAsync_ShouldRejectTask_WhenValidManager()
    {
        // Arrange
        await InitializeAsync();
        
        var manager = await DataBuilder.CreateUserAsync(userName: "manager");
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var team = await DataBuilder.CreateTeamAsync(manager.Id);
        await DataBuilder.AssignUserToTeamAsync(employee.Id, team.Id);
        
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null, workStatusId: 3);

        // Act
        await _service.RejectTaskAsync(task.Id, manager.Id);

        // Assert
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
    public async Task CreateStandaloneTaskAsync_ShouldCreateTask_WhenValidUser()
    {
        // Arrange
        await InitializeAsync();
        
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        
        var model = new CreateTaskItemViewModel
        {
            Title = "New Standalone Task",
            Description = "Task description",
            AssignedUserId = employee.Id,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7)
        };

        // Act
        await _service.CreateStandaloneTaskAsync(model);

        // Assert
        var createdTask = await Context.TaskItems
            .FirstOrDefaultAsync(t => t.Title == "New Standalone Task" && t.ProjectId == null);
        
        createdTask.ShouldNotBeNull();
        createdTask.AssignedUserId.ShouldBe(employee.Id);
        createdTask.WorkStatusId.ShouldBe(1); // Active
        createdTask.ProjectId.ShouldBeNull();
        
        _notificationServiceMock.Verify(
            x => x.CreateNotificationAsync(
                employee.Id,
                It.Is<string>(msg => msg.Contains("assigned") && msg.Contains("New Standalone Task")),
                6
            ),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateStandaloneTaskAsync_ShouldThrowNotFound_WhenUserNotExists()
    {
        // Arrange
        await InitializeAsync();
        
        var model = new CreateTaskItemViewModel
        {
            Title = "New Task",
            AssignedUserId = "non-existent-user",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7)
        };

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(
            async () => await _service.CreateStandaloneTaskAsync(model)
        );
    }

    [Fact]
    public async Task UpdateSubmissionTextAsync_ShouldUpdateText_WhenUserAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var employee = await DataBuilder.CreateUserAsync(userName: "employee");
        var task = await DataBuilder.CreateTaskItemAsync(employee.Id, "Test Task", null);

        // Act
        await _service.UpdateSubmissionTextAsync(task.Id, employee.Id, "Updated submission text");

        // Assert
        var updatedTask = await Context.TaskItems.FindAsync(task.Id);
        updatedTask.SubmissionText.ShouldBe("Updated submission text");
    }

    [Fact]
    public async Task UpdateSubmissionTextAsync_ShouldThrowUnauthorized_WhenUserNotAssigned()
    {
        // Arrange
        await InitializeAsync();
        
        var employee1 = await DataBuilder.CreateUserAsync(userName: "employee1");
        var employee2 = await DataBuilder.CreateUserAsync(userName: "employee2");
        var task = await DataBuilder.CreateTaskItemAsync(employee1.Id, "Test Task", null);

        // Act & Assert
        await Should.ThrowAsync<Application.Exceptions.UnauthorizedAccessException>(
            async () => await _service.UpdateSubmissionTextAsync(task.Id, employee2.Id, "text")
        );
    }
}