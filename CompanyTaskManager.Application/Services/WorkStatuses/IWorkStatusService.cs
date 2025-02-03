

namespace CompanyTaskManager.Application.Services.WorkStatuses;

public interface IWorkStatusService
{
    public Task<List<string>> GetAllWorkStatusesAsync();
}
