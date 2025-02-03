using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class NullableSubmissionText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionText",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6823557d-5944-4443-bd4a-928366b87306", "AQAAAAIAAYagAAAAEFsi/qGKQuujRWdUIf3hGcPdHm9zUVT0nDoH6Dplh7N++45rz6g2zDXSkVdO/z/RFw==", "916cd898-b8df-4fb3-913e-23c258ff9a48" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionText",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d66bc45f-bc57-49a7-8db3-73ad2088547a", "AQAAAAIAAYagAAAAEOL2VmnE1XFRK6YfaqKZNRQwEvkzEI68qYK4erg+bwB6x9RjCbIRPcuEbL+Zzf21Ug==", "15640a0d-3a7e-41b2-a7cc-32532de6ac18" });
        }
    }
}
