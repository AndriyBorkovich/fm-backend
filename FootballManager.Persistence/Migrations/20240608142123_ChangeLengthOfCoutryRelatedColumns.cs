using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLengthOfCoutryRelatedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Players",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Coaches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Championships",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b3a410d-8f0e-4d6c-aecf-91005e712c1a", null, "IdentityRole", "Coach", "COACH" },
                    { "9660dfbf-c00e-41b3-acc9-6c304a325069", null, "IdentityRole", "Admin", "ADMIN" },
                    { "986e274b-ade2-474f-a74c-ebdfce71bffa", null, "IdentityRole", "Player", "PLAYER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b3a410d-8f0e-4d6c-aecf-91005e712c1a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9660dfbf-c00e-41b3-acc9-6c304a325069");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "986e274b-ade2-474f-a74c-ebdfce71bffa");

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Players",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Coaches",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "Championships",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
    }
}
