using Dominio.Entidades;
using Repositorios;

namespace Tests.TestsRepositorios
{
    [TestClass]
    public class GrupoRepositorioTests
    {
        private IGrupoRepositorio _grupoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _grupoRepositorio = new GrupoRepositorio();
        }

        [TestMethod]
        public void Agregar_GrupoValido_ObtenerTodosRetornaUnGrupo()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            _grupoRepositorio.Agregar(grupo);

            Assert.AreEqual(1, _grupoRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void Agregar_GrupoValido_ObtenerPorEtiquetaRetornaGrupoCorrecto()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            _grupoRepositorio.Agregar(grupo);

            Assert.AreEqual(grupo, _grupoRepositorio.ObtenerPorEtiqueta("A"));
        }
    }
}