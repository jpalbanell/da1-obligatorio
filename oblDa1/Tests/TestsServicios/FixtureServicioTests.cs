using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class FixtureServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio;
        private IEstadioRepositorio _estadioRepositorio;
        private IPartidoRepositorio _partidoRepositorio;
        private IGrupoRepositorio _grupoRepositorio;
        private IFixtureRepositorio _fixtureRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IFixtureServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _grupoRepositorio = new GrupoRepositorio();
            _fixtureRepositorio = new FixtureRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _servicio = new FixtureServicio(
                _equipoRepositorio,
                _estadioRepositorio,
                _partidoRepositorio,
                _grupoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio
            );
        }
        
        private void CargarEquipos(int cantidad)
        {
            Confederacion[] confederaciones = {
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.OFC
            };

            for (int i = 0; i < cantidad; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"Equipo_{i + 1}";
                equipo.Confederacion = confederaciones[i];
                equipo.RankingFifa = 2500 - (i * 45);
                _equipoRepositorio.Agregar(equipo);
            }
        }
        
        private void CargarEstadios(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                var estadio = new Estadio();
                estadio.Nombre = $"Estadio_{i + 1}";
                estadio.Ciudad = $"Ciudad_{i + 1}";
                estadio.Capacidad = 40000;
                _estadioRepositorio.Agregar(estadio);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_SinEquipos_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_Sin4Estadios_DeberiaLanzarExcepcion()
        {
            CargarEquipos(48);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_YaGenerado_DeberiaLanzarExcepcion()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            fixture.EstaGenerado = true;

            _servicio.GenerarFixture(fixture);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaCrear12Grupos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture);

            var grupos = _grupoRepositorio.ObtenerTodos();
            Assert.AreEqual(12, grupos.Count);
        }
    }
}