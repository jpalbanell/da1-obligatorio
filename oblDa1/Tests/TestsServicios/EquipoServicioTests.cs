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
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConNombreDuplicado_LanzaExcepcion()
        {
            var equipo1 = new Equipo();
            equipo1.Nombre = "Uruguay";
            equipo1.Confederacion = Confederacion.CONMEBOL;
            equipo1.RankingFifa = 1500;

            var equipo2 = new Equipo();
            equipo2.Nombre = "Uruguay";
            equipo2.Confederacion = Confederacion.CONMEBOL;
            equipo2.RankingFifa = 1200;

            _equipoServicio.AgregarEquipo(equipo1);
            _equipoServicio.AgregarEquipo(equipo2);
        }
    }
}