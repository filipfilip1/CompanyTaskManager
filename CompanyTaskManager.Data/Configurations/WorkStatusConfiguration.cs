using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class WorkStatusConfiguration : IEntityTypeConfiguration<WorkStatus>
{
    public void Configure(EntityTypeBuilder<WorkStatus> builder)
    {
        builder.HasData(
            new WorkStatus { Id = 1, Name = "Active" },
            new WorkStatus { Id = 2, Name = "In Progress" },
            new WorkStatus { Id = 3, Name = "Completion Pending" },
            new WorkStatus { Id = 4, Name = "Completed" },
            new WorkStatus { Id = 5, Name = "Rejected" }
        );
    }
}
