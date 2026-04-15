using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class GrupoTests
    {
        [TestMethod]
        public void CrearGrupo_ConEtiquetaValida_DeberiaAsignarEtiqueta()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            Assert.AreEqual("A", grupo.Etiqueta);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearGrupo_ConEtiquetaInvalida_DeberiaLanzarExcepcion()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "Z";
        }
        
        [TestMethod]
        public void CrearGrupo_ListaPartidosDeberiaEstarVacia()
        {
            var grupo = new Grupo();
            Assert.IsNotNull(grupo.ListaPartidos);
            Assert.AreEqual(0, grupo.ListaPartidos.Count);
        }
        
        [TestMethod]
        public void CrearGrupo_ListaPosicionesDeberiaEstarVacia()
        {
            var grupo = new Grupo();
            Assert.IsNotNull(grupo.ListaPosiciones);
            Assert.AreEqual(0, grupo.ListaPosiciones.Count);
        }
    }
}