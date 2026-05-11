using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class FixtureRepositorioTests
    {
        private IFixtureRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new FixtureRepositorio();
        }

        [TestMethod]
        public void Guardar_ConFixtureValido_DeberiaPoderObtenerlo()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _repositorio.Guardar(fixture);
            var resultado = _repositorio.Obtener();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(42, resultado.SemillaFixture);
        }
        
        [TestMethod]
        public void Obtener_SinHaberGuardado_DeberiaRetornarNull()
        {
            var resultado = _repositorio.Obtener();
            Assert.IsNull(resultado);
        }
    }
}