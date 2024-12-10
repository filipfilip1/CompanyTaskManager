

using CompanyTaskManager.Application.ViewModels.User;

namespace CompanyTaskManager.Application.Services.Users;

public interface IUserService
{
    Task<List<UserViewModel>> GetAllUsersAsync(string searchString = null);
    Task<UserViewModel> GetUserByIdAsync(string userId);
    Task BlockUserAsync(string userId);
    Task UnblockUserAsync(string userId);
}
