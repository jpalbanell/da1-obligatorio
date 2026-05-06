using Dominio.Entidades;
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

        public FixtureServicio(
            IEquipoRepositorio equipoRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IGrupoRepositorio grupoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _grupoRepositorio = grupoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
        }

        public void GenerarFixture(Fixture fixture, Usuario usuario)
        {
            ValidarCantidadEquipos();
            ValidarCantidadEstadios();
            ValidarFixtureNoGenerado(fixture);
            CrearGrupos();

            var equiposOrdenados = OrdenarEquiposPorRanking(fixture.SemillaFixture);
            DistribuirEquiposEnGrupos(equiposOrdenados);
            GenerarPartidosPorGrupo(fixture);
            AsignarEstadios();
            
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar("Generación de fixture", usuario);
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
            string[] etiquetas = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
            foreach (var etiqueta in etiquetas)
            {
                var grupo = new Grupo();
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

            for (int i = 0; i < equiposOrdenados.Count; i++)
            {
                var grupo = grupos[i % 12];
                var posicion = new PosicionesGrupo();
                posicion.Equipo = equiposOrdenados[i];
                grupo.ListaPosiciones.Add(posicion);
            }
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
                    partido.EquipoLocal = equipos[local];
                    partido.EquipoVisitante = equipos[visitante];
                    partido.Grupo = grupo;
                    partido.Fase = FaseTorneo.FaseGrupos;
                    partido.Codigo = $"{grupo.Etiqueta}{i + 1}";
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

            for (int jornada = 0; jornada < 3; jornada++)
            {
                var fechaJornada = fixture.FechaInicioTorneo.Date
                    .AddDays(jornada * fixture.SeparacionEntreFechas);

                var partidosDeJornada = new List<Partido>();

                foreach (var partidosGrupo in todosLosPartidos)
                {
                    int inicioJornada = jornada * 2;
                    partidosDeJornada.Add(partidosGrupo[inicioJornada]);
                    partidosDeJornada.Add(partidosGrupo[inicioJornada + 1]);
                }

                var fechaActual = fechaJornada;

                foreach (var partido in partidosDeJornada)
                {
                    while (true)
                    {
                        if (!partidosPorDia.ContainsKey(fechaActual))
                            partidosPorDia[fechaActual] = 0;

                        if (partidosPorDia[fechaActual] < fixture.MaxPartidosPorDia)
                            break;

                        fechaActual = fechaActual.AddDays(1);
                    }

                    int turno = partidosPorDia[fechaActual];
                    partido.Fecha = fechaActual.AddHours(14 + (turno * 4));
                    partidosPorDia[fechaActual]++;
                }
            }
        }
        
        private void AsignarEstadios()
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => Normalizar(e.Nombre), StringComparer.Ordinal)
                .ToList();

            var partidos = _partidoRepositorio.ObtenerTodos();

            for (int i = 0; i < partidos.Count; i++)
            {
                partidos[i].Estadio = estadios[i % estadios.Count];
            }
        }
        
        private string Normalizar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            var resultado = texto.ToLowerInvariant();

            var formaDescompuesta = resultado.Normalize(NormalizationForm.FormD);
            var sinTildes = new StringBuilder();
            foreach (var c in formaDescompuesta)
            {
                var categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (categoria != UnicodeCategory.NonSpacingMark)
                    sinTildes.Append(c);
            }
            resultado = sinTildes.ToString().Normalize(NormalizationForm.FormC);

            var conEspacios = new StringBuilder();
            foreach (var c in resultado)
            {
                conEspacios.Append(char.IsLetterOrDigit(c) ? c : ' ');
            }
            resultado = conEspacios.ToString();

            resultado = Regex.Replace(resultado, @"\s+", " ").Trim();

            return resultado;
        }
    }
}