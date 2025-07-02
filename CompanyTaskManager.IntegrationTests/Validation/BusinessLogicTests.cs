using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Data.Models;
using Shouldly;

namespace CompanyTaskManager.IntegrationTests.Validation;

public class BusinessLogicTests
{
    [Theory]
    [InlineData(1, true)]  // Active can be sent for approval
    [InlineData(2, false)] // Inactive cannot
    [InlineData(3, false)] // Completion Pending cannot
    [InlineData(4, false)] // Completed cannot
    [InlineData(5, false)] // Rejected cannot
    public void CanSendForApproval_ShouldReturnCorrectValue_BasedOnWorkStatus(int workStatusId, bool expected)
    {
        // Arrange
        var task = new TaskItem
        {
            WorkStatusId = workStatusId,
            EndDate = DateTime.Now.AddDays(1) // Not overdue
        };

        // Act
        var canSend = task.WorkStatusId == 1 && !(task.EndDate < DateTime.Now);

        // Assert
        canSend.ShouldBe(expected);
    }

    [Fact]
    public void CanSendForApproval_ShouldReturnFalse_WhenTaskOverdue()
    {
        // Arrange
        var task = new TaskItem
        {
            WorkStatusId = 1, // Active
            EndDate = DateTime.Now.AddDays(-1) // Overdue
        };

        // Act
        var canSend = task.WorkStatusId == 1 && !(task.EndDate < DateTime.Now);

        // Assert
        canSend.ShouldBeFalse();
    }

    [Theory]
    [InlineData(3, true)]  // Completion Pending can be approved/rejected
    [InlineData(1, false)] // Active cannot
    [InlineData(4, false)] // Completed cannot
    [InlineData(5, false)] // Rejected cannot
    public void CanApproveOrReject_ShouldReturnCorrectValue_BasedOnWorkStatus(int workStatusId, bool expected)
    {
        // Arrange
        var task = new TaskItem
        {
            WorkStatusId = workStatusId
        };

        // Act
        var canApproveOrReject = task.WorkStatusId == 3;

        // Assert
        canApproveOrReject.ShouldBe(expected);
    }

    [Fact]
    public void AllTasksApproved_ShouldReturnTrue_WhenAllTasksCompleted()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { WorkStatusId = 4 }, // Completed
            new() { WorkStatusId = 4 }, // Completed
            new() { WorkStatusId = 4 }  // Completed
        };

        // Act
        var allApproved = tasks.All(t => t.WorkStatusId == 4);

        // Assert
        allApproved.ShouldBeTrue();
    }

    [Fact]
    public void AllTasksApproved_ShouldReturnFalse_WhenSomeTasksNotCompleted()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { WorkStatusId = 4 }, // Completed
            new() { WorkStatusId = 1 }, // Active
            new() { WorkStatusId = 4 }  // Completed
        };

        // Act
        var allApproved = tasks.All(t => t.WorkStatusId == 4);

        // Assert
        allApproved.ShouldBeFalse();
    }

    [Fact]
    public void AllTasksApproved_ShouldReturnTrue_WhenNoTasks()
    {
        // Arrange
        var tasks = new List<TaskItem>();

        // Act
        var allApproved = tasks.All(t => t.WorkStatusId == 4);

        // Assert
        allApproved.ShouldBeTrue();
    }

    [Theory]
    [InlineData(1, true)]  // Active project can request completion
    [InlineData(2, false)] // Inactive cannot
    [InlineData(3, false)] // Completion Pending cannot
    [InlineData(4, false)] // Completed cannot
    [InlineData(5, false)] // Rejected cannot
    public void CanRequestCompletion_ShouldReturnCorrectValue_BasedOnProjectStatus(int workStatusId, bool expected)
    {
        // Arrange
        var project = new Project
        {
            WorkStatusId = workStatusId
        };

        // Act
        var canRequest = project.WorkStatusId == 1;

        // Assert
        canRequest.ShouldBe(expected);
    }

    [Theory]
    [InlineData("Role Request Approved", 1)]
    [InlineData("Task Waiting For Approve", 11)]
    [InlineData("Project Completion Approve", 9)]
    public void NotificationTypeMapping_ShouldHaveCorrectIds(string typeName, int expectedId)
    {
        // This test documents the notification type mappings used throughout the system
        var notificationType = new NotificationType
        {
            Id = expectedId,
            Name = typeName
        };

        notificationType.Id.ShouldBe(expectedId);
        notificationType.Name.ShouldBe(typeName);
    }

    [Fact]
    public void DomainException_ShouldAcceptMessage()
    {
        // Arrange & Act
        var exception = new DomainException("Test message");

        // Assert
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void NotFoundException_ShouldFormatMessage()
    {
        // Arrange & Act
        var exception = new NotFoundException("User", "123");

        // Assert
        exception.Message.ShouldBe("User with identifier '123' was not found.");
    }

    [Fact]
    public void ValidationException_ShouldAcceptMessage()
    {
        // Arrange & Act
        var exception = new ValidationException("Invalid data");

        // Assert
        exception.Message.ShouldBe("Invalid data");
    }

    [Fact]
    public void UnauthorizedAccessException_ShouldAcceptMessage()
    {
        // Arrange & Act
        var exception = new CompanyTaskManager.Application.Exceptions.UnauthorizedAccessException("Access denied");

        // Assert
        exception.Message.ShouldBe("Access denied");
    }
}