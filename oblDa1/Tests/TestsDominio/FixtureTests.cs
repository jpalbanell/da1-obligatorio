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
    }
}