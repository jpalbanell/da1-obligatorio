using Dominio.Entidades;
using Servicios;
using Repositorios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class EquipoServicioTests
    {
        private IEquipoServicio _equipoServicio;
        private IEquipoRepositorio _equipoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _equipoServicio = new EquipoServicio(_equipoRepositorio);
        }
        
        [TestMethod]
        public void AgregarEquipo_ConDatosValidos_AgregaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;

            _equipoServicio.AgregarEquipo(equipo);

            Assert.AreEqual(1, _equipoRepositorio.ObtenerTodos().Count);
        }
    }
}