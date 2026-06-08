using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorios.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIncidenciasYRankingPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankingLocalAntes",
                table: "Partidos",
                type: "int",
                nullable: false,
                defaultValue: -1);

            migrationBuilder.AddColumn<int>(
                name: "RankingVisitanteAntes",
                table: "Partidos",
                type: "int",
                nullable: false,
                defaultValue: -1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankingLocalAntes",
                table: "Partidos");

            migrationBuilder.DropColumn(
                name: "RankingVisitanteAntes",
                table: "Partidos");
        }
    }
}
