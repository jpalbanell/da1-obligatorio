namespace Dominio.Entidades
{
    public class Fixture
    {
        public int SemillaFixture { get; set; }
        public DateTime FechaInicioTorneo { get; set; } = new DateTime(2026, 6, 1);
        public int MaxPartidosPorDia { get; set; } = 3;
    }
}