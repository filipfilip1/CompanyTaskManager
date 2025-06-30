
using AutoMapper;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompanyTaskManager.Application.Services.WorkStatuses;

public class WorkStatusService(ApplicationDbContext _context,
    IMapper _mapper,
    ILogger<WorkStatusService> _logger) : IWorkStatusService
{
    public async Task<List<string>> GetAllWorkStatusesAsync()
    {
        _logger.LogInformation("Fetching all work statuses");
        
        var workStatuses = await _context.WorkStatuses
            .Select(ws => ws.Name) 
            .ToListAsync();

        _logger.LogDebug("Found {StatusCount} work statuses", workStatuses.Count);
        return workStatuses;
    }
}
