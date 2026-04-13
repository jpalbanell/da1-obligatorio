using Dominio;

namespace Tests
{
    [TestClass]
    public class EquipoTests
    {
        [TestMethod]
        public void CrearEquipo_ConNombreValido_DeberiaAsignarNombre()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            Assert.AreEqual("Uruguay", equipo.Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearEquipo_ConNombreVacio_DeberiaLanzarExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "";
        }

    }
}