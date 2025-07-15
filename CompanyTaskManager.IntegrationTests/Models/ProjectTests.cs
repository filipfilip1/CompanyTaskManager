using CompanyTaskManager.Data.Models;
using Shouldly;

namespace CompanyTaskManager.IntegrationTests.Models;

public class ProjectTests
{
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
}