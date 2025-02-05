using CompanyTaskManager.Common.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "d61a13f6-088a-4250-a9e1-f55d3978ee1f",
                Name = Roles.Employee,
                NormalizedName = Roles.Employee.ToUpper()
            },
            new IdentityRole
            {
                Id = "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f",
                Name = Roles.Manager,
                NormalizedName = Roles.Manager.ToUpper()
            },
            new IdentityRole
            {
                Id = "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad",
                Name = Roles.Administrator,
                NormalizedName = Roles.Administrator.ToUpper()
            }
        );
    }
}
