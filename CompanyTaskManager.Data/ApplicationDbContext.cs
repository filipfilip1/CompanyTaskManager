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

        // Team->Members
        builder.Entity<Team>()
            .HasMany(t => t.Members)
            .WithOne(u => u.Team)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.Restrict); 

        // Team->Manager
        builder.Entity<Team>()
            .HasOne(t => t.Manager)
            .WithMany() 
            .HasForeignKey(t => t.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Project->Leader
        builder.Entity<Project>()
            .HasOne(p => p.Leader)
            .WithMany() 
            .HasForeignKey(p => p.LeaderId)
            .OnDelete(DeleteBehavior.Restrict); 

        // Project->Manager
        builder.Entity<Project>()
            .HasOne(p => p.Manager)
            .WithMany()
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.ProjectUsers)
            .HasForeignKey(pu => pu.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<ProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany(u => u.ProjectUsers)
            .HasForeignKey(pu => pu.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TaskItem>()
            .HasOne(t => t.WorkStatus)
            .WithMany() 
            .HasForeignKey(t => t.WorkStatusId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<RoleRequest> RoleRequests { get; set; }
    public DbSet<RequestStatus> RequestStatuses { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<WorkStatus> WorkStatuses { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
}
