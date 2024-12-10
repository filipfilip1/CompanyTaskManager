using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationType",
                newName: "NotificationTypes");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_NotificationTypeId",
                table: "Notifications",
                newName: "IX_Notifications_NotificationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "65cec066-4169-45d9-8824-43ae1fbaf5af", "AQAAAAIAAYagAAAAEPChsa03YpbjTnEaynDHpsQswJkeAr1O7wD5m1GWXFeh3JHp7jZxliRP8cw06RReCg==", "c5412e09-3fbd-4c6c-9d24-fbc6e0a554c0" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationTypes",
                table: "NotificationTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "NotificationTypes",
                newName: "NotificationType");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notification",
                newName: "IX_Notification_NotificationTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationType",
                table: "NotificationType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "329afa76-c6c6-4c37-accf-9e93e8b6b7fe", "AQAAAAIAAYagAAAAEMsnKsS+p7YiVZ3b4hnrgKuVWY15dXOEJsfvsHdI8ObpwW7d6dBOK0Z6d8MO6nTvjw==", "e3e9b8f2-97a6-4aae-b5ed-cceb47fc16c7" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_AspNetUsers_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationType_NotificationTypeId",
                table: "Notification",
                column: "NotificationTypeId",
                principalTable: "NotificationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
