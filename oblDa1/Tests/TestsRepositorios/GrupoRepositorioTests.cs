using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests.TestsRepositorios
{
    [TestClass]
    public class GrupoRepositorioTests
    {
        private IGrupoRepositorio _grupoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);
            _grupoRepositorio = new GrupoRepositorio(context);
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

        [TestMethod]
        public void ObtenerPorEtiqueta_EtiquetaInexistente_RetornaNull()
        {
            var resultado = _grupoRepositorio.ObtenerPorEtiqueta("A");

            Assert.IsNull(resultado);
        }
    }
}
