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
    }
}
