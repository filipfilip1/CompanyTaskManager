

namespace CompanyTaskManager.Application.Dto;

public class CalendarEventDto
{
    public string Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Type { get; set; }
    public bool IsLeader { get; set; }
}
