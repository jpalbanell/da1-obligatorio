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
            var selector = new MotorSimulacionSelector(motores);

            var motor = selector.Obtener("Probabilístico");

            Assert.IsInstanceOfType(motor, typeof(MotorProbabilistico));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Obtener_NombreInvalido_LanzaArgumentException()
        {
            var selector = new MotorSimulacionSelector(new List<IMotorSimulacion> { new MotorProbabilistico() });

            selector.Obtener("NoExiste");
        }

        [TestMethod]
        public void ObtenerNombres_RetornaTodosLosNombres()
        {
            var selector = new MotorSimulacionSelector(
                new List<IMotorSimulacion> { new MotorAleatorio(), new MotorProbabilistico() });

            var nombres = selector.ObtenerNombres().ToList();

            CollectionAssert.Contains(nombres, "Aleatorio Puro");
            CollectionAssert.Contains(nombres, "Probabilístico");
        }
    }
}
