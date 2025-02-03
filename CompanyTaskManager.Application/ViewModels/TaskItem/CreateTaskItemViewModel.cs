using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CompanyTaskManager.Application.ViewModels.TaskItem;

public class CreateTaskItemViewModel
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? ProjectId { get; set; }  // null => stand-alone

    [Required]
    public string AssignedUserId { get; set; } = string.Empty;

    // List to be filled in the controller
    public List<SelectListItem> TeamMembers { get; set; } = new();
    public List<SelectListItem> Projects { get; set; } = new();

    [Required]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);
}