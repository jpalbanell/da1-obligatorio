using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class ImportacionBajoAcoplamientoServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio;
        private IImportacionServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _servicio = new ImportacionBajoAcoplamientoServicio(
                equipo => _equipoRepositorio.Agregar(equipo),
                nombre => _equipoRepositorio.ObtenerPorNombre(nombre) != null
            );
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
    }
}