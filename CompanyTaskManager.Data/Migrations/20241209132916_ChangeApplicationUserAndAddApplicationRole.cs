using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeApplicationUserAndAddApplicationRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_ManagerId",
                table: "Teams");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d61a13f6-088a-4250-a9e1-f55d3978ee1f");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ApplicationRoleApplicationUser",
                columns: table => new
                {
                    RolesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleApplicationUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationRoleApplicationUser_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleApplicationUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7c61332b-b09f-46c4-9338-ecb2c1b3f9ad", null, "IdentityRole", "Administrator", "ADMINISTRATOR" },
                    { "b4d4dc55-3afc-40bf-adf8-7f1b24ea055f", null, "IdentityRole", "Manager", "MANAGER" },
                    { "d61a13f6-088a-4250-a9e1-f55d3978ee1f", null, "IdentityRole", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f416e116-e0ee-4701-b9cf-f734d7d5176b", "AQAAAAIAAYagAAAAEAgLUNQCEhfxT+VkasSeCYhR9oVcUEBY58QGJrUa/8gfPorhQEWriUFIttCbGD5IIQ==", "84c68395-9ca6-403e-be37-aab74fab7a52" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleApplicationUser_UsersId",
                table: "ApplicationRoleApplicationUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_ManagerId",
                table: "Teams",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_ManagerId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "ApplicationRoleApplicationUser");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1df705f0-fd0b-4758-abd0-acb94fae6a5a", "AQAAAAIAAYagAAAAEAANr1Hg7KX2ft1cYyvwLgNL/EhGx54wtryOrGEccBYbksISi8cxGjzaWNfOa5ytrg==", "aa587995-df35-461b-8168-229685a7648c" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teams_TeamId",
                table: "AspNetUsers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_ManagerId",
                table: "Teams",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
