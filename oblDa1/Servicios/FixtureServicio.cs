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

        public void GenerarFixture(Fixture fixture)
        {
            ValidarCantidadEquipos();
            ValidarCantidadEstadios();
            ValidarFixtureNoGenerado(fixture);
            CrearGrupos();

            var equiposOrdenados = OrdenarEquiposPorRanking(fixture.SemillaFixture);
            DistribuirEquiposEnGrupos(equiposOrdenados);
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
    }
}