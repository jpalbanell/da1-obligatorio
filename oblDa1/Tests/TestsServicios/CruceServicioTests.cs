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
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_ConPartidosSinResultado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);

            var grupo = CrearGrupoConPartidosSinResultado();
            _grupoRepositorio.Agregar(grupo);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }

        private Grupo CrearGrupoConPartidosSinResultado()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            var equipo1 = CrearEquipo("Uruguay", Confederacion.CONMEBOL, 2000);
            var equipo2 = CrearEquipo("Argentina", Confederacion.CONMEBOL, 1900);
            var estadio = CrearEstadio("Centenario");

            var partido = new Partido(1);
            partido.Codigo = "GA1";
            partido.Fecha = new DateTime(2026, 6, 1, 14, 0, 0);
            partido.Fase = FaseTorneo.FaseGrupos;
            partido.EquipoLocal = equipo1;
            partido.EquipoVisitante = equipo2;
            partido.Estadio = estadio;
            partido.Grupo = grupo;
            partido.TieneResultado = false;

            grupo.ListaPartidos.Add(partido);
            return grupo;
        }

        private Equipo CrearEquipo(string nombre, Confederacion confederacion, int ranking)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.Confederacion = confederacion;
            equipo.RankingFifa = ranking;
            return equipo;
        }

        private Estadio CrearEstadio(string nombre)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;
            return estadio;
        }
    }
}