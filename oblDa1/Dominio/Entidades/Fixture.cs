namespace Dominio.Entidades
{
    public class Fixture
    {
        public int Id { get; set; }
        public int SemillaFixture { get; set; }
        public DateTime FechaInicioTorneo { get; set; } = new DateTime(2026, 6, 1);
        public int MaxPartidosPorDia { get; set; } = 3;
        public int SeparacionEntreFechas { get; set; } = 3;
        public bool EstaGenerado { get; set; } = false;
        public bool CrucesGenerados { get; set; } = false;

        private string _nombreMotorSimulacion = "Probabilístico";
        public string NombreMotorSimulacion
        {
            get => _nombreMotorSimulacion;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre del motor de simulación no puede estar vacío.");
                _nombreMotorSimulacion = value;
            }
        }
        private List<Equipo> _equipos = new List<Equipo>();
        private List<Estadio> _estadios = new List<Estadio>();

        public virtual List<Equipo> Equipos => _equipos;
        public virtual List<Estadio> Estadios => _estadios;

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
            equipoExistente.Confederacion = equipo.Confederacion;
            equipoExistente.RankingFifa = equipo.RankingFifa;
            equipoExistente.Bandera = equipo.Bandera;
            if (equipoExistente.Nombre != equipo.Nombre)
                equipoExistente.Nombre = equipo.Nombre;
        }
        
        public void HidratarEquipos(IEnumerable<Equipo> equipos)
        {
            foreach (var equipo in equipos)
                if (!_equipos.Any(e => e.Nombre == equipo.Nombre))
                    _equipos.Add(equipo);
        }

        public void AgregarEstadio(Estadio estadio)
        {
            if (Estadios.Any(e => e.Nombre == estadio.Nombre))
                throw new InvalidOperationException("Ya existe un estadio con ese nombre.");
            Estadios.Add(estadio);
        }
        
        public void EliminarEstadio(string nombre)
        {
            var estadio = Estadios.FirstOrDefault(e => e.Nombre == nombre);
            if (estadio == null)
                throw new KeyNotFoundException($"No existe un estadio con el nombre {nombre}.");
            Estadios.Remove(estadio);
        }
        
        public bool PuedeGenerarse()
        {
            return Equipos.Count == 48 && Estadios.Count >= 4;
        }
        
        public void AsignarFechasAPartidos(List<List<Partido>> partidosPorGrupo)
        {
            var partidosPorDia = new Dictionary<DateTime, int>();
            var ultimoPartidoPorEquipo = new Dictionary<string, DateTime>();

            AsignarFechasJornadasPrevias(partidosPorGrupo, partidosPorDia, ultimoPartidoPorEquipo);
            AsignarFechasUltimaJornada(partidosPorGrupo, partidosPorDia, ultimoPartidoPorEquipo);
        }
        
        private void AsignarFechasJornadasPrevias(
            List<List<Partido>> todosLosPartidos,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            var partidosJornadasPrevias = new List<Partido>();
            foreach (var partidosGrupo in todosLosPartidos)
            {
                partidosJornadasPrevias.Add(partidosGrupo[0]);
                partidosJornadasPrevias.Add(partidosGrupo[1]);
                partidosJornadasPrevias.Add(partidosGrupo[2]);
                partidosJornadasPrevias.Add(partidosGrupo[3]);
            }

            var fechaActual = FechaInicioTorneo.Date;
            foreach (var partido in partidosJornadasPrevias)
            {
                fechaActual = BuscarFechaDisponible(fechaActual, partido, partidosPorDia, ultimoPartidoPorEquipo);
                int turno = partidosPorDia.ContainsKey(fechaActual) ? partidosPorDia[fechaActual] : 0;
                partido.Fecha = fechaActual.AddHours(14 + (turno * 4));
                ActualizarContadores(fechaActual, partido, partidosPorDia, ultimoPartidoPorEquipo);
            }
        }
        
        private void AsignarFechasUltimaJornada(
            List<List<Partido>> todosLosPartidos,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            foreach (var partidosGrupo in todosLosPartidos)
            {
                var partidoA = partidosGrupo[4];
                var partidoB = partidosGrupo[5];
                var fechaActual = FechaInicioTorneo.Date;

                while (true)
                {
                    if (!partidosPorDia.ContainsKey(fechaActual))
                        partidosPorDia[fechaActual] = 0;

                    bool caben = partidosPorDia[fechaActual] + 2 <= MaxPartidosPorDia;
                    bool equiposDescansados = EquiposDescansaron(fechaActual, partidoA, ultimoPartidoPorEquipo)
                                              && EquiposDescansaron(fechaActual, partidoB, ultimoPartidoPorEquipo);
                    if (caben && equiposDescansados)
                        break;

                    fechaActual = fechaActual.AddDays(1);
                }

                partidoA.Fecha = fechaActual.AddHours(14);
                partidoB.Fecha = fechaActual.AddHours(14);
                ActualizarContadores(fechaActual, partidoA, partidosPorDia, ultimoPartidoPorEquipo);
                ActualizarContadores(fechaActual, partidoB, partidosPorDia, ultimoPartidoPorEquipo);
            }
        }
        
        private DateTime BuscarFechaDisponible(
            DateTime fechaDesde,
            Partido partido,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            var fechaActual = fechaDesde;
            while (true)
            {
                if (!partidosPorDia.ContainsKey(fechaActual))
                    partidosPorDia[fechaActual] = 0;

                bool hayHueco = partidosPorDia[fechaActual] < MaxPartidosPorDia;
                bool equiposDescansados = EquiposDescansaron(fechaActual, partido, ultimoPartidoPorEquipo);

                if (hayHueco && equiposDescansados)
                    return fechaActual;

                fechaActual = fechaActual.AddDays(1);
            }
        }
        
        private bool EquiposDescansaron(
            DateTime fecha,
            Partido partido,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            if (ultimoPartidoPorEquipo.TryGetValue(partido.EquipoLocal.Nombre, out var ultimoLocal))
                if ((fecha - ultimoLocal.Date).Days < SeparacionEntreFechas)
                    return false;

            if (ultimoPartidoPorEquipo.TryGetValue(partido.EquipoVisitante.Nombre, out var ultimoVisitante))
                if ((fecha - ultimoVisitante.Date).Days < SeparacionEntreFechas)
                    return false;

            return true;
        }
        
        private void ActualizarContadores(
            DateTime fecha,
            Partido partido,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            if (!partidosPorDia.ContainsKey(fecha))
                partidosPorDia[fecha] = 0;

            partidosPorDia[fecha]++;
            ultimoPartidoPorEquipo[partido.EquipoLocal.Nombre] = fecha;
            ultimoPartidoPorEquipo[partido.EquipoVisitante.Nombre] = fecha;
        }
    }
}