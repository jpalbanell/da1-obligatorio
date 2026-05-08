using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ImportacionFacilidadComprensionServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio = null!;
        private IImportacionServicio _servicio = null!;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _servicio = new ImportacionFacilidadComprensionServicio(_equipoRepositorio);
        }

        [TestMethod]
        public void ImportarEquipos_ConUnEquipoValido_DeberiaImportarlo()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConVariosEquiposValidos_DeberiaImportarlos()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nArgentina,CONMEBOL,2000";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConConfederacionInvalida_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,INVALIDA,1500";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConRankingFueraDeRango_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,9999";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConNombreDuplicado_DeberiaRegistrarError()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nUruguay,CONMEBOL,1600";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConFilaInvalidaEntreValidas_DeberiaImportarLasValidas()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500\nMalo,INVALIDA,1500\nArgentina,CONMEBOL,2000";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(2, resultado.EquiposImportados);
            Assert.AreEqual(1, resultado.Errores.Count);
        }
        
        [TestMethod]
        public void ImportarEquipos_ConSoloEncabezado_DeberiaRetornarCeroImportados()
        {
            var csv = "Nombre,Confederación,RankingFIFA";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(0, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
    }
}