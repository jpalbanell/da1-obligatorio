using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorios.Migrations
{
    /// <inheritdoc />
    public partial class PersistirFixture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemillaFixture = table.Column<int>(type: "int", nullable: false),
                    FechaInicioTorneo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxPartidosPorDia = table.Column<int>(type: "int", nullable: false),
                    SeparacionEntreFechas = table.Column<int>(type: "int", nullable: false),
                    EstaGenerado = table.Column<bool>(type: "bit", nullable: false),
                    CrucesGenerados = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixtureEquipos",
                columns: table => new
                {
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    EquipoNombre = table.Column<string>(type: "nvarchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureEquipos", x => new { x.FixtureId, x.EquipoNombre });
                    table.ForeignKey(
                        name: "FK_FixtureEquipos_Equipos_EquipoNombre",
                        column: x => x.EquipoNombre,
                        principalTable: "Equipos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixtureEquipos_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixtureEstadios",
                columns: table => new
                {
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    EstadioNombre = table.Column<string>(type: "nvarchar(80)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixtureEstadios", x => new { x.FixtureId, x.EstadioNombre });
                    table.ForeignKey(
                        name: "FK_FixtureEstadios_Estadios_EstadioNombre",
                        column: x => x.EstadioNombre,
                        principalTable: "Estadios",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixtureEstadios_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixtureEquipos_EquipoNombre",
                table: "FixtureEquipos",
                column: "EquipoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_FixtureEstadios_EstadioNombre",
                table: "FixtureEstadios",
                column: "EstadioNombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixtureEquipos");

            migrationBuilder.DropTable(
                name: "FixtureEstadios");

            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
