﻿// <auto-generated />
using System;
using CompanyTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompanyTaskManager.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TeamId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("TeamId");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "ba80bf0a-962b-45a9-8503-069e0c8c32af",
                            Email = "admin@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "Default",
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@LOCALHOST.COM",
                            NormalizedUserName = "ADMIN@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEO/7mdlwuEqgkghm4VTYnn+X28MXFk0cxeKclN0+xd6zCgxVgwJnqVNYI2G50p+yZQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "7e99279e-286d-4594-b01f-1e143fb6efc7",
                            TwoFactorEnabled = false,
                            UserName = "admin@localhost.com"
                        },
                        new
                        {
                            Id = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f70bae9b-5690-421f-9ae2-ebd55a017bbd",
                            Email = "manager@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "Default",
                            LastName = "Manager",
                            LockoutEnabled = false,
                            NormalizedEmail = "MANAGER@LOCALHOST.COM",
                            NormalizedUserName = "MANAGER@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEMKCurknNjoi11r/VPWM7H0OQ3qGriT0ZGXcLr9ubeRVOOpa9nKWgn0uGRPFld/fTg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "fbd3afd7-4957-426b-8aa3-fe913458b2e5",
                            TwoFactorEnabled = false,
                            UserName = "manager@localhost.com"
                        },
                        new
                        {
                            Id = "56d078c0-f671-4a22-b3fd-977f2ac33eae",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "15167dc4-5aa2-4538-bc82-8199b5e2edde",
                            Email = "employee1@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "Default",
                            LastName = "employee1",
                            LockoutEnabled = false,
                            NormalizedEmail = "EMPLOYEE1@LOCALHOST.COM",
                            NormalizedUserName = "EMPLOYEE1@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEHOwszy19jPy4zG/FMKgsnwmlMUAWEMOSwCs3A9w74t9luNiMsExGjI0LvBYQ1EgQg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "f4ca39a7-c0fc-4283-9c9b-107fd9a9632e",
                            TwoFactorEnabled = false,
                            UserName = "employee1@localhost.com"
                        },
                        new
                        {
                            Id = "31dd98c5-71e1-4cbf-81a0-fd995bb0a735",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e05a7437-6ca2-4518-a0d2-fcc4166a2638",
                            Email = "employee2@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "Default",
                            LastName = "employee2",
                            LockoutEnabled = false,
                            NormalizedEmail = "EMPLOYEE2@LOCALHOST.COM",
                            NormalizedUserName = "EMPLOYEE2@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAECaSDhmBHHmROZOeZV3sreZgGEJ+a0K/QDPnF64+dh3Aj7BAu9zPK3h/TmM1+M4SnA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "fde17157-396a-4a5a-8cd7-9b1e6587e8d1",
                            TwoFactorEnabled = false,
                            UserName = "employee2@localhost.com"
                        },
                        new
                        {
                            Id = "c47afd54-3108-4ee6-8edc-a0bc2688ebba",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "c090e337-9551-4410-b1d1-0493f97e083e",
                            Email = "employee3@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "Default",
                            LastName = "employee3",
                            LockoutEnabled = false,
                            NormalizedEmail = "EMPLOYEE3@LOCALHOST.COM",
                            NormalizedUserName = "EMPLOYEE3@LOCALHOST.COM",
                            PasswordHash = "AQAAAAIAAYagAAAAEEnRzdXi+LhAXo3EZWl4EaoSfN41EXHVWE+MOEvuP5O0IW4AiR6p/A4S0j4AvW8Glw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "0bf7ee1b-24bb-4a59-a423-014e3b722ff1",
                            TwoFactorEnabled = false,
                            UserName = "employee3@localhost.com"
                        });
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Role Request Approved"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Role Request Rejected"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Added To Team"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Removed From Team"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Added As Project Leader"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Added To Task"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Added To Project"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Project Waiting For Approve"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Project Completion Approve"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Project Completion Rejected"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Task Waiting For Approve"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Task Completion Approve"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Task Completion Rejected"
                        });
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LeaderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ManagerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TeamId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeaderId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("TeamId");

                    b.HasIndex("WorkStatusId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.ProjectUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUsers");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.RequestStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("RequestStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Approved"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Declined"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Canceled"
                        });
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.RoleRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RequestStatusId")
                        .HasColumnType("int");

                    b.Property<string>("RequestedRole")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RequestStatusId");

                    b.HasIndex("UserId");

                    b.ToTable("RoleRequests");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.TaskItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AssignedUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubmissionText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("WorkStatusId");

                    b.ToTable("TaskItems");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Team", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ManagerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Teams");

                    b.HasData(
                        new
                        {
                            Id = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                            ManagerId = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                            Name = "Manager's Team"
                        });
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.WorkStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WorkStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Active"
                        },
                        new
                        {
                            Id = 2,
                            Name = "In Progress"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Completion Pending"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Completed"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Rejected"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "d61a13f6-088a-4250-a9e1-f55d3978ee1f",
                            Name = "Employee",
                            NormalizedName = "EMPLOYEE"
                        },
                        new
                        {
                            Id = "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f",
                            Name = "Manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                            RoleId = "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad"
                        },
                        new
                        {
                            UserId = "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                            RoleId = "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f"
                        },
                        new
                        {
                            UserId = "56d078c0-f671-4a22-b3fd-977f2ac33eae",
                            RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f"
                        },
                        new
                        {
                            UserId = "31dd98c5-71e1-4cbf-81a0-fd995bb0a735",
                            RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f"
                        },
                        new
                        {
                            UserId = "c47afd54-3108-4ee6-8edc-a0bc2688ebba",
                            RoleId = "d61a13f6-088a-4250-a9e1-f55d3978ee1f"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.ApplicationUser", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.Team", "Team")
                        .WithMany("Members")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Notification", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.NotificationType", "NotificationType")
                        .WithMany()
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NotificationType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Project", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "Leader")
                        .WithMany()
                        .HasForeignKey("LeaderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.WorkStatus", "WorkStatus")
                        .WithMany()
                        .HasForeignKey("WorkStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Leader");

                    b.Navigation("Manager");

                    b.Navigation("Team");

                    b.Navigation("WorkStatus");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.ProjectUser", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.Project", "Project")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "User")
                        .WithMany("ProjectUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.RoleRequest", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.RequestStatus", "RequestStatus")
                        .WithMany()
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequestStatus");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.TaskItem", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "AssignedUser")
                        .WithMany()
                        .HasForeignKey("AssignedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId");

                    b.HasOne("CompanyTaskManager.Data.Models.WorkStatus", "WorkStatus")
                        .WithMany()
                        .HasForeignKey("WorkStatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AssignedUser");

                    b.Navigation("Project");

                    b.Navigation("WorkStatus");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Team", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CompanyTaskManager.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.ApplicationUser", b =>
                {
                    b.Navigation("Notifications");

                    b.Navigation("ProjectUsers");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Project", b =>
                {
                    b.Navigation("ProjectUsers");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("CompanyTaskManager.Data.Models.Team", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
