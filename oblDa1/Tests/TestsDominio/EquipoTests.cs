using Dominio.Entidades;

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
        
        [TestMethod]
        public void CrearEquipo_ConConfederacionValida_DeberiaAsignar()
        {
            var equipo = new Equipo();
            equipo.Confederacion = Confederacion.CONMEBOL;
            Assert.AreEqual(Confederacion.CONMEBOL, equipo.Confederacion);
        }
        
        [TestMethod]
        public void CrearEquipo_ConRankingValido_DeberiaAsignar()
        {
            var equipo = new Equipo();
            equipo.RankingFifa = 1500;
            Assert.AreEqual(1500, equipo.RankingFifa);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearEquipo_ConRankingMenorA300_DeberiaLanzarExcepcion()
        {
            var equipo = new Equipo();
            equipo.RankingFifa = 299;
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearEquipo_ConRankingMayorA2500_DeberiaLanzarExcepcion()
        {
            var equipo = new Equipo();
            equipo.RankingFifa = 2501;
        }
        
        [TestMethod]
        public void CrearEquipo_ConRankingLimiteInferior_DeberiaAsignar()
        {
            var equipo = new Equipo();
            equipo.RankingFifa = 300;
            Assert.AreEqual(300, equipo.RankingFifa);
        }

        [TestMethod]
        public void CrearEquipo_ConRankingLimiteSuperior_DeberiaAsignar()
        {
            var equipo = new Equipo();
            equipo.RankingFifa = 2500;
            Assert.AreEqual(2500, equipo.RankingFifa);
        }
        [TestMethod]
        public void CrearEquipo_ConBanderaBase64Valida_DeberiaAsignar()
        {
            var equipo = new Equipo();
            var base64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==";
            equipo.Bandera = base64;
            Assert.AreEqual(base64, equipo.Bandera);
        }

        [TestMethod]
        public void CrearEquipo_SinAsignarBandera_DeberiaSerNula()
        {
            var equipo = new Equipo();
            Assert.IsNull(equipo.Bandera);
        }
        
        [TestMethod]
        public void CalcularNuevoRanking_GanaElDebilEnFaseGrupos_SubeMucho()
        {
            var equipo = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1200 };

            int nuevo = equipo.CalcularNuevoRanking(1500, 1.0, 1.0);

            Assert.AreEqual(1220, nuevo);
        }
        
        [TestMethod]
        public void CalcularNuevoRanking_GanaElFavoritoEnFaseGrupos_SubePoco()
        {
            var equipo = new Equipo { Nombre = "Alemania", Confederacion = Confederacion.UEFA, RankingFifa = 1500 };

            int nuevo = equipo.CalcularNuevoRanking(1200, 1.0, 1.0);

            Assert.AreEqual(1510, nuevo);
        }
        
        [TestMethod]
        public void CalcularNuevoRanking_GanaEnEliminatorias_CambiaMasQueEnGrupos()
        {
            var equipo = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1200 };

            int nuevo = equipo.CalcularNuevoRanking(1500, 1.0, 1.5);

            Assert.AreEqual(1230, nuevo);
        }
        
        [TestMethod]
        public void CalcularNuevoRanking_EquipoEnElMaximo_NoSuperaElTope()
        {
            var equipo = new Equipo { Nombre = "Brasil", Confederacion = Confederacion.CONMEBOL, RankingFifa = 2500 };

            int nuevo = equipo.CalcularNuevoRanking(1000, 1.0, 1.5);

            Assert.AreEqual(2500, nuevo);
        }

        [TestMethod]
        public void CalcularNuevoRanking_EquipoEnElMinimo_NoBajaDelPiso()
        {
            var equipo = new Equipo { Nombre = "SanMarino", Confederacion = Confederacion.UEFA, RankingFifa = 300 };

            int nuevo = equipo.CalcularNuevoRanking(1500, 0.0, 1.5);

            Assert.AreEqual(300, nuevo);
        }
    }
}