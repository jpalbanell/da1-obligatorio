using Dominio.Entidades;
using IRepositorios;
using IServicios;
using Servicios;
using Moq;

namespace Tests
{
    [TestClass]
    public class ImportacionServicioTests
    {
        private ImportacionServicio _importacionServicio;
        private Mock<IEquipoRepositorio> _equipoRepoMock;
        private Mock<IFixtureRepositorio> _fixtureRepoMock;
        private Mock<IAuditoriaServicio> _auditoriaMock;
        private Mock<ISesionServicio> _sesionMock;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepoMock = new Mock<IEquipoRepositorio>();
            _fixtureRepoMock = new Mock<IFixtureRepositorio>();
            _auditoriaMock = new Mock<IAuditoriaServicio>();
            _sesionMock = new Mock<ISesionServicio>();

            _equipoRepoMock.Setup(r => r.ObtenerTodos()).Returns(new List<Equipo>());
            _fixtureRepoMock.Setup(r => r.Obtener()).Returns((Fixture)null);

            var adminActual = new Usuario
            {
                Nombre = "Admin", Apellido = "Test", Email = "admin@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1), Contrasena = "Password@1"
            };
            adminActual.Roles.Add(Rol.Editor);
            _sesionMock.Setup(s => s.ObtenerUsuarioActual()).Returns(adminActual);

            _importacionServicio = new ImportacionServicio(
                _equipoRepoMock.Object, _fixtureRepoMock.Object,
                _auditoriaMock.Object, _sesionMock.Object);
        }

        [TestMethod]
        public void ImportarEquipos_CsvValido_ImportaCorrectamente()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nArgentina,CONMEBOL,1600";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_FilaConError_RegistraError()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nMal,ConfederacionInvalida,999";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_NombreDuplicado_RegistraError()
        {
            var fixtureCompartido = new Fixture();
            _fixtureRepoMock.Setup(r => r.Obtener()).Returns(fixtureCompartido);
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nUruguay,CONMEBOL,1600";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void ImportarEquipos_SinRolEditor_LanzaExcepcion()
        {
            _sesionMock.Setup(s => s.ValidarRol(Rol.Editor)).Throws<UnauthorizedAccessException>();

            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500";
            _importacionServicio.ImportarEquipos(csv);
        }

        [TestMethod]
        public void ImportarEquipos_ConRankingFueraDeRango_RegistraError()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,9999";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_ConFilaInvalidaEntreValidas_ImportaLasValidas()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nMalo,INVALIDA,1500\nArgentina,CONMEBOL,2000";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_ConSoloEncabezado_RetornaCeroImportados()
        {
            var csv = "Nombre,Confederacion,RankingFifa";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_RegistraAuditoria()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500";

            _importacionServicio.ImportarEquipos(csv);

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("Importación")),
                It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void ImportarEquipos_ConErrores_RegistraAuditoriaConErrores()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500\nMalo,INVALIDA,1500";

            _importacionServicio.ImportarEquipos(csv);

            _auditoriaMock.Verify(a => a.Registrar(
                It.Is<string>(s => s.Contains("1 errores")),
                It.IsAny<Usuario>()), Times.Once);
        }

        [TestMethod]
        public void ImportarEquipos_ConRankingFueraDeRangoInferior_RegistraError()
        {
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,100";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_ExcediendoCupoConfederacion_RegistraError()
        {
            var fixtureCompartido = new Fixture();
            _fixtureRepoMock.Setup(r => r.Obtener()).Returns(fixtureCompartido);
            var csv = "Nombre,Confederacion,RankingFifa\nOFC_A,OFC,1500\nOFC_B,OFC,1600";

            var resultado = _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }

        [TestMethod]
        public void ImportarEquipos_ConColumnaBandera_ImportaBandera()
        {
            Equipo equipoCapturado = null;
            _equipoRepoMock.Setup(r => r.Agregar(It.IsAny<Equipo>()))
                           .Callback<Equipo>(e => equipoCapturado = e);
            var csv = "Nombre,Confederacion,RankingFifa,Bandera\nUruguay,CONMEBOL,1500,base64string==";

            _importacionServicio.ImportarEquipos(csv);

            Assert.IsNotNull(equipoCapturado);
            Assert.AreEqual("base64string==", equipoCapturado.Bandera);
        }

        [TestMethod]
        public void ImportarEquipos_ConColumnaBanderaVacia_BanderaEsNull()
        {
            Equipo equipoCapturado = null;
            _equipoRepoMock.Setup(r => r.Agregar(It.IsAny<Equipo>()))
                           .Callback<Equipo>(e => equipoCapturado = e);
            var csv = "Nombre,Confederacion,RankingFifa,Bandera\nUruguay,CONMEBOL,1500,";

            _importacionServicio.ImportarEquipos(csv);

            Assert.IsNotNull(equipoCapturado);
            Assert.IsNull(equipoCapturado.Bandera);
        }

        [TestMethod]
        public void ImportarEquipos_SinColumnaBandera_BanderaEsNull()
        {
            Equipo equipoCapturado = null;
            _equipoRepoMock.Setup(r => r.Agregar(It.IsAny<Equipo>()))
                           .Callback<Equipo>(e => equipoCapturado = e);
            var csv = "Nombre,Confederacion,RankingFifa\nUruguay,CONMEBOL,1500";

            _importacionServicio.ImportarEquipos(csv);

            Assert.IsNotNull(equipoCapturado);
            Assert.IsNull(equipoCapturado.Bandera);
        }

        [TestMethod]
        public void ImportarEquipos_FilasMixtas_AlgunasConBanderaOtrasSin()
        {
            var equiposCapturados = new List<Equipo>();
            _equipoRepoMock.Setup(r => r.Agregar(It.IsAny<Equipo>()))
                           .Callback<Equipo>(e => equiposCapturados.Add(e));
            var csv = "Nombre,Confederacion,RankingFifa,Bandera\nUruguay,CONMEBOL,1500,base64==\nArgentina,CONMEBOL,1600,";

            _importacionServicio.ImportarEquipos(csv);

            Assert.AreEqual(2, equiposCapturados.Count);
            Assert.AreEqual("base64==", equiposCapturados[0].Bandera);
            Assert.IsNull(equiposCapturados[1].Bandera);
        }
    }
}
