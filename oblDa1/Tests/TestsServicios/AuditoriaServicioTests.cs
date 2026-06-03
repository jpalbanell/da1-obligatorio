using Dominio.Entidades;
using Servicios;
using IRepositorios;
using IServicios;
using Moq;

namespace Tests.TestsServicios
{
    [TestClass]
    public class AuditoriaServicioTests
    {
        private IAuditoriaServicio _auditoriaServicio;
        private Mock<IAuditoriaRepositorio> _repoMock;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IAuditoriaRepositorio>();
            _auditoriaServicio = new AuditoriaServicio(_repoMock.Object);
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }

        [TestMethod]
        public void Registrar_AccionValidaConUsuario_AgregaLogCorrectamente()
        {
            var usuario = CrearUsuarioValido();

            _auditoriaServicio.Registrar("Alta de equipo", usuario);

            _repoMock.Verify(r => r.Agregar(It.IsAny<LogAuditoria>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Registrar_AccionNula_LanzaExcepcion()
        {
            var usuario = CrearUsuarioValido();

            _auditoriaServicio.Registrar(null, usuario);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Registrar_UsuarioNulo_LanzaExcepcion()
        {
            _auditoriaServicio.Registrar("Alta de equipo", null);
        }

        [TestMethod]
        public void ObtenerTodos_ListaVacia_RetornaListaVacia()
        {
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(new List<LogAuditoria>());

            var resultado = _auditoriaServicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ObtenerTodos_ConLogs_RetornaListaCorrecta()
        {
            var logs = new List<LogAuditoria> { new LogAuditoria() };
            _repoMock.Setup(r => r.ObtenerTodos()).Returns(logs);

            var resultado = _auditoriaServicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }
    }
}