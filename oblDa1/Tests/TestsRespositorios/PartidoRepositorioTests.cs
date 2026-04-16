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
    }
}