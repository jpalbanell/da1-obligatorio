using Dominio.Entidades;

namespace Web.DTOs
{
    public class GenerarFixtureRequest
    {
        public int SemillaFixture { get; set; } = 42;
        public string FechaInicioTorneo { get; set; } = "2026-06-01";
        public int MaxPartidosPorDia { get; set; } = 3;
        public int SeparacionEntreFechas { get; set; } = 3;
        public string NombreMotor { get; set; } = "Probabilístico";

        public Fixture ToEntity()
        {
            if (!DateTime.TryParse(FechaInicioTorneo, out var fecha))
                throw new ArgumentException("La fecha de inicio debe tener el formato AAAA-MM-DD. Ejemplo: 2026-06-01");

            var fixture = new Fixture();
            fixture.SemillaFixture = SemillaFixture;
            fixture.FechaInicioTorneo = fecha;
            fixture.MaxPartidosPorDia = MaxPartidosPorDia;
            fixture.SeparacionEntreFechas = SeparacionEntreFechas;
            fixture.NombreMotorSimulacion = NombreMotor;
            return fixture;
        }
    }
}