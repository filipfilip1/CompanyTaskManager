
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad",
                UserId = "4b0b2aeb-474e-45f2-8899-e4a1536a52bf"
            }, new IdentityUserRole<string>
            {
                RoleId = "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f",
                UserId = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb"
            }, new IdentityUserRole<string>
            {
                RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f",
                UserId = "56d078c0-f671-4a22-b3fd-977f2ac33eae"
            }, new IdentityUserRole<string>
            {
                RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f",
                UserId = "31dd98c5-71e1-4cbf-81a0-fd995bb0a735"
            }, new IdentityUserRole<string>
            {
                RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f",
                UserId = "c47afd54-3108-4ee6-8edc-a0bc2688ebba"
            });
    }
}
