using Dominio.Entidades;
using IRepositorios;
using Servicios;
using IServicios;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class AutenticacionServicioTests
    {
        private IUsuarioServicio _usuarioServicio;
        private IUsuarioRepositorio _repositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;
        private IAutenticacionServicio _autenticacionServicio;
        private Usuario _usuarioAdmin;
        private Usuario _usuarioOtro;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new UsuarioRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _usuarioServicio = new UsuarioServicio(_repositorio, _auditoriaServicio, _sesionServicio);
            _autenticacionServicio = new AutenticacionServicio(_repositorio, _auditoriaServicio, _sesionServicio);

             _usuarioAdmin = new Usuario();
            _usuarioAdmin.Nombre = "Santiago";
            _usuarioAdmin.Apellido = "Garcia";
            _usuarioAdmin.Email = "santiago@ejemplo.com";
            _usuarioAdmin.FechaNacimiento = new DateTime(1990, 5, 15);
            _usuarioAdmin.Contrasena = "Abcdef1@";
            _usuarioAdmin.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(_usuarioAdmin);
            
            _usuarioOtro = new Usuario();
            _usuarioOtro.Id = 2;
            _usuarioOtro.Nombre = "Juan";
            _usuarioOtro.Apellido = "Perez";
            _usuarioOtro.Email = "otro@ejemplo.com";
            _usuarioOtro.FechaNacimiento = new DateTime(1995, 3, 10);
            _usuarioOtro.Contrasena = "Abcdef1@";
            _repositorio.Agregar(_usuarioOtro);
        }

        private Usuario CrearUsuarioValido(string nombre, string apellido, string email)
        {
            var _usuarioAdmin = new Usuario();
            _usuarioAdmin.Nombre = nombre;
            _usuarioAdmin.Apellido = apellido;
            _usuarioAdmin.Email = email;
            _usuarioAdmin.FechaNacimiento = new DateTime(1990, 5, 15);
            _usuarioAdmin.Contrasena = "Abcdef1@";
            return _usuarioAdmin;
        }

        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaRetornarUsuario()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Login_ConEmailInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.Login("noexiste@ejemplo.com", "Abcdef1@");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Login_ConContrasenaIncorrecta_DeberiaLanzarExcepcion()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            _autenticacionServicio.Login("juan@ejemplo.com", "Incorrecta@1");
        }
        
        [TestMethod]
        public void CambiarContrasena_ConContrasenaValida_DeberiaActualizar()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);
            _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            _autenticacionServicio.CambiarContrasena(_usuarioAdmin.Id, "NuevaPass@1");
            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "NuevaPass@1");

            Assert.IsNotNull(resultado);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CambiarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.CambiarContrasena(999, "NuevaPass@1");
        }
        
        [TestMethod]
        public void ReiniciarContrasena_ConUsuarioExistente_DeberiaAsignarContrasenaPorDefecto()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            _autenticacionServicio.ReiniciarContrasena(_usuarioAdmin.Id);
            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "Password@1");

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReiniciarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.ReiniciarContrasena(999);
        }
        
        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaIniciarSesion()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            Assert.AreEqual("Juan", _sesionServicio.ObtenerUsuarioActual().Nombre);
        }
        
        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaRegistrarEnAuditoria()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => l.Accion.Contains("juan@ejemplo.com")));
        }
        
        [TestMethod]
        public void CambiarContrasena_ConUsuarioExistente_DeberiaRegistrarEnAuditoria()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);
            _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            _autenticacionServicio.CambiarContrasena(_usuarioAdmin.Id, "NuevaPass@1");

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => l.Accion.Contains("contraseña") && l.Accion.Contains("juan@ejemplo.com")));
        }
        
        [TestMethod]
        public void ReiniciarContrasena_ConUsuarioExistente_DeberiaRegistrarEnAuditoria()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            _autenticacionServicio.ReiniciarContrasena(_usuarioAdmin.Id);

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => 
                l.Accion.Contains("reinicio", StringComparison.OrdinalIgnoreCase) && 
                l.Accion.Contains("juan@ejemplo.com")));
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReiniciarContrasena_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var _usuarioAdminObjetivo = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdminObjetivo);

            var _usuarioAdminEditor = CrearUsuarioValido("Editor", "Editor", "editor@ejemplo.com");
            _usuarioAdminEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(_usuarioAdminEditor);

            _autenticacionServicio.ReiniciarContrasena(_usuarioAdminObjetivo.Id);
        }
        
        [TestMethod]
        public void Login_ConEmailEnDistintaCapitalizacion_DeberiaFuncionar()
        {
            var _usuarioAdmin = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(_usuarioAdmin);

            var resultado = _autenticacionServicio.Login("JUAN@EJEMPLO.COM", "Abcdef1@");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReiniciarContrasena_PropiaContrasena_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.ReiniciarContrasena(_usuarioAdmin.Id);
        }

        [TestMethod]
        public void ReiniciarContrasena_UsuarioValido_RetornaContrasenaDefault()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(usuario);

            _autenticacionServicio.ReiniciarContrasena(usuario.Id);

            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "Password@1");
            Assert.IsNotNull(resultado);
        }
    }
}