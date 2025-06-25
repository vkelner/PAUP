using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paup.Migrations
{
    /// <inheritdoc />
    public partial class AddZupanijeToAppKorisnik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zupanija",
                table: "AspNetUsers",
                newName: "Zupanije");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zupanije",
                table: "AspNetUsers",
                newName: "Zupanija");
        }
    }
}
