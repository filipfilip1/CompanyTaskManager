using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CompanyTaskManager.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Team>()
            .HasMany(t => t.Members)
            .WithOne(u => u.Team)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Team>()
            .HasOne(t => t.Manager)
            .WithMany() 
            .HasForeignKey(t => t.ManagerId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<RoleRequest> RoleRequests { get; set; }
    public DbSet<RequestStatus> RequestStatuses { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }
    public DbSet<Team> Teams { get; set; }
}
