using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class FixtureTests
    {
        [TestMethod]
        public void CrearFixture_ConSemillaValida_DeberiaAsignarSemilla()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            Assert.AreEqual(42, fixture.SemillaFixture);
        }
        
        [TestMethod]
        public void CrearFixture_SinAsignarFecha_DeberiaSerPrimerDeJunio2026()
        {
            var fixture = new Fixture();
            Assert.AreEqual(new DateTime(2026, 6, 1), fixture.FechaInicioTorneo);
        }
        
        [TestMethod]
        public void CrearFixture_SinAsignarMaxPartidos_DeberiaSerTres()
        {
            var fixture = new Fixture();
            Assert.AreEqual(3, fixture.MaxPartidosPorDia);
        }
    }
}