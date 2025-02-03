

using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Data.Models;

namespace CompanyTaskManager.Application.Services.Teams;

public interface ITeamService 
{
    Task<List<UserViewModel>> GetAvailableUserAsync();
    Task AddMemberAsync(string teamId, string userId);
    Task RemoveMemberAsync(string teamId, string userId);
    Task<List<UserViewModel>> GetTeamMembersAsync(string teamId);
    Task CreateTeamAsync(string managerId, string teamName);
    Task<List<UserViewModel>> GetProjectMembersAsync(int projectId);
    Task<List<UserViewModel>> GetTeamMembersForCreateTaskAsync(string teamId);
}
