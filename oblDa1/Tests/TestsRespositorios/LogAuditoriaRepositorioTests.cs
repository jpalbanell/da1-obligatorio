using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class LogAuditoriaRepositorioTests
    {
        private ILogAuditoriaRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new LogAuditoriaRepositorio();
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Pérez";
            usuario.Email = "juan@ejemplo.com";
            return usuario;
        }

        private LogAuditoria CrearLogValido(string accion)
        {
            var log = new LogAuditoria();
            log.Id = 1;
            log.Timestamp = DateTime.Now;
            log.Accion = accion;
            log.Usuario = CrearUsuarioValido();
            return log;
        }

        [TestMethod]
        public void Agregar_ConLogValido_DeberiaPoderObtenerPorId()
        {
            var log = CrearLogValido("Alta de equipo: Uruguay");

            _repositorio.Agregar(log);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.AreEqual(log, resultado);
        }
    }
}