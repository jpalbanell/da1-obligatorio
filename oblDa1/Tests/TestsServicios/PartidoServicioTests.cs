using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class PartidoServicioTests
    {
        private IPartidoServicio _servicio;
        private IPartidoRepositorio _repositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new PartidoRepositorio();
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
            _sesionServicio = new SesionServicio();
            _servicio = new PartidoServicio(_repositorio, _auditoriaServicio, _sesionServicio);

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            _sesionServicio.IniciarSesion(usuario);
        }

        private Equipo CrearEquipoValido(string nombre)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.Confederacion = Confederacion.CONMEBOL;
            equipo.RankingFifa = 1500;
            return equipo;
        }

        private Estadio CrearEstadioValido(string nombre)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;
            return estadio;
        }

        private Grupo CrearGrupoValido(string etiqueta)
        {
            var grupo = new Grupo();
            grupo.Etiqueta = etiqueta;
            return grupo;
        }

        private Partido CrearPartidoValido()
        {
            var partido = new Partido();
            partido.Codigo = "P001";
            partido.Fecha = new DateTime(2026, 6, 1);
            partido.Fase = FaseTorneo.FaseGrupos;
            partido.EquipoLocal = CrearEquipoValido("Uruguay");
            partido.EquipoVisitante = CrearEquipoValido("Argentina");
            partido.Estadio = CrearEstadioValido("Centenario");
            partido.Grupo = CrearGrupoValido("A");
            return partido;
        }

        [TestMethod]
        public void AgregarPartido_ConDatosValidos_DeberiaAgregarlo()
        {
            var partido = CrearPartidoValido();

            _servicio.AgregarPartido(partido);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void AgregarPartido_DeberiaAsignarIdAutomaticamente()
        {
            var partido = CrearPartidoValido();

            _servicio.AgregarPartido(partido);

            Assert.AreEqual(1, partido.Id);
        }
        
        [TestMethod]
        public void AgregarPartido_VariosPartidos_DeberiaAsignarIdsIncrementales()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();

            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            Assert.AreEqual(1, partido1.Id);
            Assert.AreEqual(2, partido2.Id);
        }
        
        [TestMethod]
        public void ObtenerPartido_ConIdExistente_DeberiaRetornarlo()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            var resultado = _servicio.ObtenerPartido(partido.Id);

            Assert.AreEqual("P001", resultado.Codigo);
        }
        
        [TestMethod]
        public void ObtenerPartido_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _servicio.ObtenerPartido(999);
            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConVariosPartidos_DeberiaRetornarTodos()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();
            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
        
        [TestMethod]
        public void ModificarPartido_CambiarFecha_DeberiaActualizar()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            partido.Fecha = new DateTime(2026, 6, 10);
            _servicio.ModificarPartido(partido);

            var resultado = _servicio.ObtenerPartido(partido.Id);
            Assert.AreEqual(new DateTime(2026, 6, 10), resultado.Fecha);
        }
        
        [TestMethod]
        public void ModificarPartido_CambiarEstadio_DeberiaActualizar()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            var nuevoEstadio = CrearEstadioValido("Maracaná");
            partido.Estadio = nuevoEstadio;
            _servicio.ModificarPartido(partido);

            var resultado = _servicio.ObtenerPartido(partido.Id);
            Assert.AreEqual("Maracaná", resultado.Estadio.Nombre);
        }
        
        [TestMethod]
        public void ModificarPartido_CargarResultado_DeberiaActualizar()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            partido.GolesLocal = 3;
            partido.GolesVisitante = 1;
            partido.Vencedor = partido.EquipoLocal;
            _servicio.ModificarPartido(partido);

            var resultado = _servicio.ObtenerPartido(partido.Id);
            Assert.AreEqual(3, resultado.GolesLocal);
            Assert.AreEqual(1, resultado.GolesVisitante);
            Assert.AreEqual(partido.EquipoLocal, resultado.Vencedor);
        }
         
        [TestMethod]
        public void ObtenerPorFecha_ConFechaExistente_DeberiaRetornarPartidos()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();
            partido2.Fecha = new DateTime(2026, 6, 4);

            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            var resultado = _servicio.ObtenerPorFecha(new DateTime(2026, 6, 1));

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorEstadio_ConEstadioExistente_DeberiaRetornarPartidos()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();
            partido2.Estadio = CrearEstadioValido("Maracaná");

            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            var resultado = _servicio.ObtenerPorEstadio("Centenario");

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorGrupo_ConGrupoExistente_DeberiaRetornarPartidos()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();
            partido2.Grupo = CrearGrupoValido("B");

            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            var resultado = _servicio.ObtenerPorGrupo("A");

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorFase_ConFaseExistente_DeberiaRetornarPartidos()
        {
            var partido1 = CrearPartidoValido();
            var partido2 = CrearPartidoValido();
            partido2.Fase = FaseTorneo.Octavos;

            _servicio.AgregarPartido(partido1);
            _servicio.AgregarPartido(partido2);

            var resultado = _servicio.ObtenerPorFase(FaseTorneo.FaseGrupos);

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarPartido_ConPartidoInexistente_DeberiaLanzarExcepcion()
        {
            var partido = CrearPartidoValido();
            partido.Id = 999;

            _servicio.ModificarPartido(partido);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ModificarPartido_PartidoBloqueado_LanzaExcepcion()
        {
            var partido = new Partido(1);
            partido.EstaBloqueado = true;
            _repositorio.Agregar(partido);

            _servicio.ModificarPartido(partido);
        }
        
        [TestMethod]
        public void ObtenerPorEstadio_PartidoSinEstadio_NoLanzaExcepcion()
        {
            var partido = new Partido(1);
            _repositorio.Agregar(partido);

            var resultado = _servicio.ObtenerPorEstadio("Centenario");

            Assert.AreEqual(0, resultado.Count);
        }
        [TestMethod]
        public void ObtenerPorGrupo_PartidoSinGrupo_NoLanzaExcepcion()
        {
            var partido = new Partido(1);
            _repositorio.Agregar(partido);

            var resultado = _servicio.ObtenerPorGrupo("A");

            Assert.AreEqual(0, resultado.Count);
        }
        
        [TestMethod]
        public void ModificarPartido_ConDatosValidos_RegistraLogDeAuditoria()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            partido.Fecha = new DateTime(2026, 6, 10);
            _servicio.ModificarPartido(partido);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void ModificarPartido_ConGolesCargados_DeberiaMarcarTieneResultado()
        {
            var partido = CrearPartidoValido();
            _servicio.AgregarPartido(partido);

            partido.GolesLocal = 2;
            partido.GolesVisitante = 1;
            partido.Vencedor = partido.EquipoLocal;
            partido.TieneResultado = true;
            _servicio.ModificarPartido(partido);

            var resultado = _servicio.ObtenerPartido(partido.Id);
            Assert.IsTrue(resultado.TieneResultado);
        }
    }
}