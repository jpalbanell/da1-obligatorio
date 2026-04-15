using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class LogAuditoriaTests
    {
        [TestMethod]
        public void CrearLogAuditoria_ConIdValido_DeberiaAsignarId()
        {
            var log = new LogAuditoria();
            log.Id = 1;
            Assert.AreEqual(1, log.Id);
        }
    }
}