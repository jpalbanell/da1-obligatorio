using Dominio.Entidades;
using Servicios.Exportacion;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ConversorFixtureTests
    {
        [TestMethod]
        public void ATabla_ConUnPartidoCompleto_GeneraEncabezadosYUnaFila()
        {
            var local = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1500 };
            var visitante = new Equipo { Nombre = "Alemania", Confederacion = Confederacion.UEFA, RankingFifa = 1600 };
            var estadio = new Estadio { Nombre = "Centenario", Ciudad = "Montevideo", Capacidad = 25000 };
            var partido = new Partido
            {
                Codigo = "A1",
                Fecha = new DateTime(2026, 6, 15, 18, 0, 0),
                Fase = FaseTorneo.FaseGrupos,
                EquipoLocal = local,
                EquipoVisitante = visitante,
                Estadio = estadio,
                GolesLocal = 2,
                GolesVisitante = 1
            };
            var partidos = new List<Partido> { partido };

            var tabla = ConversorFixture.ATabla(partidos);

            CollectionAssert.AreEqual(
                new List<string> { "Codigo", "Fase", "Local", "Visitante", "GolesLocal", "GolesVisitante", "Fecha", "Estadio" },
                tabla.Encabezados);
            Assert.AreEqual(1, tabla.Filas.Count);
            CollectionAssert.AreEqual(
                new List<string> { "A1", "FaseGrupos", "Uruguay", "Alemania", "2", "1", "2026-06-15 18:00:00", "Centenario" },
                tabla.Filas[0]);
        }
        
        [TestMethod]
        public void ATabla_ConPartidoSinEquiposDefinidos_MuestraPorDefinir()
        {
            var estadio = new Estadio { Nombre = "Centenario", Ciudad = "Montevideo", Capacidad = 25000 };
            var partido = new Partido
            {
                Codigo = "OF-1",
                Fecha = new DateTime(2026, 7, 1, 20, 0, 0),
                Fase = FaseTorneo.Octavos,
                Estadio = estadio
            };
            var partidos = new List<Partido> { partido };

            var tabla = ConversorFixture.ATabla(partidos);

            Assert.AreEqual(1, tabla.Filas.Count);
            Assert.AreEqual("Por definir", tabla.Filas[0][2]); // columna Local
            Assert.AreEqual("Por definir", tabla.Filas[0][3]); // columna Visitante
        }
    }
}