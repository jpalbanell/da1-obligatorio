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
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoUEFACompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 16; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"UEFA_{i}";
                equipo.Confederacion = Confederacion.UEFA;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "UEFA_17";
            equipoExtra.Confederacion = Confederacion.UEFA;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCONMEBOLCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 7; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CONMEBOL_{i}";
                equipo.Confederacion = Confederacion.CONMEBOL;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CONMEBOL_8";
            equipoExtra.Confederacion = Confederacion.CONMEBOL;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCONCАCAFCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 7; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CONCACAF_{i}";
                equipo.Confederacion = Confederacion.CONCACAF;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CONCACAF_8";
            equipoExtra.Confederacion = Confederacion.CONCACAF;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AgregarEquipo_ConCupoCAFCompleto_LanzaExcepcion()
        {
            for (int i = 1; i <= 9; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"CAF_{i}";
                equipo.Confederacion = Confederacion.CAF;
                equipo.RankingFifa = 1500;
                _equipoServicio.AgregarEquipo(equipo);
            }

            var equipoExtra = new Equipo();
            equipoExtra.Nombre = "CAF_10";
            equipoExtra.Confederacion = Confederacion.CAF;
            equipoExtra.RankingFifa = 1500;
            _equipoServicio.AgregarEquipo(equipoExtra);
        }
    }
}