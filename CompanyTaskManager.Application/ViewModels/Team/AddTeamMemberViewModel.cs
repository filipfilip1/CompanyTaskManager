

using CompanyTaskManager.Application.ViewModels.User;

namespace CompanyTaskManager.Application.ViewModels.Team;

public class AddTeamMemberViewModel
{
    public List<UserViewModel> AvailableUsers { get; set; } = new List<UserViewModel>();
}
