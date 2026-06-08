using IServicios;
using Servicios;
using Servicios.Simulacion;

namespace Tests
{
    [TestClass]
    public class MotorSimulacionSelectorTests
    {
        [TestMethod]
        public void Obtener_NombreValido_RetornaMotorCorrecto()
        {
            var motores = new List<IMotorSimulacion> { new MotorAleatorio(), new MotorProbabilistico() };
            var factory = new MotorSimulacionSelector(motores);

            var motor = factory.Obtener("Probabilístico");

            Assert.IsInstanceOfType(motor, typeof(MotorProbabilistico));
        }
    }
}
