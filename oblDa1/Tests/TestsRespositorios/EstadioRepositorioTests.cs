using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class EstadioRepositorioTests
    {
        private IEstadioRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new EstadioRepositorio();
        }

        private Estadio CrearEstadioValido(string nombre, string ciudad, int capacidad)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = ciudad;
            estadio.Capacidad = capacidad;
            return estadio;
        }

        [TestMethod]
        public void Agregar_ConEstadioValido_DeberiaPoderObtenerPorId()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            estadio.Id = 1;

            _repositorio.Agregar(estadio);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.AreEqual(estadio, resultado);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConEstadiosAgregados_DeberiaRetornarTodos()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            estadio1.Id = 1;
            var estadio2 = CrearEstadioValido("Camp Nou", "Barcelona", 99000);
            estadio2.Id = 2;

            _repositorio.Agregar(estadio1);
            _repositorio.Agregar(estadio2);
            var resultado = _repositorio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
    }
}