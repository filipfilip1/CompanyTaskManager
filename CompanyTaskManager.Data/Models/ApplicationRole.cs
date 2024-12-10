

using Microsoft.AspNetCore.Identity;

namespace CompanyTaskManager.Data.Models;

public class ApplicationRole : IdentityRole
{
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
}
