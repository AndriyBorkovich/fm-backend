using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChampionshipNameAndCountryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Championships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Championships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4fefe718-8fa0-415b-a770-7b793ddcf939", null, "IdentityRole", "Coach", "COACH" },
                    { "51d66239-d4b3-4137-8eeb-3f7b6015230f", null, "IdentityRole", "Player", "PLAYER" },
                    { "8aa57ec9-2c99-4eb3-8c3b-762a17a4bcb7", null, "IdentityRole", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fefe718-8fa0-415b-a770-7b793ddcf939");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51d66239-d4b3-4137-8eeb-3f7b6015230f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8aa57ec9-2c99-4eb3-8c3b-762a17a4bcb7");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Championships");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Championships");

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
    }
}
