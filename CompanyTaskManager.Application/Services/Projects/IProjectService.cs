

using CompanyTaskManager.Application.ViewModels.Project;
using CompanyTaskManager.Application.ViewModels.User;

namespace CompanyTaskManager.Application.Services.Projects;

public interface IProjectService
{
    Task<List<ProjectIndexViewModel>> GetProjectsByUserAsync(string userId);

    Task<ProjectDetailsViewModel> GetProjectByIdAsync(int projectId);
    Task CreateProjectAsync(CreateProjectViewModel projectViewModel);
    Task AssignProjectLeader(int projectId, string leaderId, string managerId);

    Task RequestProjectCompletionAsync(int projectId);
    Task ApproveProjectAsync(int projectId);
    Task RejectProjectCompletionAsync(int projectId);
}
