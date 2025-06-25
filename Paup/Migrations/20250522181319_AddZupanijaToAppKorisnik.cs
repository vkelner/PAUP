using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paup.Migrations
{
    /// <inheritdoc />
    public partial class AddZupanijaToAppKorisnik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Zupanija",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zupanija",
                table: "AspNetUsers");
        }
    }
}
