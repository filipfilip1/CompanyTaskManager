

using CompanyTaskManager.Application.Services.Notifications;
using CompanyTaskManager.Application.Services.RoleRequests;
using CompanyTaskManager.Application.Services.Teams;
using CompanyTaskManager.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CompanyTaskManager.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IRoleRequestsService, RoleRequestService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ITeamService, TeamService>();

        return services;
    }
}
