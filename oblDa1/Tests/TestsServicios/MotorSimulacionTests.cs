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

        [TestMethod]
        public void MotorAleatorio_Simular_ConSemillaFija_RetornaGolesEsperados()
        {
            var motor = new MotorAleatorio();
            var random = new Random(42);

            var (golesLocal, golesVisitante) = motor.Simular(0, 0, random);

            Assert.AreEqual(4, golesLocal);
            Assert.AreEqual(0, golesVisitante);
        }
    }
}
