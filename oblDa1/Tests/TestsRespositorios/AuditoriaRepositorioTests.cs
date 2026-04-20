using Dominio.Entidades;
using Repositorios;

namespace Tests.TestsRepositorios
{
    [TestClass]
    public class AuditoriaRepositorioTests
    {
        private IAuditoriaRepositorio _auditoriaRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _auditoriaRepositorio = new AuditoriaRepositorio();
        }
        
        [TestMethod]
        public void Agregar_LogValido_NoLanzaExcepcion()
        {
            var log = new LogAuditoria();
            log.Timestamp = DateTime.Now;
            log.Accion = "Alta de equipo";
            log.Usuario = new Usuario();

            _auditoriaRepositorio.Agregar(log);
        }
    }
}