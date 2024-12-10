using AutoMapper;
using CompanyTaskManager.Application.ViewModels.Notification;
using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Data.Models;


namespace CompanyTaskManager.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RoleRequest, RoleRequestViewModel>();
        CreateMap<RoleRequest, RoleRequestSummaryViewModel>();

        CreateMap<Notification, NotificationViewModel>()
            .ForMember(dest => dest.NotificationTypeName, opt => opt.MapFrom(src => src.NotificationType.Name))
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead));

    }
}
