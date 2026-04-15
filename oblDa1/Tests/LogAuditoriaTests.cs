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
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearLogAuditoria_ConTimestampVacio_DeberiaLanzarExcepcion()
        {
            var log = new LogAuditoria();
            log.Timestamp = default;
        }
        
        [TestMethod]
        public void CrearLogAuditoria_ConAccionValida_DeberiaAsignarAccion()
        {
            var log = new LogAuditoria();
            log.Accion = "Alta de equipo: Uruguay";
            Assert.AreEqual("Alta de equipo: Uruguay", log.Accion);
        }
    }
}