using IServicios;
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

        [TestMethod]
        public void MotorProbabilistico_Nombre_EsProbabilistico()
        {
            var motor = new MotorProbabilistico();

            Assert.AreEqual("Probabilístico", motor.Nombre);
        }

        [TestMethod]
        public void MotorProbabilistico_Simular_ConSemillaFija_RetornaGolesEsperados()
        {
            var motor = new MotorProbabilistico();
            var random = new Random(42);

            var (golesLocal, golesVisitante) = motor.Simular(1500, 2000, random);

            Assert.AreEqual(2, golesLocal);
            Assert.AreEqual(0, golesVisitante);
        }

        [TestMethod]
        public void MotorProbabilistico_Simular_RankingAlto_ProduceMasGolesEnPromedioQueRankingBajo()
        {
            var motor = new MotorProbabilistico();
            int sumaAlto = 0, sumaBajo = 0;

            for (int semilla = 0; semilla < 50; semilla++)
            {
                var (golesAlto, _) = motor.Simular(2500, 300, new Random(semilla));
                var (golesBajo, _) = motor.Simular(300, 2500, new Random(semilla));
                sumaAlto += golesAlto;
                sumaBajo += golesBajo;
            }

            Assert.IsTrue(sumaAlto > sumaBajo);
        }
    }
}
