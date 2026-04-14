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
    }
}