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
        
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AgregarEquipo_SinRolAdmin_LanzaExcepcion()
        {
            _sesionServicio.CerrarSesion();
            var editor = new Usuario();
            editor.Nombre = "Editor";
            editor.Apellido = "Test";
            editor.Email = "editor@test.com";
            editor.FechaNacimiento = new DateTime(1990, 1, 1);
            editor.Contrasena = "Password@1";
            editor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(editor);

            _torneoServicio.AgregarEquipo(CrearEquipoValido());
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            _torneoServicio.AgregarEquipo(CrearEquipoValido());
            _torneoServicio.AgregarEquipo(CrearEquipoValido());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_CupoConfederacionCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 7; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CONMEBOL_{i}";
                equipo.Confederacion = Confederacion.CONMEBOL;
                equipo.RankingFifa = 1500;
                _torneoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CONMEBOL_08";
            equipoExtra.Confederacion = Confederacion.CONMEBOL;
            equipoExtra.RankingFifa = 1500;
            _torneoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        public void EditarEquipo_RolAdminYDatosValidos_EditaCorrectamente()
        {
            _torneoServicio.AgregarEquipo(CrearEquipoValido());

            var equipoEditado = new Equipo();
            equipoEditado.Nombre = "Uruguay Editado";
            equipoEditado.Confederacion = Confederacion.CONMEBOL;
            equipoEditado.RankingFifa = 1600;

            _torneoServicio.EditarEquipo(equipoEditado, "Uruguay");

            var resultado = _torneoServicio.ObtenerPorNombre("Uruguay Editado");
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void EditarEquipo_SinRolAdmin_LanzaExcepcion()
        {
            _torneoServicio.AgregarEquipo(CrearEquipoValido());
            _sesionServicio.CerrarSesion();
            var editor = new Usuario();
            editor.Nombre = "Editor";
            editor.Apellido = "Test";
            editor.Email = "editor@test.com";
            editor.FechaNacimiento = new DateTime(1990, 1, 1);
            editor.Contrasena = "Password@1";
            editor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(editor);

            _torneoServicio.EditarEquipo(CrearEquipoValido(), "Uruguay");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            _torneoServicio.AgregarEquipo(CrearEquipoValido());
            var equipo2 = new Equipo();
            equipo2.Nombre = "Argentina";
            equipo2.Confederacion = Confederacion.CONMEBOL;
            equipo2.RankingFifa = 1600;
            _torneoServicio.AgregarEquipo(equipo2);

            var equipoEditado = new Equipo();
            equipoEditado.Nombre = "Argentina";
            equipoEditado.Confederacion = Confederacion.CONMEBOL;
            equipoEditado.RankingFifa = 1500;
            _torneoServicio.EditarEquipo(equipoEditado, "Uruguay");
        }
    }
    

}