using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedNewUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32694db8-ab14-43e7-8599-836deef4a2e8", "AQAAAAIAAYagAAAAEB/cXlE8nNB9CkHoRJXNrEcnYiOI0GdBUemhb795iMk2Jd4KJ+uQv9kt/1LEQGX5JA==", "ac9631ff-da6c-4683-9a36-ec93969079f8" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TeamId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "31dd98c5-71e1-4cbf-81a0-fd995bb0a735", 0, "98d12077-85f4-4b5a-81f7-fc4c9edce568", "employee2@localhost.com", true, "Default", "employee2", false, null, "EMPLOYEE2@LOCALHOST.COM", "EMPLOYEE2@LOCALHOST.COM", "AQAAAAIAAYagAAAAENukYiJXvSFCoOikXGz+pDr8Sj2dLVSCcj6ZSxgrpKxAQOxUuRZxg3XZqwji5csfKQ==", null, false, "9f7f4d0e-e022-48ef-ad71-8dec930a22d9", null, false, "employee2@localhost.com" },
                    { "56d078c0-f671-4a22-b3fd-977f2ac33eae", 0, "45699219-7f91-4f73-95d3-cfecd4066ccd", "employee1@localhost.com", true, "Default", "employee1", false, null, "EMPLOYEE1@LOCALHOST.COM", "EMPLOYEE1@LOCALHOST.COM", "AQAAAAIAAYagAAAAEPyff1visD/D/mH0x0a2c151+C2pkwFnAs3G6oK8yCK9K8/Ovz539PS8VzOGhso4Xg==", null, false, "213e5a7b-3437-456f-a22f-4fc5d18c9795", null, false, "employee1@localhost.com" },
                    { "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb", 0, "0da48a21-3642-4c6f-a4b8-9174ff18e7da", "manager@localhost.com", true, "Default", "Manager", false, null, "MANAGER@LOCALHOST.COM", "MANAGER@LOCALHOST.COM", "AQAAAAIAAYagAAAAEMXLOeLtHBxO0fRNOHx42Ivo7og+xeGUkrO2NA69mTLPOjCmuuqF392rHiahE55Iog==", null, false, "6075e293-867a-40d6-9f93-d1ddc08a9fc2", null, false, "manager@localhost.com" },
                    { "c47afd54-3108-4ee6-8edc-a0bc2688ebba", 0, "627cccdc-d8ea-4881-9fb6-09eb91670c58", "employee3@localhost.com", true, "Default", "employee3", false, null, "EMPLOYEE3@LOCALHOST.COM", "EMPLOYEE3@LOCALHOST.COM", "AQAAAAIAAYagAAAAEAsGEXlZuwgkF3iK9BxTL/yd4GWnQW31aPzIcgiJ22XAZ/dm5/NfGw36e//5JvppHQ==", null, false, "115c65b6-130b-48b2-846c-bbd45c086ad1", null, false, "employee3@localhost.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "31dd98c5-71e1-4cbf-81a0-fd995bb0a735" },
                    { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "56d078c0-f671-4a22-b3fd-977f2ac33eae" },
                    { "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f", "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb" },
                    { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "c47afd54-3108-4ee6-8edc-a0bc2688ebba" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "31dd98c5-71e1-4cbf-81a0-fd995bb0a735" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "56d078c0-f671-4a22-b3fd-977f2ac33eae" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f", "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", "c47afd54-3108-4ee6-8edc-a0bc2688ebba" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "31dd98c5-71e1-4cbf-81a0-fd995bb0a735");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56d078c0-f671-4a22-b3fd-977f2ac33eae");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c47afd54-3108-4ee6-8edc-a0bc2688ebba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6823557d-5944-4443-bd4a-928366b87306", "AQAAAAIAAYagAAAAEFsi/qGKQuujRWdUIf3hGcPdHm9zUVT0nDoH6Dplh7N++45rz6g2zDXSkVdO/z/RFw==", "916cd898-b8df-4fb3-913e-23c258ff9a48" });
        }
    }
}
