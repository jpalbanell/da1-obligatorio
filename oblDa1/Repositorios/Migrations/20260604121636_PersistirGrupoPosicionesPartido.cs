using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorios.Migrations
{
    /// <inheritdoc />
    public partial class PersistirGrupoPosicionesPartido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Etiqueta = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrigenLocalId = table.Column<int>(type: "int", nullable: true),
                    OrigenVisitanteId = table.Column<int>(type: "int", nullable: true),
                    EstaBloqueado = table.Column<bool>(type: "bit", nullable: false),
                    TieneResultado = table.Column<bool>(type: "bit", nullable: false),
                    EsPorPerdedor = table.Column<bool>(type: "bit", nullable: false),
                    GolesLocalAnterior = table.Column<int>(type: "int", nullable: false),
                    GolesVisitanteAnterior = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fase = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EquipoLocalNombre = table.Column<string>(type: "nvarchar(60)", nullable: true),
                    EquipoVisitanteNombre = table.Column<string>(type: "nvarchar(60)", nullable: true),
                    EstadioNombre = table.Column<string>(type: "nvarchar(80)", nullable: true),
                    GrupoId = table.Column<int>(type: "int", nullable: true),
                    GolesLocal = table.Column<int>(type: "int", nullable: false),
                    GolesVisitante = table.Column<int>(type: "int", nullable: false),
                    VencedorNombre = table.Column<string>(type: "nvarchar(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partidos_Equipos_EquipoLocalNombre",
                        column: x => x.EquipoLocalNombre,
                        principalTable: "Equipos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Equipos_EquipoVisitanteNombre",
                        column: x => x.EquipoVisitanteNombre,
                        principalTable: "Equipos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Equipos_VencedorNombre",
                        column: x => x.VencedorNombre,
                        principalTable: "Equipos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Estadios_EstadioNombre",
                        column: x => x.EstadioNombre,
                        principalTable: "Estadios",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Partidos_OrigenLocalId",
                        column: x => x.OrigenLocalId,
                        principalTable: "Partidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partidos_Partidos_OrigenVisitanteId",
                        column: x => x.OrigenVisitanteId,
                        principalTable: "Partidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PosicionesGrupo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiferenciaGoles = table.Column<int>(type: "int", nullable: false),
                    EquipoNombre = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: false),
                    GolesFavor = table.Column<int>(type: "int", nullable: false),
                    GolesContra = table.Column<int>(type: "int", nullable: false),
                    PosicionFinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosicionesGrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PosicionesGrupo_Equipos_EquipoNombre",
                        column: x => x.EquipoNombre,
                        principalTable: "Equipos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PosicionesGrupo_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoLocalNombre",
                table: "Partidos",
                column: "EquipoLocalNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EquipoVisitanteNombre",
                table: "Partidos",
                column: "EquipoVisitanteNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_EstadioNombre",
                table: "Partidos",
                column: "EstadioNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_GrupoId",
                table: "Partidos",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_OrigenLocalId",
                table: "Partidos",
                column: "OrigenLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_OrigenVisitanteId",
                table: "Partidos",
                column: "OrigenVisitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Partidos_VencedorNombre",
                table: "Partidos",
                column: "VencedorNombre");

            migrationBuilder.CreateIndex(
                name: "IX_PosicionesGrupo_EquipoNombre",
                table: "PosicionesGrupo",
                column: "EquipoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_PosicionesGrupo_GrupoId",
                table: "PosicionesGrupo",
                column: "GrupoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partidos");

            migrationBuilder.DropTable(
                name: "PosicionesGrupo");

            migrationBuilder.DropTable(
                name: "Grupos");
        }
    }
}
