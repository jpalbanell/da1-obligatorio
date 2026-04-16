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
        public void AgregarEstadio_DeberiaAsignarIdAutomaticamente()
        {
            var estadio = CrearEstadioValido("Centenario", "Montevideo", 60000);

            _servicio.AgregarEstadio(estadio);

            Assert.AreEqual(1, estadio.Id);
        }
    }
}