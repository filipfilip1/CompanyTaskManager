
namespace CompanyTaskManager.Application.ViewModels.User;

public class UserViewModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
    public bool IsLockedOut { get; set; }
}
