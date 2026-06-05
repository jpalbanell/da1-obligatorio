using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class NotificacionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_MensajeNulo_LanzaExcepcion()
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = null;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_MensajeVacio_LanzaExcepcion()
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = "   ";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_MensajeMayorA500Caracteres_LanzaExcepcion()
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = new string('a', 501);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_FechaCreacionPorDefecto_LanzaExcepcion()
        {
            var notificacion = new Notificacion();
            notificacion.FechaCreacion = default;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Crear_PeriodistaNull_LanzaExcepcion()
        {
            var notificacion = new Notificacion();
            notificacion.Periodista = null;
        }

        [TestMethod]
        public void MarcarLeida_NotificacionNoLeida_CambiaLeidaATrue()
        {
            var notificacion = new Notificacion();
            notificacion.MarcarLeida();
            Assert.IsTrue(notificacion.Leida);
        }
    }
}
