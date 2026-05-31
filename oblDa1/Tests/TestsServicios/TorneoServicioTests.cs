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
        
        private Estadio CrearEstadioValido()
        {
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 25000;
            return estadio;
        }
        
        private void IniciarSesionComoEditor()
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
        
        [TestMethod]
        public void EliminarEquipo_RolAdminYEquipoExiste_EliminaCorrectamente()
        {
            _torneoServicio.AgregarEquipo(CrearEquipoValido());

            _torneoServicio.EliminarEquipo("Uruguay");

            var resultado = _torneoServicio.ObtenerPorNombre("Uruguay");
            Assert.IsNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void EliminarEquipo_SinRolAdmin_LanzaExcepcion()
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

            _torneoServicio.EliminarEquipo("Uruguay");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEquipo_NoExiste_LanzaExcepcion()
        {
            _torneoServicio.EliminarEquipo("Uruguay");
        }

        [TestMethod]
        public void CompletarEquiposAutomaticamente_RolAdmin_Completa48Equipos()
        {
            _torneoServicio.CompletarEquiposAutomaticamente(42);

            var equipos = _torneoServicio.ObtenerTodos();
            Assert.AreEqual(48, equipos.Count);
        }
        
        [TestMethod]
        public void AgregarEstadio_RolAdminYEstadioValido_AgregaCorrectamente()
        {
            _torneoServicio.AgregarEstadio(CrearEstadioValido());

            var estadios = _torneoServicio.ObtenerTodosEstadios();
            Assert.AreEqual(1, estadios.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AgregarEstadio_SinRolAdmin_LanzaExcepcion()
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

            _torneoServicio.AgregarEstadio(CrearEstadioValido());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEstadio_NombreDuplicado_LanzaExcepcion()
        {
            _torneoServicio.AgregarEstadio(CrearEstadioValido());
            _torneoServicio.AgregarEstadio(CrearEstadioValido());
        }

        [TestMethod]
        public void EliminarEstadio_Existe_EliminaCorrectamente()
        {
            _torneoServicio.AgregarEstadio(CrearEstadioValido());

            _torneoServicio.EliminarEstadio("Centenario");

            var resultado = _torneoServicio.ObtenerEstadio("Centenario");
            Assert.IsNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEstadio_NoExiste_LanzaExcepcion()
        {
            _torneoServicio.EliminarEstadio("Centenario");
        }

        [TestMethod]
        public void ModificarEstadio_DatosValidos_ModificaCorrectamente()
        {
            _torneoServicio.AgregarEstadio(CrearEstadioValido());

            var estadioEditado = new Estadio();
            estadioEditado.Nombre = "Centenario Editado";
            estadioEditado.Ciudad = "Montevideo";
            estadioEditado.Capacidad = 25000;

            _torneoServicio.ModificarEstadio(estadioEditado, "Centenario");

            var resultado = _torneoServicio.ObtenerEstadio("Centenario Editado");
            Assert.IsNotNull(resultado);
        }
        
        [TestMethod]
        public void ImportarEquipos_CsvValido_ImportaCorrectamente()
        {
            IniciarSesionComoEditor();
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nArgentina,CONMEBOL,1600";

            var resultado = _torneoServicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_FilaConError_RegistraError()
        {
            IniciarSesionComoEditor();
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nMal,ConfederacionInvalida,999";

            var resultado = _torneoServicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_NombreDuplicado_RegistraError()
        {
            IniciarSesionComoEditor();
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nUruguay,CONMEBOL,1600";

            var resultado = _torneoServicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ImportarEquipos_SinRolEditor_LanzaExcepcion()
        {
            _sesionServicio.CerrarSesion();
            var admin = new Usuario();
            admin.Nombre = "Admin";
            admin.Apellido = "Test";
            admin.Email = "admin2@test.com";
            admin.FechaNacimiento = new DateTime(1990, 1, 1);
            admin.Contrasena = "Password@1";
            admin.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(admin);

            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500";
            _torneoServicio.ImportarEquipos(csv);
        }
        
        [TestMethod]
        public void GenerarFixture_CondicionesValidas_GeneraFixture()
        {
            // Completar equipos como admin
            _torneoServicio.CompletarEquiposAutomaticamente(42);

            // Agregar estadios como admin
            for (int i = 1; i <= 4; i++)
            {
                var estadio = new Estadio();
                estadio.Nombre = $"Estadio {i}";
                estadio.Ciudad = "Montevideo";
                estadio.Capacidad = 25000;
                _torneoServicio.AgregarEstadio(estadio);
            }

            // Generar fixture como editor
            IniciarSesionComoEditor();
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _torneoServicio.GenerarFixture(fixture);

            Assert.IsTrue(fixture.EstaGenerado);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void GenerarFixture_SinRolEditor_LanzaExcepcion()
        {
            var fixture = new Fixture();
            _torneoServicio.GenerarFixture(fixture);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GenerarFixture_MenosDe48Equipos_LanzaExcepcion()
        {
            IniciarSesionComoEditor();
            var fixture = new Fixture();
            _torneoServicio.GenerarFixture(fixture);
        }
    }
    

}