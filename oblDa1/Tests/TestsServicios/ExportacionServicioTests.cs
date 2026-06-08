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
        
        [TestMethod]
        public void ExportarAuditoria_EnCsv_GeneraElCsvDeLosLogs()
        {
            var usuario = new Usuario
            {
                Nombre = "Admin", Apellido = "Test", Email = "admin@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1), Contrasena = "Password@1"
            };
            var log = new LogAuditoria
            {
                Timestamp = new DateTime(2026, 6, 5, 14, 30, 0),
                Accion = "Creacion de usuario",
                Usuario = usuario
            };
            _auditoriaMock.Setup(a => a.ObtenerEntreFechas(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<LogAuditoria> { log });

            byte[] resultado = _exportacionServicio.ExportarAuditoria(
                FormatoExportacion.CSV, new DateTime(2026, 6, 1), new DateTime(2026, 6, 30));

            string csv = Encoding.UTF8.GetString(resultado);
            StringAssert.Contains(csv, "Fecha,Accion,Usuario");
            StringAssert.Contains(csv, "Creacion de usuario,admin@test.com");
        }
        
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ExportarFixture_SinRolPermitido_Lanza()
        {
            _sesionMock.Setup(s => s.ValidarAlgunRol(It.IsAny<Rol[]>()))
                .Throws(new UnauthorizedAccessException());

            _exportacionServicio.ExportarFixture(FormatoExportacion.CSV);
        }
        
        [TestMethod]
        public void ExportarFixture_Audita()
        {
            var usuario = new Usuario
            {
                Nombre = "Admin", Apellido = "Test", Email = "admin@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1), Contrasena = "Password@1"
            };
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(usuario);
            _torneoMock.Setup(t => t.ObtenerTodosPartidos()).Returns(new List<Partido>());

            _exportacionServicio.ExportarFixture(FormatoExportacion.CSV);

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("xporta") && s.Contains("ixture")),
                usuario), Times.Once);
        }
        
        [TestMethod]
        public void ExportarAuditoria_Audita()
        {
            var usuario = new Usuario
            {
                Nombre = "Admin", Apellido = "Test", Email = "admin@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1), Contrasena = "Password@1"
            };
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(usuario);
            _auditoriaMock.Setup(a => a.ObtenerEntreFechas(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<LogAuditoria>());

            _exportacionServicio.ExportarAuditoria(
                FormatoExportacion.XLSX, new DateTime(2026, 6, 1), new DateTime(2026, 6, 30));

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("xporta") && s.Contains("uditor")),
                usuario), Times.Once);
        }
    }
}