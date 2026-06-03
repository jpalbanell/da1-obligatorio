using Dominio.Entidades;
using IRepositorios;
using Servicios;
using IServicios;
using Moq;

namespace Tests
{
    [TestClass]
    public class UsuarioServicioTests
    {
        private IUsuarioServicio _servicio;
        private Mock<IUsuarioRepositorio> _repoMock;
        private Mock<IAuditoriaServicio> _auditoriaMock;
        private Mock<ISesionServicio> _sesionMock;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IUsuarioRepositorio>();
            _auditoriaMock = new Mock<IAuditoriaServicio>();
            _sesionMock = new Mock<ISesionServicio>();
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario>());
            _servicio = new UsuarioServicio(_repoMock.Object, _auditoriaMock.Object, _sesionMock.Object);
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

            _repoMock.Verify(r => r.Agregar(It.IsAny<Usuario>()), Times.Once);
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
            var usuarioExistente = CrearUsuarioValido("Existente", "Apellido", "existente@ejemplo.com");
            usuarioExistente.Id = 1;
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuarioExistente });

            var nuevoUsuario = CrearUsuarioValido("María", "López", "maria@ejemplo.com");
            _servicio.AgregarUsuario(nuevoUsuario);

            Assert.AreEqual(2, nuevoUsuario.Id);
        }

        [TestMethod]
        public void ObtenerUsuario_ConIdExistente_DeberiaRetornarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);

            var resultado = _servicio.ObtenerUsuario(1);

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
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario1, usuario2 });

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void ModificarUsuario_ConDatosValidos_DeberiaActualizar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            usuario.Nombre = "Juan Pablo";
            usuario.Apellido = "González";
            _servicio.ModificarUsuario(usuario);

            _repoMock.Verify(r => r.Actualizar(It.Is<Usuario>(u => u.Nombre == "Juan Pablo" && u.Apellido == "González")), Times.Once);
        }

        [TestMethod]
        public void EliminarUsuario_ConIdExistente_DeberiaEliminarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);

            _servicio.EliminarUsuario(1);

            _repoMock.Verify(r => r.Eliminar(1), Times.Once);
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
            var usuarioExistente = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuarioExistente });

            var duplicado = CrearUsuarioValido("Pedro", "García", "juan@ejemplo.com");
            _servicio.AgregarUsuario(duplicado);
        }

        [TestMethod]
        public void AgregarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario);

            _auditoriaMock.Verify(a => a.Registrar(It.IsAny<string>(), It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void ModificarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            usuario.Nombre = "Juan Pablo";
            _servicio.ModificarUsuario(usuario);

            _auditoriaMock.Verify(a => a.Registrar(It.IsAny<string>(), It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void EliminarUsuario_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);

            _servicio.EliminarUsuario(1);

            _auditoriaMock.Verify(a => a.Registrar(It.IsAny<string>(), It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AgregarUsuario_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            _sesionMock.Setup(s => s.ValidarRol(Rol.Administrador)).Throws<UnauthorizedAccessException>();

            var nuevoUsuario = CrearUsuarioValido("Pedro", "Lopez", "pedro@ejemplo.com");
            _servicio.AgregarUsuario(nuevoUsuario);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ModificarUsuario_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            var usuario = CrearUsuarioValido("Juan", "Perez", "juan@ejemplo.com");
            usuario.Id = 999;

            _servicio.ModificarUsuario(usuario);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModificarUsuario_ConEmailDeOtroUsuario_DeberiaLanzarExcepcion()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Perez", "juan@ejemplo.com");
            usuario1.Id = 1;
            var usuario2 = CrearUsuarioValido("Pedro", "Lopez", "pedro@ejemplo.com");
            usuario2.Id = 2;
            _repoMock.Setup(r => r.ObtenerPorId(2)).Returns(usuario2);
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario1, usuario2 });

            usuario2.Email = "juan@ejemplo.com";
            _servicio.ModificarUsuario(usuario2);
        }

        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaRetornarUsuario()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            var resultado = _servicio.Login("juan@ejemplo.com", "Abcdef1@");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void Login_ConEmailInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.Login("noexiste@ejemplo.com", "Abcdef1@");
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void Login_ConContrasenaIncorrecta_DeberiaLanzarExcepcion()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            _servicio.Login("juan@ejemplo.com", "Incorrecta@1");
        }

        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaIniciarSesion()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            _servicio.Login("juan@ejemplo.com", "Abcdef1@");

            _sesionMock.Verify(s => s.IniciarSesion(It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void Login_ConCredencialesValidas_DeberiaRegistrarEnAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            _servicio.Login("juan@ejemplo.com", "Abcdef1@");

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("juan@ejemplo.com")),
                It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void Login_ConEmailEnDistintaCapitalizacion_DeberiaFuncionar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Usuario> { usuario });

            var resultado = _servicio.Login("JUAN@EJEMPLO.COM", "Abcdef1@");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        public void CambiarContrasena_ConContrasenaValida_DeberiaActualizar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);

            _servicio.CambiarContrasena(1, "NuevaPass@1");

            _repoMock.Verify(r => r.Actualizar(It.Is<Usuario>(u => u.VerificarContrasena("NuevaPass@1"))), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CambiarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.CambiarContrasena(999, "NuevaPass@1");
        }

        [TestMethod]
        public void CambiarContrasena_ConUsuarioExistente_DeberiaRegistrarEnAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);

            _servicio.CambiarContrasena(1, "NuevaPass@1");

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("contraseña") && s.Contains("juan@ejemplo.com")),
                It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void ReiniciarContrasena_ConUsuarioExistente_DeberiaAsignarContrasenaPorDefecto()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);
            var adminActual = CrearUsuarioValido("Admin", "Actual", "admin@ejemplo.com");
            adminActual.Id = 99;
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(adminActual);

            _servicio.ReiniciarContrasena(1);

            _repoMock.Verify(r => r.Actualizar(It.Is<Usuario>(u => u.VerificarContrasena("Password@1"))), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ReiniciarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.ReiniciarContrasena(999);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ReiniciarContrasena_SinRolAdministrador_DeberiaLanzarExcepcion()
        {
            _sesionMock.Setup(s => s.ValidarRol(Rol.Administrador)).Throws<UnauthorizedAccessException>();

            _servicio.ReiniciarContrasena(1);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ReiniciarContrasena_PropiaContrasena_DeberiaLanzarExcepcion()
        {
            var usuario = CrearUsuarioValido("Admin", "Dos", "admindos@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(usuario);

            _servicio.ReiniciarContrasena(1);
        }

        [TestMethod]
        public void ReiniciarContrasena_ConUsuarioExistente_DeberiaRegistrarEnAuditoria()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repoMock.Setup(r => r.ObtenerPorId(1)).Returns(usuario);
            var adminActual = CrearUsuarioValido("Admin", "Actual", "admin@ejemplo.com");
            adminActual.Id = 99;
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(adminActual);

            _servicio.ReiniciarContrasena(1);

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("reinicio", StringComparison.OrdinalIgnoreCase) && s.Contains("juan@ejemplo.com")),
                It.IsAny<Usuario>()), Times.Once);
        }
    }
}
