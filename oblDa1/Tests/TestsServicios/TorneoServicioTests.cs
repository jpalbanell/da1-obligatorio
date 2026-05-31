using Dominio.Entidades;
using IRepositorios;
using IServicios;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class TorneoServicioTests
    {
        private TorneoServicio _torneoServicio;
        private IEquipoRepositorio _equipoRepositorio;
        private IEstadioRepositorio _estadioRepositorio;
        private IPartidoRepositorio _partidoRepositorio;
        private IGrupoRepositorio _grupoRepositorio;
        private IFixtureRepositorio _fixtureRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _grupoRepositorio = new GrupoRepositorio();
            _fixtureRepositorio = new FixtureRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _sesionServicio = new SesionServicio();
            _torneoServicio = new TorneoServicio(
                _equipoRepositorio,
                _estadioRepositorio,
                _partidoRepositorio,
                _grupoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio,
                _sesionServicio);

            var admin = new Usuario();
            admin.Nombre = "Admin";
            admin.Apellido = "Test";
            admin.Email = "admin@test.com";
            admin.FechaNacimiento = new DateTime(1990, 1, 1);
            admin.Contrasena = "Password@1";
            admin.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(admin);
        }

        private Equipo CrearEquipoValido()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            return equipo;
        }
        
        [TestMethod]
        public void AgregarEquipo_RolAdminYEquipoValido_AgregaCorrectamente()
        {
            var equipo = CrearEquipoValido();

            _torneoServicio.AgregarEquipo(equipo);

            var equipos = _torneoServicio.ObtenerTodos();
            Assert.AreEqual(1, equipos.Count);
        }
    }
    

}