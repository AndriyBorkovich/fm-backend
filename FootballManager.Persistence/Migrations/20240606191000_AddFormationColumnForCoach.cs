using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFormationColumnForCoach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "652a3ac6-ee92-4445-b92a-77e2e0efb76b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fba81b0-336d-4fe8-8734-bb45b24fa580");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7217859-a86d-445b-a15c-59c9a0e9cac2");

            migrationBuilder.AddColumn<int>(
                name: "PreferredFormation",
                table: "Coaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01a073b7-9059-485c-b3f4-40bde9f79068", null, "IdentityRole", "Coach", "COACH" },
                    { "621e0610-5778-4195-b19e-8e2b467c614f", null, "IdentityRole", "Admin", "ADMIN" },
                    { "93565879-3b57-4f5c-80e6-073e2adb62ae", null, "IdentityRole", "Player", "PLAYER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01a073b7-9059-485c-b3f4-40bde9f79068");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "621e0610-5778-4195-b19e-8e2b467c614f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93565879-3b57-4f5c-80e6-073e2adb62ae");

            migrationBuilder.DropColumn(
                name: "PreferredFormation",
                table: "Coaches");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "652a3ac6-ee92-4445-b92a-77e2e0efb76b", null, "IdentityRole", "Player", "PLAYER" },
                    { "8fba81b0-336d-4fe8-8734-bb45b24fa580", null, "IdentityRole", "Admin", "ADMIN" },
                    { "b7217859-a86d-445b-a15c-59c9a0e9cac2", null, "IdentityRole", "Coach", "COACH" }
                });
        }
    }
}
