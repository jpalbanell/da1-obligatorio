using Dominio.Entidades;
using Servicios;
using IRepositorios;
using IServicios;
using Moq;

namespace Tests.TestsServicios
{
    [TestClass]
    public class NotificacionServicioTests
    {
        private INotificacionServicio _servicio;
        private Mock<INotificacionRepositorio> _notificacionRepoMock;
        private Mock<IUsuarioRepositorio> _usuarioRepoMock;

        [TestInitialize]
        public void Setup()
        {
            _notificacionRepoMock = new Mock<INotificacionRepositorio>();
            _usuarioRepoMock = new Mock<IUsuarioRepositorio>();
            _servicio = new NotificacionServicio(_notificacionRepoMock.Object, _usuarioRepoMock.Object);
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Test";
            usuario.Apellido = "Usuario";
            usuario.Email = "test@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }

        [TestMethod]
        public void Notificar_MensajeValidoYPeriodista_LlamaAgregar()
        {
            var periodista = CrearUsuarioValido();

            _servicio.Notificar("Partido editado", periodista);

            _notificacionRepoMock.Verify(r => r.Agregar(It.IsAny<Notificacion>()), Times.Once);
        }

        [TestMethod]
        public void NotificarPorRol_RolConDosUsuarios_LlamaAgregarDosVeces()
        {
            var periodistas = new List<Usuario> { CrearUsuarioValido(), CrearUsuarioValido() };
            _usuarioRepoMock.Setup(r => r.ObtenerPorRol(Rol.Periodista)).Returns(periodistas);

            _servicio.NotificarPorRol("Resultado simulado", Rol.Periodista);

            _notificacionRepoMock.Verify(r => r.Agregar(It.IsAny<Notificacion>()), Times.Exactly(2));
        }

        [TestMethod]
        public void MarcarLeida_IdExistente_LlamaActualizar()
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = "Partido editado";
            notificacion.FechaCreacion = DateTime.Now;
            notificacion.Periodista = CrearUsuarioValido();
            _notificacionRepoMock.Setup(r => r.ObtenerPorId(1)).Returns(notificacion);

            _servicio.MarcarLeida(1);

            _notificacionRepoMock.Verify(r => r.Actualizar(notificacion), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void MarcarLeida_IdInexistente_LanzaKeyNotFoundException()
        {
            _notificacionRepoMock.Setup(r => r.ObtenerPorId(99)).Returns((Notificacion)null);

            _servicio.MarcarLeida(99);
        }

        [TestMethod]
        public void ObtenerNoLeidas_UsuarioConNotificaciones_DelegaEnRepositorio()
        {
            var periodista = CrearUsuarioValido();
            var notificaciones = new List<Notificacion> { new Notificacion(), new Notificacion() };
            _notificacionRepoMock.Setup(r => r.ObtenerNoLeidasPorUsuario(periodista)).Returns(notificaciones);

            var resultado = _servicio.ObtenerNoLeidas(periodista);

            Assert.AreEqual(2, resultado.Count);
        }
    }
}
