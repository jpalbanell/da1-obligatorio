using Dominio;
using Servicios.Simulacion;

namespace Tests
{
    [TestClass]
    public class MotorSimulacionTests
    {
        [TestMethod]
        public void MotorAleatorio_Nombre_EsAleatorioPuro()
        {
            var motor = new MotorAleatorio();

            Assert.AreEqual("Aleatorio Puro", motor.Nombre);
        }
    }
}
