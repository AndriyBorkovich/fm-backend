using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballManager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialRolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "652a3ac6-ee92-4445-b92a-77e2e0efb76b", null, "IdentityRole", "Player", "PLAYER" },
                    { "8fba81b0-336d-4fe8-8734-bb45b24fa580", null, "IdentityRole", "Admin", "ADMIN" },
                    { "b7217859-a86d-445b-a15c-59c9a0e9cac2", null, "IdentityRole", "Coach", "COACH" }
                });

            // migrationBuilder.Sql(@"INSERT INTO AspNetUserRoles
            //                        VALUES('2a0fe5a3-21a9-4b1d-959e-8cf2f9927412', '8fba81b0-336d-4fe8-8734-bb45b24fa580')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");
        }
    }
}
