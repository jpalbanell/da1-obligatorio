using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class CruceServicioTests
    {
        private IGrupoRepositorio _grupoRepositorio = null!;
        private IPartidoRepositorio _partidoRepositorio = null!;
        private IAuditoriaServicio _auditoriaServicio = null!;
        private ISesionServicio _sesionServicio = null!;
        private IFixtureRepositorio _fixtureRepositorio = null!;
        private ICruceServicio _servicio = null!;

        [TestInitialize]
        public void Setup()
        {
            _grupoRepositorio = new GrupoRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _sesionServicio = new SesionServicio();
            _fixtureRepositorio = new FixtureRepositorio();
            _servicio = new CruceServicio(
                _grupoRepositorio,
                _partidoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio,
                _sesionServicio
            );
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_SinFixtureGenerado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = false;
            _fixtureRepositorio.Guardar(fixture);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            return usuario;
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_CrucesYaGenerados_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            fixture.CrucesGenerados = true;
            _fixtureRepositorio.Guardar(fixture);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }
    }
}