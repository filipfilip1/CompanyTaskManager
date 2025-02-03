
using AutoMapper;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyTaskManager.Application.Services.WorkStatuses;

public class WorkStatusService(ApplicationDbContext _context,
    IMapper _mapper) : IWorkStatusService
{
    public async Task<List<string>> GetAllWorkStatusesAsync()
    {
        var workStatuses = await _context.WorkStatuses
            .Select(ws => ws.Name) 
            .ToListAsync();

        return workStatuses;
    }
}
