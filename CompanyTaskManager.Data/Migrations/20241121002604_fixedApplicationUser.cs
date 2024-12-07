using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9db8dd21-6b70-47b1-86f9-b602d5c08ffe", "AQAAAAIAAYagAAAAEMDvqqO4Cf7hZoHf9L7dB7fKTga5Lp/vwv82sEOwYS2EaR/vKy3UjH8zWCmQ7Yv5xw==", "a67a1a73-6f5d-46eb-8d77-62a28486540a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10eb1e00-0371-47ef-bc8c-3f126ca37e74", "AQAAAAIAAYagAAAAEMCl/yfiAaNrIDzpOg11wq63yni6ZrzNQoI6Zykt4rtjycjxD3LzMAHY94m60M7Wlw==", "614d92b8-04a7-41da-86ef-8437ad65d9e2" });
        }
    }
}
