using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyTaskManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "31dd98c5-71e1-4cbf-81a0-fd995bb0a735",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e05a7437-6ca2-4518-a0d2-fcc4166a2638", "AQAAAAIAAYagAAAAECaSDhmBHHmROZOeZV3sreZgGEJ+a0K/QDPnF64+dh3Aj7BAu9zPK3h/TmM1+M4SnA==", "fde17157-396a-4a5a-8cd7-9b1e6587e8d1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba80bf0a-962b-45a9-8503-069e0c8c32af", "AQAAAAIAAYagAAAAEO/7mdlwuEqgkghm4VTYnn+X28MXFk0cxeKclN0+xd6zCgxVgwJnqVNYI2G50p+yZQ==", "7e99279e-286d-4594-b01f-1e143fb6efc7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56d078c0-f671-4a22-b3fd-977f2ac33eae",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "15167dc4-5aa2-4538-bc82-8199b5e2edde", "AQAAAAIAAYagAAAAEHOwszy19jPy4zG/FMKgsnwmlMUAWEMOSwCs3A9w74t9luNiMsExGjI0LvBYQ1EgQg==", "f4ca39a7-c0fc-4283-9c9b-107fd9a9632e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f70bae9b-5690-421f-9ae2-ebd55a017bbd", "AQAAAAIAAYagAAAAEMKCurknNjoi11r/VPWM7H0OQ3qGriT0ZGXcLr9ubeRVOOpa9nKWgn0uGRPFld/fTg==", "fbd3afd7-4957-426b-8aa3-fe913458b2e5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c47afd54-3108-4ee6-8edc-a0bc2688ebba",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c090e337-9551-4410-b1d1-0493f97e083e", "AQAAAAIAAYagAAAAEEnRzdXi+LhAXo3EZWl4EaoSfN41EXHVWE+MOEvuP5O0IW4AiR6p/A4S0j4AvW8Glw==", "0bf7ee1b-24bb-4a59-a423-014e3b722ff1" });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "ManagerId", "Name" },
                values: new object[] { "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb", "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb", "Manager's Team" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "31dd98c5-71e1-4cbf-81a0-fd995bb0a735",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "98d12077-85f4-4b5a-81f7-fc4c9edce568", "AQAAAAIAAYagAAAAENukYiJXvSFCoOikXGz+pDr8Sj2dLVSCcj6ZSxgrpKxAQOxUuRZxg3XZqwji5csfKQ==", "9f7f4d0e-e022-48ef-ad71-8dec930a22d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4b0b2aeb-474e-45f2-8899-e4a1536a52bf",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32694db8-ab14-43e7-8599-836deef4a2e8", "AQAAAAIAAYagAAAAEB/cXlE8nNB9CkHoRJXNrEcnYiOI0GdBUemhb795iMk2Jd4KJ+uQv9kt/1LEQGX5JA==", "ac9631ff-da6c-4683-9a36-ec93969079f8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "56d078c0-f671-4a22-b3fd-977f2ac33eae",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45699219-7f91-4f73-95d3-cfecd4066ccd", "AQAAAAIAAYagAAAAEPyff1visD/D/mH0x0a2c151+C2pkwFnAs3G6oK8yCK9K8/Ovz539PS8VzOGhso4Xg==", "213e5a7b-3437-456f-a22f-4fc5d18c9795" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "66d7ea08-73a8-44a1-84a1-3d41d7c16ecb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0da48a21-3642-4c6f-a4b8-9174ff18e7da", "AQAAAAIAAYagAAAAEMXLOeLtHBxO0fRNOHx42Ivo7og+xeGUkrO2NA69mTLPOjCmuuqF392rHiahE55Iog==", "6075e293-867a-40d6-9f93-d1ddc08a9fc2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c47afd54-3108-4ee6-8edc-a0bc2688ebba",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "627cccdc-d8ea-4881-9fb6-09eb91670c58", "AQAAAAIAAYagAAAAEAsGEXlZuwgkF3iK9BxTL/yd4GWnQW31aPzIcgiJ22XAZ/dm5/NfGw36e//5JvppHQ==", "115c65b6-130b-48b2-846c-bbd45c086ad1" });
        }
    }
}
