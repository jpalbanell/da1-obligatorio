using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class EstadioServicioTests
    {
        private IEstadioRepositorio _repositorio;
        private IEstadioServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new EstadioRepositorio();
            _servicio = new EstadioServicio(_repositorio);
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
        public void AgregarEstadio_ConDatosValidos_DeberiaAgregarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);

            _servicio.AgregarEstadio(estadio);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEstadio_ConNombreDuplicado_DeberiaLanzarExcepcion()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Centenario", "Buenos Aires", 50000);

            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);
        }

        [TestMethod]
        public void ObtenerEstadio_ConNombreExistente_DeberiaRetornarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            var resultado = _servicio.ObtenerEstadio("Centenario");

            Assert.AreEqual("Centenario", resultado.Nombre);
        }

        [TestMethod]
        public void ObtenerEstadio_ConNombreInexistente_DeberiaRetornarNull()
        {
            var resultado = _servicio.ObtenerEstadio("NoExiste");
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ObtenerTodos_ConVariosEstadios_DeberiaRetornarTodos()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Camp Nou", "Barcelona", 99000);
            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public void ModificarEstadio_ConDatosValidos_DeberiaActualizar()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            estadio.Capacidad = 65000;
            _servicio.ModificarEstadio(estadio);

            var resultado = _servicio.ObtenerEstadio("Centenario");
            Assert.AreEqual(65000, resultado.Capacidad);
        }

        [TestMethod]
        public void EliminarEstadio_ConNombreExistente_DeberiaEliminarlo()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);
            _servicio.AgregarEstadio(estadio);

            _servicio.EliminarEstadio("Centenario");
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EliminarEstadio_ConNombreInexistente_DeberiaLanzarExcepcion()
        {
            _servicio.EliminarEstadio("NoExiste");
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarEstadio_ConNombreDuplicadoDeOtroEstadio_LanzaExcepcion()
        {
            var estadio1 = CrearEstadioValido("Centenario", "Montevideo", 60000);
            var estadio2 = CrearEstadioValido("Maracana", "Rio de Janeiro", 78000);
            _servicio.AgregarEstadio(estadio1);
            _servicio.AgregarEstadio(estadio2);

            estadio2.Nombre = "Centenario";
            _servicio.ModificarEstadio(estadio2);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarEstadio_EstadioInexistente_LanzaExcepcion()
        {
            var estadio = new Estadio();
            estadio.Nombre = "NoExiste";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            _servicio.ModificarEstadio(estadio);
        }
    }
}