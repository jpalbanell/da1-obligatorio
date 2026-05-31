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
        
        [TestMethod]
        public void EliminarEquipo_Existe_EliminaCorrectamente()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));

            fixture.EliminarEquipo("Uruguay");

            Assert.AreEqual(0, fixture.Equipos.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEquipo_NoExiste_LanzaExcepcion()
        {
            var fixture = new Fixture();

            fixture.EliminarEquipo("Uruguay");
        }
        
        [TestMethod]
        public void EditarEquipo_NombreValido_EditaCorrectamente()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            var equipoEditado = CrearEquipoValido("Uruguay2");

            fixture.EditarEquipo(equipoEditado, "Uruguay");

            Assert.AreEqual("Uruguay2", fixture.Equipos[0].Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            fixture.AgregarEquipo(CrearEquipoValido("Argentina"));
            var equipoEditado = CrearEquipoValido("Argentina");

            fixture.EditarEquipo(equipoEditado, "Uruguay");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditarEquipo_CambioConfederacionSinCupo_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("OFC_1", Confederacion.OFC));
            fixture.AgregarEquipo(CrearEquipoValido("CAF_1", Confederacion.CAF));
            var equipoEditado = CrearEquipoValido("CAF_1", Confederacion.OFC);

            fixture.EditarEquipo(equipoEditado, "CAF_1");
        }
        
        [TestMethod]
        public void AgregarEstadio_NombreUnico_AgregaCorrectamente()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            fixture.AgregarEstadio(estadio);

            Assert.AreEqual(1, fixture.Estadios.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEstadio_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            fixture.AgregarEstadio(estadio);
            fixture.AgregarEstadio(estadio);
        }
        
        [TestMethod]
        public void EliminarEstadio_Existe_EliminaCorrectamente()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;
            fixture.AgregarEstadio(estadio);

            fixture.EliminarEstadio("Centenario");

            Assert.AreEqual(0, fixture.Estadios.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEstadio_NoExiste_LanzaExcepcion()
        {
            var fixture = new Fixture();

            fixture.EliminarEstadio("Centenario");
        }
    }
}