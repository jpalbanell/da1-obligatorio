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

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_SinEquipos_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture);
        }
    }
}