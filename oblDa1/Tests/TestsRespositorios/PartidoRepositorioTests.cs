using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class PartidoRepositorioTests
    {
        private IPartidoRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new PartidoRepositorio();
        }

        [TestMethod]
        public void Agregar_ConPartidoValido_DeberiaPoderObtenerPorId()
        {
            var partido = new Partido(1);

            _repositorio.Agregar(partido);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.AreEqual(partido, resultado);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConPartidosAgregados_DeberiaRetornarTodos()
        {
            var partido1 = new Partido(1);
            var partido2 = new Partido(2);

            _repositorio.Agregar(partido1);
            _repositorio.Agregar(partido2);
            var resultado = _repositorio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerTodos_SinPartidos_DeberiaRetornarListaVacia()
        {
            var resultado = _repositorio.ObtenerTodos();
            Assert.AreEqual(0, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorId_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _repositorio.ObtenerPorId(999);
            Assert.IsNull(resultado);
        }
    }
}