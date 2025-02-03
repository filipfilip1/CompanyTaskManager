using AutoMapper;
using CompanyTaskManager.Application.ViewModels.Notification;
using CompanyTaskManager.Application.ViewModels.Project;
using CompanyTaskManager.Application.ViewModels.RoleRequest;
using CompanyTaskManager.Application.ViewModels.TaskItem;
using CompanyTaskManager.Application.ViewModels.User;
using CompanyTaskManager.Data.Models;

namespace CompanyTaskManager.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ApplicationUser, UserViewModel>();


        CreateMap<RoleRequest, RoleRequestViewModel>();
        CreateMap<RoleRequest, RoleRequestSummaryViewModel>();


        CreateMap<Notification, NotificationViewModel>()
            .ForMember(dest => dest.NotificationTypeName, opt => opt.MapFrom(src => src.NotificationType.Name));


        CreateMap<Project, ProjectDetailsViewModel>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.UserName : string.Empty))
            .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : string.Empty))
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus != null ? src.WorkStatus.Name : string.Empty))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.ProjectMembers));

        CreateMap<Project, ProjectIndexViewModel>()
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus.Name))
            .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src.Leader.FirstName + " " + src.Leader.LastName));

        CreateMap<CreateProjectViewModel, Project>(MemberList.Source);



        CreateMap<TaskItem, TaskItemViewModel>()
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus != null ? src.WorkStatus.Name : string.Empty));

        CreateMap<TaskItem, ProjectTaskItemViewModel>()
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus.Name))
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.UserName));

        CreateMap<TaskItem, ManagerTaskItemViewModel>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.UserName))
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus.Name));

        CreateMap<TaskItem, ManagerTaskDetailsViewModel>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser.UserName))
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus.Name));

        CreateMap<TaskItem, StandaloneTaskItemViewModel>()
            .ForMember(dest => dest.AssignedUserName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.UserName : ""))
            .ForMember(dest => dest.WorkStatusName, opt => opt.MapFrom(src => src.WorkStatus != null ? src.WorkStatus.Name : ""));


    }
}
