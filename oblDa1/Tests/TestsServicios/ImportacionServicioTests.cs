using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class ImportacionServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio;
        private IImportacionServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _servicio = new ImportacionServicio(_equipoRepositorio);
        }

        [TestMethod]
        public void ImportarEquipos_ConUnEquipoValido_DeberiaImportarlo()
        {
            var csv = "Nombre,Confederación,RankingFIFA\nUruguay,CONMEBOL,1500";

            var resultado = _servicio.ImportarEquipos(csv);

            Assert.AreEqual(1, resultado.EquiposImportados);
            Assert.AreEqual(0, resultado.Errores.Count);
        }
    }
}