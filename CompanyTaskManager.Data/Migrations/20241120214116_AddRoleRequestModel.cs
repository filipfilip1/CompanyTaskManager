using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestedRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleRequests_RequestStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10eb1e00-0371-47ef-bc8c-3f126ca37e74", "AQAAAAIAAYagAAAAEMCl/yfiAaNrIDzpOg11wq63yni6ZrzNQoI6Zykt4rtjycjxD3LzMAHY94m60M7Wlw==", "614d92b8-04a7-41da-86ef-8437ad65d9e2" });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Declined" },
                    { 4, "Canceled" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequests_RequestStatusId",
                table: "RoleRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleRequests_UserId",
                table: "RoleRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleRequests");

            migrationBuilder.DropTable(
                name: "RequestStatuses");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "778a4834-609e-4073-875f-dec5c88ace07", "AQAAAAIAAYagAAAAEEmU5faZ5tzzdhkSP7CsmFtjmieCXdihNH0b5qDrurauS1DZmt6ki9vem8VbQ6rizg==", "71c2dbdb-8297-4d03-aaca-117f9159548b" });
        }
    }
}
