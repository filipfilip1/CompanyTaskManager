

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyTaskManager.Application.ViewModels.Project;

public class CreateProjectViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string LeaderId { get; set; } = string.Empty;
    public string ManagerId { get; set; } = string.Empty;
    public string TeamId { get; set; } = string.Empty; 
    public List<SelectListItem> TeamMembers { get; set; } = new();
    public List<string> SelectedMemberIds { get; set; } = new();

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]  
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);
}
