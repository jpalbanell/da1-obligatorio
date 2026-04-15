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
        
        [TestMethod]
        public void CrearLogAuditoria_ConTimestampValido_DeberiaAsignarTimestamp()
        {
            var log = new LogAuditoria();
            var fecha = new DateTime(2026, 4, 15, 10, 30, 0);
            log.Timestamp = fecha;
            Assert.AreEqual(fecha, log.Timestamp);
        }
    }
}