using AutoMapper;
using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Data.Models;


namespace CompanyTaskManager.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RoleRequest, RoleRequestViewModel>();
        CreateMap<RoleRequest, RoleRequestSummaryViewModel>();
    }
}
