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
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearEquipo_ConNombreNulo_DeberiaLanzarExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = null;
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearEquipo_ConNombreMasDe60Chars_DeberiaLanzarExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = new string('A', 61);
        }
        
        [TestMethod]
        public void CrearEquipo_ConNombreDe60Chars_DeberiaAsignarNombre()
        {
            var equipo = new Equipo();
            var nombre = new string('A', 60);
            equipo.Nombre = nombre;
            Assert.AreEqual(nombre, equipo.Nombre);
        }
    }
}