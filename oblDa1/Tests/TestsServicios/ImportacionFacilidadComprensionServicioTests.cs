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
    }
}