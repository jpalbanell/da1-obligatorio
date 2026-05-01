using Dominio.Entidades;
using Repositorios;

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
            foreach (var grupo in grupos)
            {
                var equipos = grupo.ListaPosiciones.Select(p => p.Equipo).ToList();

                var cruces = new (int, int)[]
                {
                    (0, 3), (1, 2),
                    (0, 2), (1, 3),
                    (0, 1), (2, 3)
                };

                for (int i = 0; i < cruces.Length; i++)
                {
                    var (local, visitante) = cruces[i];
                    int jornada = i / 2;
                    var fechaPartido = fixture.FechaInicioTorneo
                        .AddDays(jornada * fixture.SeparacionEntreFechas)
                        .Date
                        .AddHours(14);

                    var partido = new Partido();
                    partido.EquipoLocal = equipos[local];
                    partido.EquipoVisitante = equipos[visitante];
                    partido.Grupo = grupo;
                    partido.Fase = FaseTorneo.FaseGrupos;
                    partido.Fecha = fechaPartido;
                    _partidoRepositorio.Agregar(partido);
                }
            }
        }
        
        private void AsignarEstadios()
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => e.Nombre)
                .ToList();

            var partidos = _partidoRepositorio.ObtenerTodos();

            for (int i = 0; i < partidos.Count; i++)
            {
                partidos[i].Estadio = estadios[i % estadios.Count];
            }
        }
    }
}