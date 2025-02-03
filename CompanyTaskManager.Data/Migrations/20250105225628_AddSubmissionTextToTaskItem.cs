using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionTextToTaskItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmissionText",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d66bc45f-bc57-49a7-8db3-73ad2088547a", "AQAAAAIAAYagAAAAEOL2VmnE1XFRK6YfaqKZNRQwEvkzEI68qYK4erg+bwB6x9RjCbIRPcuEbL+Zzf21Ug==", "15640a0d-3a7e-41b2-a7cc-32532de6ac18" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionText",
                table: "TaskItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2cec480-3f6f-4cd1-9752-bafd9f60ca72", "AQAAAAIAAYagAAAAECM/oyvpf0c8UhzeqKMg6AhhuAKtA5Av5TiGZwIjnZsGciFvWpF3n0Mfm67cPsfGTg==", "299832d9-ef07-41fc-b055-9585aa6a5e33" });
        }
    }
}
