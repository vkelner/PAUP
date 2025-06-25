using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paup.Migrations
{
    /// <inheritdoc />
    public partial class DodajZupanijeCSV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zupanija",
                table: "PruzateljiUsluga",
                newName: "ZupanijeCSV");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZupanijeCSV",
                table: "PruzateljiUsluga",
                newName: "Zupanija");
        }
    }
}
