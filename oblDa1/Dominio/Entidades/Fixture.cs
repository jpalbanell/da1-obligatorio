namespace Dominio.Entidades
{
    public class Fixture
    {
        public int SemillaFixture { get; set; }
        public DateTime FechaInicioTorneo { get; set; } = new DateTime(2026, 6, 1);
        public int MaxPartidosPorDia { get; set; } = 3;
        public int SeparacionEntreFechas { get; set; } = 3;
        public bool EstaGenerado { get; set; } = false;
        public bool CrucesGenerados { get; set; } = false;
        public List<Equipo> Equipos { get; private set; } = new List<Equipo>();

        public void AgregarEquipo(Equipo equipo)
        {
            if (Equipos.Any(e => e.Nombre == equipo.Nombre))
                throw new InvalidOperationException("Ya existe un equipo con ese nombre.");
            Equipos.Add(equipo);
        }
    }
}