using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class SimulacionServicioTests
    {
        private IPartidoRepositorio _partidoRepositorio;
        private ISimulacionServicio _simulacionServicio;

        [TestInitialize]
        public void Setup()
        {
            _partidoRepositorio = new PartidoRepositorio();
            _simulacionServicio = new SimulacionServicio(_partidoRepositorio);
        }

        private Partido CrearPartidoConEquipos(int rankingLocal, int rankingVisitante)
        {
            var local = new Equipo();
            local.Nombre = "Local";
            local.Confederacion = Confederacion.CONMEBOL;
            local.RankingFifa = rankingLocal;

            var visitante = new Equipo();
            visitante.Nombre = "Visitante";
            visitante.Confederacion = Confederacion.UEFA;
            visitante.RankingFifa = rankingVisitante;

            var partido = new Partido(1);
            partido.Codigo = "P001";
            partido.Fecha = new DateTime(2026, 6, 1);
            partido.EquipoLocal = local;
            partido.EquipoVisitante = visitante;

            return partido;
        }

        [TestMethod]
        public void SimularPartido_EquipoLocalMasFuerte_GanaLocal()
        {
            var partido = CrearPartidoConEquipos(2500, 300);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            Assert.AreEqual(partido.EquipoLocal, partido.Vencedor);
        }
        
        [TestMethod]
        public void SimularPartido_EquipoVisitanteMasFuerte_GanaVisitante()
        {
            var partido = CrearPartidoConEquipos(300, 2500);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            Assert.AreEqual(partido.EquipoVisitante, partido.Vencedor);
        }
    }
}