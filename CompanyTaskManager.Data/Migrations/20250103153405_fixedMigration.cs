using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2cec480-3f6f-4cd1-9752-bafd9f60ca72", "AQAAAAIAAYagAAAAECM/oyvpf0c8UhzeqKMg6AhhuAKtA5Av5TiGZwIjnZsGciFvWpF3n0Mfm67cPsfGTg==", "299832d9-ef07-41fc-b055-9585aa6a5e33" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0414581e-615c-46d2-b196-a30111135f83", "AQAAAAIAAYagAAAAEHJKXZkV77GZAEIhBBs2Q+ykkV7yeNqLtyhVLN9N/vr7qF56WijkDVABsUePKNovqg==", "ad228f87-f9f9-4e31-8042-e741c0798a18" });
        }
    }
}
