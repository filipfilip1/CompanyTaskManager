using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedNotifcationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "634f3507-8a26-44bb-977b-39fc84db2d7a", "AQAAAAIAAYagAAAAEIwEowfTezIiOD8uUPFl4VfxDW3sUmM6jrzy7uUyyiNZ51xWI9/M6U/EDppYCYZBHQ==", "d041a523-dcfb-430c-93d0-5b3231a994ee" });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Added To Team" },
                    { 4, "Removed From Team" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ca6c813-a038-4788-96c1-0be9ab77022f", "AQAAAAIAAYagAAAAEDmjrHURWfBm+t+Et0gXcCj+VecMn14UJZHVffoUEJDQCvuVZtBAzcPdtp/bUycwyQ==", "6241e042-12bc-41cc-a4b0-55ac557b0484" });
        }
    }
}
