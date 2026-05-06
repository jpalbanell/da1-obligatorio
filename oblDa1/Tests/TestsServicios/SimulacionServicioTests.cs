using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class SimulacionServicioTests
    {
        private ISimulacionServicio _simulacionServicio;
        private IPartidoRepositorio _partidoRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _partidoRepositorio = new PartidoRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _simulacionServicio = new SimulacionServicio(_partidoRepositorio, _auditoriaServicio, _sesionServicio);

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            _sesionServicio.IniciarSesion(usuario);
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
        public void SimularPartido_EquipoConRankingMaximo_TieneMasGolesMaximosQueRankingMinimo()
        {
            var partidoFuerte = CrearPartidoConEquipos(2500, 300);
            var partidoDebil = CrearPartidoConEquipos(300, 2500);
            _partidoRepositorio.Agregar(partidoFuerte);
            _partidoRepositorio.Agregar(partidoDebil);

            _simulacionServicio.SimularPartido(partidoFuerte.Id, 42);
            _simulacionServicio.SimularPartido(partidoDebil.Id, 42);

            Assert.IsTrue(partidoFuerte.GolesLocal >= partidoDebil.GolesLocal);
        }
        
        [TestMethod]
        public void SimularPartido_ConMismaSemilla_YDistintoId_DeberiaProducirResultadosDiferentes()
        {
            var partido1 = CrearPartidoConEquipos(1500, 1500);
            partido1.Id = 1;
            _partidoRepositorio.Agregar(partido1);

            var partido2 = CrearPartidoConEquipos(1500, 1500);
            partido2.Id = 2;
            _partidoRepositorio.Agregar(partido2);

            _simulacionServicio.SimularPartido(1, 42);
            _simulacionServicio.SimularPartido(2, 42);

            Assert.IsFalse(partido1.GolesLocal == partido2.GolesLocal &&
                           partido1.GolesVisitante == partido2.GolesVisitante);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SimularPartido_PartidoInexistente_LanzaExcepcion()
        {
            _simulacionServicio.SimularPartido(999, 42);
        }
        
        [TestMethod]
        public void SimularPartido_GolesResultantes_NoSonNegativos()
        {
            var partido = CrearPartidoConEquipos(1500, 1500);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            Assert.IsTrue(partido.GolesLocal >= 0);
            Assert.IsTrue(partido.GolesVisitante >= 0);
        }
        
        [TestMethod]
        public void SimularPartido_AsignaVencedorSegunGoles()
        {
            var partido = CrearPartidoConEquipos(1500, 1500);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            if (partido.GolesLocal > partido.GolesVisitante)
                Assert.AreEqual(partido.EquipoLocal, partido.Vencedor);
            else if (partido.GolesVisitante > partido.GolesLocal)
                Assert.AreEqual(partido.EquipoVisitante, partido.Vencedor);
            else
                Assert.IsNull(partido.Vencedor);
        }
        
        [TestMethod]
        public void SimularFase_ConPartidosEnFaseGrupos_SimulaTodosLosPartidos()
        {
            var partido1 = CrearPartidoConEquipos(1500, 1200);
            partido1.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido1);

            var partidoOtraFase = CrearPartidoConEquipos(1800, 1400);
            partidoOtraFase.Fase = FaseTorneo.Octavos;
            _partidoRepositorio.Agregar(partidoOtraFase);

            _simulacionServicio.SimularFase(FaseTorneo.FaseGrupos, 42);

            Assert.IsTrue(partido1.GolesLocal + partido1.GolesVisitante > 0 || 
                          partido1.Vencedor != null);
            Assert.AreEqual(0, partidoOtraFase.GolesLocal + partidoOtraFase.GolesVisitante);
        }
        
        [TestMethod]
        public void SimularFase_SinPartidosEnFase_NoLanzaExcepcion()
        {
            _simulacionServicio.SimularFase(FaseTorneo.Final, 42);
        }
        
        [TestMethod]
        public void SimularPartido_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var partido = CrearPartidoConEquipos(1500, 1200);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void SimularFase_ConPartidos_RegistraLogDeAuditoria()
        {
            var partido = CrearPartidoConEquipos(1500, 1200);
            partido.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularFase(FaseTorneo.FaseGrupos, 42);

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void SimularFase_ConVariosPartidos_DeberiaTenerResultadosDiferentes()
        {
            var partido1 = CrearPartidoConEquipos(1500, 1500);
            partido1.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido1);

            var partido2 = CrearPartidoConEquipos(1500, 1500);
            partido2.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido2);

            _simulacionServicio.SimularFase(FaseTorneo.FaseGrupos, 42);

            Assert.IsFalse(partido1.GolesLocal == partido2.GolesLocal &&
                           partido1.GolesVisitante == partido2.GolesVisitante);
        }
        
        [TestMethod]
        public void SimularPartido_DeberiaRegistrarSemillaEnAuditoria()
        {
            var partido = CrearPartidoConEquipos(1500, 1200);
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 42);

            var logs = _auditoriaRepositorio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => l.Accion.Contains("42")));
        }
        
        [TestMethod]
        public void SimularFase_DeberiaRegistrarSemillaEnAuditoria()
        {
            var partido = CrearPartidoConEquipos(1500, 1200);
            partido.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularFase(FaseTorneo.FaseGrupos, 42);

            var logs = _auditoriaRepositorio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => l.Accion.Contains("FaseGrupos") && l.Accion.Contains("42")));
        }
        [TestMethod]
        public void SimularPartido_EnFaseEliminatoria_ConEmpate_DeberiaAsignarVencedor()
        {
            var partido = CrearPartidoConEquipos(1500, 1500);
            partido.Fase = FaseTorneo.Octavos;
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 0);

            Assert.IsNotNull(partido.Vencedor);
        }
        [TestMethod]
        public void SimularPartido_EnFaseGrupos_ConEmpate_NoDeberiaAsignarVencedor()
        {
            var partido = CrearPartidoConEquipos(1500, 1500);
            partido.Fase = FaseTorneo.FaseGrupos;
            _partidoRepositorio.Agregar(partido);

            _simulacionServicio.SimularPartido(partido.Id, 0);

            Assert.IsNull(partido.Vencedor);
        }
        
    }
}