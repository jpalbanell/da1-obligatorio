
using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class IncidenciaTests
    {
        private Equipo CrearEquipo() => new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };

        [TestMethod]
        public void CrearIncidencia_ConTipoYEquipoValidos_DeberiaCrearseCorrectamente()
        {
            var incidencia = new Incidencia();
            incidencia.Tipo = TipoIncidencia.TarjetaAmarilla;
            incidencia.Equipo = CrearEquipo();
            incidencia.Cantidad = 1;

            Assert.AreEqual(TipoIncidencia.TarjetaAmarilla, incidencia.Tipo);
        }
    }
}
