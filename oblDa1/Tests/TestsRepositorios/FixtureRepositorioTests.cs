using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class FixtureRepositorioTests
    {
        private SqlContext _context;
        private IFixtureRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new SqlContext(options);
            _repositorio = new FixtureRepositorio(_context);
        }

        [TestMethod]
        public void Guardar_FixtureNuevo_LoInserta()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _repositorio.Guardar(fixture);

            Assert.AreEqual(1, _context.Fixtures.Count());
        }

        [TestMethod]
        public void Guardar_FixtureExistente_LoActualiza()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            _repositorio.Guardar(fixture);

            var fixtureActualizado = new Fixture();
            fixtureActualizado.SemillaFixture = 99;
            _repositorio.Guardar(fixtureActualizado);

            Assert.AreEqual(1, _context.Fixtures.Count());
            Assert.AreEqual(99, _repositorio.Obtener().SemillaFixture);
        }

        [TestMethod]
        public void Obtener_SinFixtureGuardado_RetornaNull()
        {
            var resultado = _repositorio.Obtener();

            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void Obtener_ConFixtureGuardado_RetornaFixture()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            _repositorio.Guardar(fixture);

            var resultado = _repositorio.Obtener();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(42, resultado.SemillaFixture);
        }
    }
}
