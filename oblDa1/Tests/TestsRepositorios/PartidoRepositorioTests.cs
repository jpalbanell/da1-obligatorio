using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class PartidoRepositorioTests
    {
        private IPartidoRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);
            _repositorio = new PartidoRepositorio(context);
        }

        private static Partido CrearPartido(int id)
        {
            var p = new Partido(id);
            p.Codigo = $"P{id}";
            return p;
        }

        [TestMethod]
        public void Agregar_ConPartidoValido_DeberiaPoderObtenerPorId()
        {
            var partido = CrearPartido(1);

            _repositorio.Agregar(partido);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.AreEqual(partido, resultado);
        }

        [TestMethod]
        public void ObtenerTodos_ConPartidosAgregados_DeberiaRetornarTodos()
        {
            var partido1 = CrearPartido(1);
            var partido2 = CrearPartido(2);

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

        [TestMethod]
        public void Actualizar_ConPartidoExistente_DeberiaActualizarDatos()
        {
            var partido = CrearPartido(1);
            partido.Fecha = new DateTime(2026, 6, 1);
            _repositorio.Agregar(partido);

            partido.Fecha = new DateTime(2026, 6, 5);
            _repositorio.Actualizar(partido);

            var resultado = _repositorio.ObtenerPorId(1);
            Assert.AreEqual(new DateTime(2026, 6, 5), resultado.Fecha);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Actualizar_ConPartidoInexistente_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(999);

            _repositorio.Actualizar(partido);
        }
    }
}
