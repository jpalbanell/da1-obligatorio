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
        
        [TestMethod]
        public void ObtenerTodos_ConLogsAgregados_DeberiaRetornarTodos()
        {
            var log1 = CrearLogValido("Alta de equipo: Uruguay");
            log1.Id = 1;
            var log2 = CrearLogValido("Alta de equipo: Argentina");
            log2.Id = 2;

            _repositorio.Agregar(log1);
            _repositorio.Agregar(log2);
            var resultado = _repositorio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerTodos_SinLogs_DeberiaRetornarListaVacia()
        {
            var resultado = _repositorio.ObtenerTodos();
            Assert.AreEqual(0, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorId_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _repositorio.ObtenerPorId(999);
            Assert.IsNull(resultado);
        }
    }
}