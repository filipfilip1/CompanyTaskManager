using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeededAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad", null, "Administrator", "ADMINISTRATOR" },
                    { "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f", null, "Manager", "MANAGER" },
                    { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", null, "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4b0b2aeb-474e-45f2-8899-e4a1536a52bf", 0, "778a4834-609e-4073-875f-dec5c88ace07", "admin@localhost.com", true, "Default", "Admin", false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEEmU5faZ5tzzdhkSP7CsmFtjmieCXdihNH0b5qDrurauS1DZmt6ki9vem8VbQ6rizg==", null, false, "71c2dbdb-8297-4d03-aaca-117f9159548b", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad", "4b0b2aeb-474e-45f2-8899-e4a1536a52bf" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d61a13f6-088a-4250-a9e1-f55d3978ee1f");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad", "4b0b2aeb-474e-45f2-8899-e4a1536a52bf" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
