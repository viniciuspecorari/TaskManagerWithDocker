using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagerWithDocker.Migrations
{
    /// <inheritdoc />
    public partial class RoleConfiguration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c3bc6492-4b78-45f4-a555-b728abfdeccc", null, "Administrator", "ADMINISTRATOR" },
                    { "d45f731c-4294-4295-8bbc-6adc95dbb186", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3bc6492-4b78-45f4-a555-b728abfdeccc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d45f731c-4294-4295-8bbc-6adc95dbb186");
        }
    }
}
