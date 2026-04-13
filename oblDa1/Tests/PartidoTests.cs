using Dominio;

namespace Tests
{
    [TestClass]
    public class PartidoTests
    {
        [TestMethod]
        public void CrearPartido_ConIdValido_AsignaIdCorrectamente()
        {
            var partido = new Partido(1);
            Assert.AreEqual(1, partido.Id);
        }

        [TestMethod]
        public void CrearPartido_ConCodigoValido_AsignaCodigoCorrectamente()
        {
            var partido = new Partido(1);
            partido.Codigo = "P001";
            Assert.AreEqual("P001", partido.Codigo);
        }

        [TestMethod]
        public void CrearPartido_ConFechaValida_AsignaFechaCorrectamente()
        {
            var partido = new Partido(1);
            partido.Fecha = new DateTime(2026, 6, 1);
            Assert.AreEqual(new DateTime(2026, 6, 1), partido.Fecha);
        }

        [TestMethod]
        public void CrearPartido_ConFaseValida_AsignaFaseCorrectamente()
        {
            var partido = new Partido(1);
            partido.Fase = FaseTorneo.FaseGrupos;
            Assert.AreEqual(FaseTorneo.FaseGrupos, partido.Fase);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConCodigoNulo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Codigo = null;
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConCodigoVacio_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Codigo = "";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConFechaVacia_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Fecha = default;
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoLocalValido_AsignaEquipoLocalCorrectamente()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
    
            partido.EquipoLocal = equipo;
    
            Assert.AreEqual(equipo, partido.EquipoLocal);
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoVisitanteValido_AsignaEquipoVisitanteCorrectamente()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Argentina";
    
            partido.EquipoVisitante = equipo;
    
            Assert.AreEqual(equipo, partido.EquipoVisitante);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConEquipoLocalNulo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = null;
        }
        
        
    }
}