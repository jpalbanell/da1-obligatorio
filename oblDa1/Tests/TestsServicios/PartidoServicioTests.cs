using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class PartidoServicioTests
    {
        private IPartidoRepositorio _repositorio;
        private IPartidoServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new PartidoRepositorio();
            _servicio = new PartidoServicio(_repositorio);
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
    }
}