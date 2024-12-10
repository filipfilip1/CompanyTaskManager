
using CompanyTaskManager.Application.ViewModels.User;

namespace CompanyTaskManager.Application.ViewModels.Team;

public class ManageTeamViewModel
{
    public  List<UserViewModel> TeamMembers { get; set; } = new List<UserViewModel>();
}
