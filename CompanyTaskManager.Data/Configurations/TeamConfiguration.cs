

using CompanyTaskManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasData( 
            new Team
            {
                Id = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                ManagerId = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                Name = "Manager's Team"
            });
    }
}
