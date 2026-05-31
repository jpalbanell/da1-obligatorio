using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class FixtureTests
    {
        private Equipo CrearEquipoValido(string nombre, Confederacion confederacion = Confederacion.CAF)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.Confederacion = confederacion;
            equipo.RankingFifa = 500;
            return equipo;
        }
        
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
        
        [TestMethod]
        public void CrearFixture_SinAsignarSeparacion_DeberiaSerTres()
        {
            var fixture = new Fixture();
            Assert.AreEqual(3, fixture.SeparacionEntreFechas);
        }
        
        [TestMethod]
        public void CrearFixture_SinGenerar_EstaGeneradoDeberiaSerFalse()
        {
            var fixture = new Fixture();
            Assert.IsFalse(fixture.EstaGenerado);
        }
        
        [TestMethod]
        public void AgregarEquipo_FixtureVacio_AgregaCorrectamente()
        {
            var fixture = new Fixture();
            var equipo = CrearEquipoValido("Uruguay");

            fixture.AgregarEquipo(equipo);

            Assert.AreEqual(1, fixture.Equipos.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_CupoConfederacionCompleto_LanzaExcepcion()
        {
            var fixture = new Fixture();
            for (int i = 0; i < Confederacion.OFC.CupoMaximo(); i++)
                fixture.AgregarEquipo(CrearEquipoValido($"OFC_{i}", Confederacion.OFC));

            fixture.AgregarEquipo(CrearEquipoValido("OFC_Extra", Confederacion.OFC));
        }
        
        
    }
}