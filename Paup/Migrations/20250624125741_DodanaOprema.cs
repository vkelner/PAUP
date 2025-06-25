using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Paup.Migrations
{
    /// <inheritdoc />
    public partial class DodanaOprema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Opreme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Opis = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    Mjera = table.Column<int>(type: "int", nullable: false),
                    VrstaPonude = table.Column<int>(type: "int", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    NajamPo = table.Column<int>(type: "int", nullable: true),
                    CijenaNajma = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PruzateljUslugeId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opreme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opreme_AspNetUsers_PruzateljUslugeId",
                        column: x => x.PruzateljUslugeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Opreme_PruzateljUslugeId",
                table: "Opreme",
                column: "PruzateljUslugeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Opreme");
        }
    }
}
