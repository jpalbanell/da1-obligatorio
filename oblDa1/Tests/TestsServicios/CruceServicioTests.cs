using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class CruceServicioTests
    {
        private IGrupoRepositorio _grupoRepositorio = null!;
        private IPartidoRepositorio _partidoRepositorio = null!;
        private IAuditoriaServicio _auditoriaServicio = null!;
        private ISesionServicio _sesionServicio = null!;
        private IFixtureRepositorio _fixtureRepositorio = null!;
        private ICruceServicio _servicio = null!;

        [TestInitialize]
        public void Setup()
        {
            _grupoRepositorio = new GrupoRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _sesionServicio = new SesionServicio();
            _fixtureRepositorio = new FixtureRepositorio();
            _servicio = new CruceServicio(
                _grupoRepositorio,
                _partidoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio,
                _sesionServicio
            );
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_SinFixtureGenerado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = false;
            _fixtureRepositorio.Guardar(fixture);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }

        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            return usuario;
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_CrucesYaGenerados_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            fixture.CrucesGenerados = true;
            _fixtureRepositorio.Guardar(fixture);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_ConPartidosSinResultado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);

            var grupo = CrearGrupoConPartidosSinResultado();
            _grupoRepositorio.Agregar(grupo);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42, _sesionServicio.ObtenerUsuarioActual());
        }

        private Grupo CrearGrupoConPartidosSinResultado()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            var equipo1 = CrearEquipo("Uruguay", Confederacion.CONMEBOL, 2000);
            var equipo2 = CrearEquipo("Argentina", Confederacion.CONMEBOL, 1900);
            var estadio = CrearEstadio("Centenario");

            var partido = new Partido(1);
            partido.Codigo = "GA1";
            partido.Fecha = new DateTime(2026, 6, 1, 14, 0, 0);
            partido.Fase = FaseTorneo.FaseGrupos;
            partido.EquipoLocal = equipo1;
            partido.EquipoVisitante = equipo2;
            partido.Estadio = estadio;
            partido.Grupo = grupo;
            partido.TieneResultado = false;

            grupo.ListaPartidos.Add(partido);
            return grupo;
        }

        private Equipo CrearEquipo(string nombre, Confederacion confederacion, int ranking)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.Confederacion = confederacion;
            equipo.RankingFifa = ranking;
            return equipo;
        }

        private Estadio CrearEstadio(string nombre)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;
            return estadio;
        }
        [TestMethod]
        public void CalcularPosiciones_ConPartidosConResultado_DeberiaCalcularCorrectamente()
        {
            var equipo1 = CrearEquipo("Uruguay", Confederacion.CONMEBOL, 2000);
            var equipo2 = CrearEquipo("Argentina", Confederacion.CONMEBOL, 1900);
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            var estadio = CrearEstadio("Centenario");

            var partido = new Partido(1);
            partido.Codigo = "GA1";
            partido.Fecha = new DateTime(2026, 6, 1, 14, 0, 0);
            partido.Fase = FaseTorneo.FaseGrupos;
            partido.EquipoLocal = equipo1;
            partido.EquipoVisitante = equipo2;
            partido.Estadio = estadio;
            partido.Grupo = grupo;
            partido.GolesLocal = 2;
            partido.GolesVisitante = 0;
            partido.Vencedor = equipo1;
            partido.TieneResultado = true;
            grupo.ListaPartidos.Add(partido);

            var posiciones = _servicio.CalcularPosicionesGrupo(grupo);

            var posUruguay = posiciones.First(p => p.Equipo == equipo1);
            var posArgentina = posiciones.First(p => p.Equipo == equipo2);

            Assert.AreEqual(3, posUruguay.Puntos);
            Assert.AreEqual(2, posUruguay.GolesFavor);
            Assert.AreEqual(0, posUruguay.GolesContra);
            Assert.AreEqual(2, posUruguay.DiferenciaGoles);

            Assert.AreEqual(0, posArgentina.Puntos);
            Assert.AreEqual(0, posArgentina.GolesFavor);
            Assert.AreEqual(2, posArgentina.GolesContra);
            Assert.AreEqual(-2, posArgentina.DiferenciaGoles);
        }
        
        [TestMethod]
        public void ObtenerClasificados_DeberiaOrdenarPorPuntosYDiferencia()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);

            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            var equipo1 = CrearEquipo("Uruguay", Confederacion.CONMEBOL, 2000);
            var equipo2 = CrearEquipo("Argentina", Confederacion.CONMEBOL, 1900);
            var equipo3 = CrearEquipo("Brasil", Confederacion.CONMEBOL, 1800);
            var equipo4 = CrearEquipo("Chile", Confederacion.CONMEBOL, 1700);
            var estadio = CrearEstadio("Centenario");

            AgregarPartidoConResultado(grupo, equipo1, equipo2, 3, 0, estadio, 1);
            AgregarPartidoConResultado(grupo, equipo3, equipo4, 2, 1, estadio, 2);
            AgregarPartidoConResultado(grupo, equipo1, equipo3, 1, 0, estadio, 3);
            AgregarPartidoConResultado(grupo, equipo2, equipo4, 2, 0, estadio, 4);
            AgregarPartidoConResultado(grupo, equipo1, equipo4, 1, 0, estadio, 5);
            AgregarPartidoConResultado(grupo, equipo2, equipo3, 1, 1, estadio, 6);

            _grupoRepositorio.Agregar(grupo);

            var posiciones = _servicio.ObtenerClasificados();

            Assert.AreEqual(4, posiciones.Count);
            Assert.AreEqual(equipo1, posiciones[0].Equipo);
        }

        private void AgregarPartidoConResultado(Grupo grupo, Equipo local, Equipo visitante,
            int golesLocal, int golesVisitante, Estadio estadio, int id)
        {
            var partido = new Partido(id);
            partido.Codigo = $"G{id}";
            partido.Fecha = new DateTime(2026, 6, 1, 14, 0, 0);
            partido.Fase = FaseTorneo.FaseGrupos;
            partido.EquipoLocal = local;
            partido.EquipoVisitante = visitante;
            partido.Estadio = estadio;
            partido.Grupo = grupo;
            partido.GolesLocal = golesLocal;
            partido.GolesVisitante = golesVisitante;
            partido.Vencedor = golesLocal > golesVisitante ? local : 
                golesVisitante > golesLocal ? visitante : null;
            partido.TieneResultado = true;
            grupo.ListaPartidos.Add(partido);
        }
    }
}