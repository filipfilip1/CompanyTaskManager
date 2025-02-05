using CompanyTaskManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyTaskManager.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(new ApplicationUser
        {
            Id = "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            NormalizedUserName = "ADMIN@LOCALHOST.COM",
            UserName = "admin@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "Admin",
        }, new ApplicationUser
        {
            Id = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
            Email = "manager@localhost.com",
            NormalizedEmail = "MANAGER@LOCALHOST.COM",
            NormalizedUserName = "MANAGER@LOCALHOST.COM",
            UserName = "manager@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "Manager",
        }, new ApplicationUser
        {
            Id = "56d078c0-f671-4a22-b3fd-977f2ac33eae",
            Email = "employee1@localhost.com",
            NormalizedEmail = "EMPLOYEE1@LOCALHOST.COM",
            NormalizedUserName = "EMPLOYEE1@LOCALHOST.COM",
            UserName = "employee1@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "employee1",
        }, new ApplicationUser
        {
            Id = "31dd98c5-71e1-4cbf-81a0-fd995bb0a735",
            Email = "employee2@localhost.com",
            NormalizedEmail = "EMPLOYEE2@LOCALHOST.COM",
            NormalizedUserName = "EMPLOYEE2@LOCALHOST.COM",
            UserName = "employee2@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "employee2",
        }, new ApplicationUser
        {
            Id = "c47afd54-3108-4ee6-8edc-a0bc2688ebba",
            Email = "employee3@localhost.com",
            NormalizedEmail = "EMPLOYEE3@LOCALHOST.COM",
            NormalizedUserName = "EMPLOYEE3@LOCALHOST.COM",
            UserName = "employee3@localhost.com",
            PasswordHash = hasher.HashPassword(null, "P@ssword1"),
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "employee3",
        });
    }
}
