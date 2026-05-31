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
    
            var cantidadConfederacion = Equipos.Count(e => e.Confederacion == equipo.Confederacion);
            if (cantidadConfederacion >= equipo.Confederacion.CupoMaximo())
                throw new InvalidOperationException($"El cupo de {equipo.Confederacion} está completo.");
    
            Equipos.Add(equipo);
        }
        
        public void EliminarEquipo(string nombre)
        {
            var equipo = Equipos.FirstOrDefault(e => e.Nombre == nombre);
            if (equipo == null)
                throw new KeyNotFoundException($"No existe un equipo con el nombre {nombre}.");
            Equipos.Remove(equipo);
        }
        
        public void EditarEquipo(Equipo equipo, string nombreOriginal)
        {
            var equipoExistente = Equipos.FirstOrDefault(e => e.Nombre == nombreOriginal);
            if (equipoExistente == null)
                throw new KeyNotFoundException($"No existe un equipo con el nombre {nombreOriginal}.");
            if (equipo.Nombre != nombreOriginal && Equipos.Any(e => e.Nombre == equipo.Nombre))
                throw new InvalidOperationException("Ya existe un equipo con ese nombre.");
            var cantidadConfederacion = Equipos.Count(e => e.Confederacion == equipo.Confederacion && e.Nombre != nombreOriginal);
            if (cantidadConfederacion >= equipo.Confederacion.CupoMaximo())
                throw new InvalidOperationException($"El cupo de {equipo.Confederacion} está completo.");
            var indice = Equipos.IndexOf(equipoExistente);
            Equipos[indice] = equipo;
        }
    }
}