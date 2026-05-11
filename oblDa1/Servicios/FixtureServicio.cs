using Dominio.Entidades;
using Dominio;
using Repositorios;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Servicios
{
    public class FixtureServicio : IFixtureServicio
    {
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IEstadioRepositorio _estadioRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        
        private int _proximoIdPartido = 1;
        private int _proximoIdGrupo = 1;
        
        private const int CantidadBombos = 4;
        private const int TamanoBombo = 12;
        
        public FixtureServicio(
            IEquipoRepositorio equipoRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IGrupoRepositorio grupoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _grupoRepositorio = grupoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void GenerarFixture(Fixture fixture)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            ValidarCantidadEquipos();
            ValidarCantidadEstadios();
            ValidarFixtureNoGenerado(fixture);
            _proximoIdGrupo = 1;
            _proximoIdPartido = 1;
            CrearGrupos();

            var equiposOrdenados = OrdenarEquiposPorRanking(fixture.SemillaFixture);
            DistribuirEquiposEnGrupos(equiposOrdenados);
            GenerarPartidosPorGrupo(fixture);
            AsignarEstadios();

            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Generación de fixture con SemillaFixture: {fixture.SemillaFixture}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private void ValidarCantidadEquipos()
        {
            var equipos = _equipoRepositorio.ObtenerTodos();
            if (equipos.Count != 48)
                throw new Exception("Se necesitan exactamente 48 equipos para generar el fixture.");
        }
        
        private void ValidarCantidadEstadios()
        {
            var estadios = _estadioRepositorio.ObtenerTodos();
            if (estadios.Count < 4)
                throw new Exception("Se necesitan al menos 4 estadios para generar el fixture.");
        }
        
        private void ValidarFixtureNoGenerado(Fixture fixture)
        {
            if (fixture.EstaGenerado)
                throw new Exception("El fixture ya fue generado.");
        }
        
        private void CrearGrupos()
        {
            var etiquetas = Grupo.EtiquetasValidas;
            foreach (var etiqueta in etiquetas)
            {
                var grupo = new Grupo();
                grupo.Id = _proximoIdGrupo++;
                grupo.Etiqueta = etiqueta;
                _grupoRepositorio.Agregar(grupo);
            }
        }
        
        private List<Equipo> OrdenarEquiposPorRanking(int semilla)
        {
            var equipos = _equipoRepositorio.ObtenerTodos()
                .OrderByDescending(e => e.RankingFifa)
                .ToList();

            var random = new Random(semilla);
            var empates = equipos.GroupBy(e => e.RankingFifa)
                .Where(g => g.Count() > 1);

            foreach (var grupo in empates)
            {
                var indices = equipos
                    .Select((e, i) => new { Equipo = e, Indice = i })
                    .Where(x => x.Equipo.RankingFifa == grupo.Key)
                    .Select(x => x.Indice)
                    .ToList();

                for (int i = indices.Count - 1; i > 0; i--)
                {
                    int j = random.Next(0, i + 1);
                    var temp = equipos[indices[i]];
                    equipos[indices[i]] = equipos[indices[j]];
                    equipos[indices[j]] = temp;
                }
            }

            return equipos;
        }
        
        private void DistribuirEquiposEnGrupos(List<Equipo> equiposOrdenados)
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var bombos = Enumerable.Range(0, CantidadBombos)
                .Select(i => equiposOrdenados.Skip(i * TamanoBombo).Take(TamanoBombo).ToList())
                .ToList();
            foreach (var bombo in bombos)
                AsignarBomboAGrupos(bombo, grupos);
        }
        
        private void AsignarBomboAGrupos(List<Equipo> bombo, List<Grupo> grupos)
        {
            var gruposDisponibles = new List<Grupo>(grupos);
            var asignaciones = new List<(Equipo, Grupo)>();

            if (!AsignarBomboConBacktracking(bombo, 0, gruposDisponibles, asignaciones))
                throw new Exception("No se encontró una distribución válida para el bombo actual.");

            foreach (var (equipo, grupo) in asignaciones)
            {
                var posicion = new PosicionesGrupo();
                posicion.Equipo = equipo;
                grupo.ListaPosiciones.Add(posicion);
            }
        }
        
        private bool AsignarBomboConBacktracking(
            List<Equipo> bombo,
            int indice,
            List<Grupo> gruposDisponibles,
            List<(Equipo, Grupo)> asignaciones)
        {
            if (indice == bombo.Count)
                return true;

            var equipo = bombo[indice];

            foreach (var grupo in gruposDisponibles.ToList())
            {
                if (!PuedeAgregarseAlGrupo(grupo, equipo))
                    continue;

                gruposDisponibles.Remove(grupo);
                asignaciones.Add((equipo, grupo));

                if (AsignarBomboConBacktracking(bombo, indice + 1, gruposDisponibles, asignaciones))
                    return true;

                gruposDisponibles.Add(grupo);
                asignaciones.RemoveAt(asignaciones.Count - 1);
            }

            return false;
        }
        
        private bool PuedeAgregarseAlGrupo(Grupo grupo, Equipo equipo)
        {
            if (grupo.ListaPosiciones.Count >= 4)
                return false;

            var equiposEnGrupo = grupo.ListaPosiciones.Select(p => p.Equipo).ToList();
            var cantidadMismaConfederacion = equiposEnGrupo.Count(e => e.Confederacion == equipo.Confederacion);

            if (equipo.Confederacion == Confederacion.UEFA)
                return cantidadMismaConfederacion < 2;

            return cantidadMismaConfederacion == 0;
        }
        
        private void GenerarPartidosPorGrupo(Fixture fixture)
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var todosLosPartidos = new List<List<Partido>>();

            foreach (var grupo in grupos)
            {
                var equipos = grupo.ListaPosiciones.Select(p => p.Equipo).ToList();

                var cruces = new (int, int)[]
                {
                    (0, 3), (1, 2),
                    (0, 2), (1, 3),
                    (0, 1), (2, 3)
                };

                var partidosDelGrupo = new List<Partido>();

                for (int i = 0; i < cruces.Length; i++)
                {
                    var (local, visitante) = cruces[i];
                    var partido = new Partido();
                    partido.Id = _proximoIdPartido++;
                    partido.EquipoLocal = equipos[local];
                    partido.EquipoVisitante = equipos[visitante];
                    partido.Grupo = grupo;
                    partido.Fase = FaseTorneo.FaseGrupos;
                    partido.Codigo = $"G{grupo.Etiqueta}-{i + 1}";
                    grupo.ListaPartidos.Add(partido);
                    partidosDelGrupo.Add(partido);
                }

                todosLosPartidos.Add(partidosDelGrupo);
            }

            AsignarFechas(todosLosPartidos, fixture);

            foreach (var partidosGrupo in todosLosPartidos)
            {
                foreach (var partido in partidosGrupo)
                {
                    _partidoRepositorio.Agregar(partido);
                }
            }
        }
        
        private void AsignarFechas(List<List<Partido>> todosLosPartidos, Fixture fixture)
        {
            var partidosPorDia = new Dictionary<DateTime, int>();
            var ultimoPartidoPorEquipo = new Dictionary<string, DateTime>();

            AsignarFechasJornadasPrevias(todosLosPartidos, fixture, partidosPorDia, ultimoPartidoPorEquipo);
            AsignarFechasUltimaJornada(todosLosPartidos, fixture, partidosPorDia, ultimoPartidoPorEquipo);
        }
        
        
        private void AsignarEstadios()
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => UtilTexto.Normalizar(e.Nombre), StringComparer.Ordinal)
                .ToList();

            var partidos = _partidoRepositorio.ObtenerTodos();

            for (int i = 0; i < partidos.Count; i++)
            {
                partidos[i].Estadio = estadios[i % estadios.Count];
            }
        }
        
        private void AsignarFechasJornadasPrevias(
            List<List<Partido>> todosLosPartidos,
            Fixture fixture,
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

            var fechaActual = fixture.FechaInicioTorneo.Date;

            foreach (var partido in partidosJornadasPrevias)
            {
                fechaActual = BuscarFechaDisponible(
                    fechaActual, partido, fixture, partidosPorDia, ultimoPartidoPorEquipo);

                int turno = partidosPorDia.ContainsKey(fechaActual) ? partidosPorDia[fechaActual] : 0;
                partido.Fecha = fechaActual.AddHours(14 + (turno * 4));

                ActualizarContadores(fechaActual, partido, partidosPorDia, ultimoPartidoPorEquipo);
            }
        }
        
        private void AsignarFechasUltimaJornada(
            List<List<Partido>> todosLosPartidos,
            Fixture fixture,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            foreach (var partidosGrupo in todosLosPartidos)
            {
                var partidoA = partidosGrupo[4];
                var partidoB = partidosGrupo[5];

                var fechaBase = fixture.FechaInicioTorneo.Date;
                var fechaActual = fechaBase;

                while (true)
                {
                    if (!partidosPorDia.ContainsKey(fechaActual))
                        partidosPorDia[fechaActual] = 0;

                    bool caben = partidosPorDia[fechaActual] + 2 <= fixture.MaxPartidosPorDia;
                    bool equiposDescansados = EquiposDescansaron(fechaActual, partidoA, fixture, ultimoPartidoPorEquipo)
                                              && EquiposDescansaron(fechaActual, partidoB, fixture, ultimoPartidoPorEquipo);

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
            Fixture fixture,
            Dictionary<DateTime, int> partidosPorDia,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            var fechaActual = fechaDesde;

            while (true)
            {
                if (!partidosPorDia.ContainsKey(fechaActual))
                    partidosPorDia[fechaActual] = 0;

                bool hayHueco = partidosPorDia[fechaActual] < fixture.MaxPartidosPorDia;
                bool equiposDescansados = EquiposDescansaron(fechaActual, partido, fixture, ultimoPartidoPorEquipo);

                if (hayHueco && equiposDescansados)
                    return fechaActual;

                fechaActual = fechaActual.AddDays(1);
            }
        }
        
        private bool EquiposDescansaron(
            DateTime fecha,
            Partido partido,
            Fixture fixture,
            Dictionary<string, DateTime> ultimoPartidoPorEquipo)
        {
            if (ultimoPartidoPorEquipo.TryGetValue(partido.EquipoLocal.Nombre, out var ultimoLocal))
            {
                if ((fecha - ultimoLocal.Date).Days < fixture.SeparacionEntreFechas)
                    return false;
            }

            if (ultimoPartidoPorEquipo.TryGetValue(partido.EquipoVisitante.Nombre, out var ultimoVisitante))
            {
                if ((fecha - ultimoVisitante.Date).Days < fixture.SeparacionEntreFechas)
                    return false;
            }

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
        
        public List<Grupo> ObtenerGrupos()
        {
            return _grupoRepositorio.ObtenerTodos();
        }

        public Fixture ObtenerFixture()
        {
            return _fixtureRepositorio.Obtener();
        }
    }
}