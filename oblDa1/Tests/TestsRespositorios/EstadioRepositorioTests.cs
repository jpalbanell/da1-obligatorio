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
        public void Agregar_ConEstadioValido_DeberiaPoderObtenerPorNombre()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);

            _repositorio.Agregar(estadio);
            var resultado = _repositorio.ObtenerPorNombre("Centenario");

            Assert.AreEqual(estadio, resultado);
        }

        [TestMethod]
        public void ObtenerTodos_ConEstadiosAgregados_DeberiaRetornarTodos()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Camp Nou", "Barcelona", 99000);

            _repositorio.Agregar(estadio1);
            _repositorio.Agregar(estadio2);
            var resultado = _repositorio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void ObtenerTodos_SinEstadios_DeberiaRetornarListaVacia()
        {
            var resultado = _repositorio.ObtenerTodos();
            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        public void ObtenerPorNombre_ConNombreInexistente_DeberiaRetornarNull()
        {
            var resultado = _repositorio.ObtenerPorNombre("NoExiste");
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void Actualizar_ConEstadioExistente_DeberiaActualizarDatos()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _repositorio.Agregar(estadio);

            estadio.Capacidad = 65000;
            _repositorio.Actualizar(estadio, "Centenario");


            var resultado = _repositorio.ObtenerPorNombre("Centenario");
            Assert.AreEqual(65000, resultado.Capacidad);
        }

        [TestMethod]
        public void Eliminar_ConEstadioExistente_DeberiaEliminarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _repositorio.Agregar(estadio);

            _repositorio.Eliminar("Centenario");
            var resultado = _repositorio.ObtenerPorNombre("Centenario");

            Assert.IsNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Eliminar_ConNombreInexistente_DeberiaLanzarExcepcion()
        {
            _repositorio.Eliminar("NoExiste");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Actualizar_ConEstadioInexistente_DeberiaLanzarExcepcion()
        {
            var estadio = CrearEstadioValido("NoExiste", "Ciudad", 30000);

            _repositorio.Actualizar(estadio, "NoExiste");
        }
    }
}