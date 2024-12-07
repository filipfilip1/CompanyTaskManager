
using System.ComponentModel.DataAnnotations;

namespace CompanyTaskManager.Data.Models;

public class RequestStatus : BaseEntity
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}
