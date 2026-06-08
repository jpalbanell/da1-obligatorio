using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositorios.Migrations
{
    /// <inheritdoc />
    public partial class AddNombreMotorSimulacionToFixture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreMotorSimulacion",
                table: "Fixtures",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreMotorSimulacion",
                table: "Fixtures");
        }
    }
}
