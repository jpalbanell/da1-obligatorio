using Dominio.Entidades;
using IRepositorios;
using Servicios;
using IServicios;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class UsuarioServicioTests
    {
        private IUsuarioServicio _servicio;
        private IUsuarioRepositorio _repositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new UsuarioRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _servicio = new UsuarioServicio(_repositorio, _auditoriaServicio, _sesionServicio);

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(usuario);
        }

        private Usuario CrearUsuarioValido(string nombre, string apellido, string email)
        {
            var usuario = new Usuario();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Email = email;
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            return usuario;
        }

        [TestMethod]
        public void AgregarUsuario_ConDatosValidos_DeberiaAgregarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod]
        public void AgregarUsuario_DeberiaAsignarIdAutomaticamente()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario);

            Assert.AreEqual(1, usuario.Id);
        }

        [TestMethod]
        public void AgregarUsuario_VariosUsuarios_DeberiaAsignarIdsIncrementales()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            var usuario2 = CrearUsuarioValido("María", "López", "maria@ejemplo.com");

            _servicio.AgregarUsuario(usuario1);
            _servicio.AgregarUsuario(usuario2);

            Assert.AreEqual(1, usuario1.Id);
            Assert.AreEqual(2, usuario2.Id);
        }

        [TestMethod]
        public void ObtenerUsuario_ConIdExistente_DeberiaRetornarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            var resultado = _servicio.ObtenerUsuario(usuario.Id);

            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        public void ObtenerUsuario_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _servicio.ObtenerUsuario(999);
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ObtenerTodos_ConVariosUsuarios_DeberiaRetornarTodos()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            var usuario2 = CrearUsuarioValido("María", "López", "maria@ejemplo.com");
            _servicio.AgregarUsuario(usuario1);
            _servicio.AgregarUsuario(usuario2);

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void ModificarUsuario_ConDatosValidos_DeberiaActualizar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            usuario.Nombre = "Juan Pablo";
            usuario.Apellido = "González";
            _servicio.ModificarUsuario(usuario);

            var resultado = _servicio.ObtenerUsuario(usuario.Id);
            Assert.AreEqual("Juan Pablo", resultado.Nombre);
            Assert.AreEqual("González", resultado.Apellido);
        }

        [TestMethod]
        public void EliminarUsuario_ConIdExistente_DeberiaEliminarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            _servicio.EliminarUsuario(usuario.Id);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarUsuario_ConIdInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.EliminarUsuario(999);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarUsuario_ConEmailDuplicado_DeberiaLanzarExcepcion()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            var usuario2 = CrearUsuarioValido("Pedro", "García", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario1);
            _servicio.AgregarUsuario(usuario2);
        }
        
        [TestMethod]
        public void AgregarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);
            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void ModificarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            usuario.Nombre = "Juan Pablo";
            _servicio.ModificarUsuario(usuario);

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void EliminarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            _servicio.EliminarUsuario(usuario.Id);

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AgregarUsuario_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            var usuarioEditor = new Usuario();
            usuarioEditor.Nombre = "Juan";
            usuarioEditor.Apellido = "Perez";
            usuarioEditor.Email = "juan@ejemplo.com";
            usuarioEditor.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioEditor.Contrasena = "Abcdef1@";
            usuarioEditor.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuarioEditor);

            var nuevoUsuario = new Usuario();
            nuevoUsuario.Nombre = "Pedro";
            nuevoUsuario.Apellido = "Lopez";
            nuevoUsuario.Email = "pedro@ejemplo.com";
            nuevoUsuario.FechaNacimiento = new DateTime(1995, 3, 20);
            nuevoUsuario.Contrasena = "Abcdef1@";

            _servicio.AgregarUsuario(nuevoUsuario);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ModificarUsuario_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Id = 999;
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";

            _servicio.ModificarUsuario(usuario);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModificarUsuario_ConEmailDeOtroUsuario_DeberiaLanzarExcepcion()
        {
            var usuario1 = new Usuario();
            usuario1.Nombre = "Juan";
            usuario1.Apellido = "Perez";
            usuario1.Email = "juan@ejemplo.com";
            usuario1.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario1.Contrasena = "Abcdef1@";
            _servicio.AgregarUsuario(usuario1);

            var usuario2 = new Usuario();
            usuario2.Nombre = "Pedro";
            usuario2.Apellido = "Lopez";
            usuario2.Email = "pedro@ejemplo.com";
            usuario2.FechaNacimiento = new DateTime(1995, 3, 20);
            usuario2.Contrasena = "Abcdef1@";
            _servicio.AgregarUsuario(usuario2);

            usuario2.Email = "juan@ejemplo.com";
            _servicio.ModificarUsuario(usuario2);
        }
    }
}