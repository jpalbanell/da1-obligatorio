using System.Text;
using Dominio.Entidades;
using IServicios;
using Moq;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ExportacionServicioTests
    {
        private Mock<ITorneoServicio> _torneoMock;
        private Mock<IAuditoriaServicio> _auditoriaMock;
        private Mock<ISesionServicio> _sesionMock;
        private ExportacionServicio _exportacionServicio;

        [TestInitialize]
        public void Setup()
        {
            _torneoMock = new Mock<ITorneoServicio>();
            _auditoriaMock = new Mock<IAuditoriaServicio>();
            _sesionMock = new Mock<ISesionServicio>();
            _exportacionServicio = new ExportacionServicio(
                _torneoMock.Object, _auditoriaMock.Object, _sesionMock.Object);
        }

        [TestMethod]
        public void ExportarFixture_EnCsv_GeneraElCsvDeLosPartidos()
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
            _torneoMock.Setup(t => t.ObtenerTodosPartidos())
                .Returns(new List<Partido> { partido });

            byte[] resultado = _exportacionServicio.ExportarFixture(FormatoExportacion.CSV);

            string csv = Encoding.UTF8.GetString(resultado);
            StringAssert.Contains(csv, "Codigo,Fase,Local,Visitante");
            StringAssert.Contains(csv, "A1,FaseGrupos,Uruguay,Alemania,2,1");
        }
    }
}