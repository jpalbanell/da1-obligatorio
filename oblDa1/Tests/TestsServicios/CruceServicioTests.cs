using Dominio.Entidades;
using IRepositorios;
using Servicios;
using IServicios;
using Repositorios;

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
        private IEstadioRepositorio _estadioRepositorio = null!;

        [TestInitialize]
        public void Setup()
        {
            _grupoRepositorio = new GrupoRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _sesionServicio = new SesionServicio();
            _fixtureRepositorio = new FixtureRepositorio();
            _servicio = new CruceServicio(
                _grupoRepositorio,
                _partidoRepositorio,
                _fixtureRepositorio,
                _estadioRepositorio,
                _auditoriaServicio,
                _sesionServicio
            );

            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Editor);
            _sesionServicio.IniciarSesion(usuario);
        }
        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Editor);
            return usuario;
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
            _partidoRepositorio.Agregar(partido);

            if (_estadioRepositorio.ObtenerPorNombre(estadio.Nombre) == null)
                _estadioRepositorio.Agregar(estadio);
        }

        private void CargarDoceGruposCompletos()
        {
            var etiquetas = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
            var idPartido = 1;

            foreach (var etiqueta in etiquetas)
            {
                var grupo = new Grupo();
                grupo.Etiqueta = etiqueta;
                var estadio = CrearEstadio("Estadio " + etiqueta);

                var e1 = CrearEquipo(etiqueta + "_1", Confederacion.UEFA, 2000);
                var e2 = CrearEquipo(etiqueta + "_2", Confederacion.UEFA, 1900);
                var e3 = CrearEquipo(etiqueta + "_3", Confederacion.UEFA, 1800);
                var e4 = CrearEquipo(etiqueta + "_4", Confederacion.UEFA, 1700);

                AgregarPartidoConResultado(grupo, e1, e2, 3, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e3, e4, 2, 1, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e1, e3, 1, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e2, e4, 2, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e1, e4, 1, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e2, e3, 1, 1, estadio, idPartido++);

                _grupoRepositorio.Agregar(grupo);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_SinFixtureGenerado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = false;
            _fixtureRepositorio.Guardar(fixture);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);
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
            _servicio.GenerarCruces(42);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_ConPartidosSinResultado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);

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
            _grupoRepositorio.Agregar(grupo);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);
        }

        [TestMethod]
        public void GenerarCruces_DeberiaCrear16PartidosDeDieciseisavos()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.Dieciseisavos)
                .ToList();
            Assert.AreEqual(16, partidos.Count);
        }

        [TestMethod]
        public void GenerarCruces_MismaSemilla_DeberiaGenerarMismoOrden()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);

            var partidos1 = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.Dieciseisavos)
                .OrderBy(p => p.Codigo)
                .ToList();

            _grupoRepositorio = new GrupoRepositorio();
            _fixtureRepositorio = new FixtureRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            _servicio = new CruceServicio(
                _grupoRepositorio,
                _partidoRepositorio,
                _fixtureRepositorio,
                _estadioRepositorio,
                _auditoriaServicio,
                _sesionServicio
            );
            CargarDoceGruposCompletos();
            _servicio.GenerarCruces(42);

            var partidos2 = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.Dieciseisavos)
                .OrderBy(p => p.Codigo)
                .ToList();

            Assert.AreEqual(partidos1.Count, partidos2.Count);
            for (int i = 0; i < partidos1.Count; i++)
            {
                Assert.AreEqual(partidos1[i].EquipoLocal.Nombre, partidos2[i].EquipoLocal.Nombre);
                Assert.AreEqual(partidos1[i].EquipoVisitante.Nombre, partidos2[i].EquipoVisitante.Nombre);
            }
        }

        [TestMethod]
        public void GenerarCruces_NingunaPareja_DeberiaSerDelMismoGrupo()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.Dieciseisavos)
                .ToList();

            foreach (var partido in partidos)
            {
                Assert.AreNotEqual(
                    partido.EquipoLocal.Nombre.Split('_')[0],
                    partido.EquipoVisitante.Nombre.Split('_')[0]
                );
            }
        }

        [TestMethod]
        public void GenerarCruces_DeberiaAsignarCodigosCorrectos()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.Dieciseisavos)
                .ToList();

            Assert.IsTrue(partidos.Any(p => p.Codigo == "A1"));
            Assert.IsTrue(partidos.Any(p => p.Codigo == "A8"));
            Assert.IsTrue(partidos.Any(p => p.Codigo == "B1"));
            Assert.IsTrue(partidos.Any(p => p.Codigo == "B8"));
        }

        [TestMethod]
        public void GenerarCruces_DeberiaBloquearPartidosDeFaseGrupos()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var grupos = _grupoRepositorio.ObtenerTodos();
            var todosLosPartidos = grupos.SelectMany(g => g.ListaPartidos).ToList();
            Assert.IsTrue(todosLosPartidos.All(p => p.EstaBloqueado));
        }

        [TestMethod]
        public void GenerarCruces_DeberiaRegistrarAuditoria()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.AreEqual(1, logs.Count);
            Assert.IsTrue(logs[0].Accion.Contains("cruces"));
        }

        [TestMethod]
        public void GenerarCruces_DeberiaMarcarCrucesComoGenerados()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var fixtureActualizado = _fixtureRepositorio.Obtener();
            Assert.IsTrue(fixtureActualizado.CrucesGenerados);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_SinRolEditor_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();

            var usuarioSinRol = new Usuario();
            usuarioSinRol.Nombre = "Juan";
            usuarioSinRol.Apellido = "Perez";
            usuarioSinRol.Email = "juan@ejemplo.com";
            usuarioSinRol.FechaNacimiento = new DateTime(1990, 5, 15);
            usuarioSinRol.Contrasena = "Abcdef1@";
            _sesionServicio.IniciarSesion(usuarioSinRol);

            _servicio.GenerarCruces(42);
        }
        
        [TestMethod]
        public void GenerarCruces_PartidoTercerPuesto_DeberiaUsarPerdedoresDeSemifinales()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var partidoTP = _partidoRepositorio.ObtenerTodos()
                .First(p => p.Codigo == "TP");

            Assert.IsTrue(partidoTP.EsPorPerdedor,
                "El tercer puesto debe estar marcado como por perdedor");
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarCruces_ConPartidosEnRepositorioSinResultado_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);

            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            _grupoRepositorio.Agregar(grupo);

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
            _partidoRepositorio.Agregar(partido);

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);
        }
        
        [TestMethod]
        public void GenerarCruces_DeberiaBloquearTodosLosPartidosDeFaseGruposEnElRepositorio()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var partidosFaseGrupos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .ToList();

            Assert.IsTrue(partidosFaseGrupos.All(p => p.EstaBloqueado));
        }
        
        [TestMethod]
        public void GenerarCruces_DeberiaAplicarSemillaCrucesFaseEnDesempate()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposEmpateTotal();

            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.IsTrue(logs.Any(l => l.Accion.Contains("SemillaCrucesFase: 42")));
        }
        
        private void CargarDoceGruposEmpateTotal()
        {
            var etiquetas = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
            var idPartido = 1;

            foreach (var etiqueta in etiquetas)
            {
                var grupo = new Grupo();
                grupo.Etiqueta = etiqueta;
                var estadio = CrearEstadio("Estadio " + etiqueta);

                var e1 = CrearEquipo(etiqueta + "_1", Confederacion.UEFA, 2000);
                var e2 = CrearEquipo(etiqueta + "_2", Confederacion.UEFA, 1900);
                var e3 = CrearEquipo(etiqueta + "_3", Confederacion.UEFA, 1800);
                var e4 = CrearEquipo(etiqueta + "_4", Confederacion.UEFA, 1700);

                AgregarPartidoConResultado(grupo, e1, e2, 0, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e3, e4, 0, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e1, e3, 0, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e2, e4, 0, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e1, e4, 0, 0, estadio, idPartido++);
                AgregarPartidoConResultado(grupo, e2, e3, 0, 0, estadio, idPartido++);

                _grupoRepositorio.Agregar(grupo);
            }
        }
        
        [TestMethod]
        public void GenerarCruces_DeberiaAsignarIdsQueNoChocanConPartidosExistentes()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();
            _sesionServicio.IniciarSesion(CrearUsuarioValido());

            _servicio.GenerarCruces(42);

            var ids = _partidoRepositorio.ObtenerTodos().Select(p => p.Id).ToList();
            var idsUnicos = ids.Distinct().ToList();

            Assert.AreEqual(ids.Count, idsUnicos.Count, "No debe haber IDs duplicados entre partidos de fase de grupos y cruces");
        }
        [TestMethod]
        public void GenerarCruces_DeberiaRotarEstadiosPorNombreNormalizado()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();

            _estadioRepositorio.Agregar(CrearEstadio("Maracaná"));
            _estadioRepositorio.Agregar(CrearEstadio("Centenario"));
            _estadioRepositorio.Agregar(CrearEstadio("Azteca"));
            _estadioRepositorio.Agregar(CrearEstadio("Wembley"));

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);

            var partidosEliminatorios = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase != FaseTorneo.FaseGrupos)
                .OrderBy(p => p.Id)
                .ToList();

            var nombresEstadios = partidosEliminatorios.Select(p => p.Estadio.Nombre).Distinct().ToList();

            Assert.IsTrue(nombresEstadios.Count > 1, "Los partidos eliminatorios deben rotar entre múltiples estadios");
        }
        
        [TestMethod]
        public void GenerarCruces_DeberiaAsignarFechasDistintasParaFasesEliminatorias()
        {
            var fixture = new Fixture();
            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            CargarDoceGruposCompletos();

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarCruces(42);

            var partidosEliminatorios = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase != FaseTorneo.FaseGrupos)
                .ToList();

            var fechasDistintas = partidosEliminatorios.Select(p => p.Fecha.Date).Distinct().ToList();

            Assert.IsTrue(fechasDistintas.Count > 1, 
                "Los partidos eliminatorios deben tener fechas distintas, no todos el mismo día");
        }
    }
}