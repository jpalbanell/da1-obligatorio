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
    }
}
