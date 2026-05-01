using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class FixtureServicioTests
    {
        private IEquipoRepositorio _equipoRepositorio;
        private IEstadioRepositorio _estadioRepositorio;
        private IPartidoRepositorio _partidoRepositorio;
        private IGrupoRepositorio _grupoRepositorio;
        private IFixtureRepositorio _fixtureRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private IFixtureServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _grupoRepositorio = new GrupoRepositorio();
            _fixtureRepositorio = new FixtureRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _servicio = new FixtureServicio(
                _equipoRepositorio,
                _estadioRepositorio,
                _partidoRepositorio,
                _grupoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio
            );
        }
        
        private void CargarEquipos(int cantidad)
        {
            Confederacion[] confederaciones = {
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.OFC
            };

            for (int i = 0; i < cantidad; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"Equipo_{i + 1}";
                equipo.Confederacion = confederaciones[i];
                equipo.RankingFifa = 2500 - (i * 45);
                _equipoRepositorio.Agregar(equipo);
            }
        }
        
        private void CargarEquiposConEmpate()
        {
            Confederacion[] confederaciones = {
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.OFC
            };

            for (int i = 0; i < 48; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"Equipo_{i + 1}";
                equipo.Confederacion = confederaciones[i];
                equipo.RankingFifa = 1500;
                _equipoRepositorio.Agregar(equipo);
            }
        }
        
        private void CargarEstadios(int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                var estadio = new Estadio();
                estadio.Nombre = $"Estadio_{i + 1}";
                estadio.Ciudad = $"Ciudad_{i + 1}";
                estadio.Capacidad = 40000;
                _estadioRepositorio.Agregar(estadio);
            }
        }
        
        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Leonardo";
            usuario.Apellido = "Fernandez";
            usuario.Email = "leo@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_SinEquipos_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_Sin4Estadios_DeberiaLanzarExcepcion()
        {
            CargarEquipos(48);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_YaGenerado_DeberiaLanzarExcepcion()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            fixture.EstaGenerado = true;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaCrear12Grupos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var grupos = _grupoRepositorio.ObtenerTodos();
            Assert.AreEqual(12, grupos.Count);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_CadaGrupoDeberiaTener4Equipos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var grupos = _grupoRepositorio.ObtenerTodos();
            foreach (var grupo in grupos)
            {
                Assert.AreEqual(4, grupo.ListaPosiciones.Count);
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_NoDeberiaRepetirConfederacionExceptoUefa()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var grupos = _grupoRepositorio.ObtenerTodos();
            foreach (var grupo in grupos)
            {
                var equipos = grupo.ListaPosiciones.Select(p => p.Equipo).ToList();

                int cantidadUefa = equipos.Count(e => e.Confederacion == Confederacion.UEFA);
                Assert.IsTrue(cantidadUefa <= 2, $"Grupo {grupo.Etiqueta} tiene {cantidadUefa} equipos UEFA");

                var noUefa = equipos.Where(e => e.Confederacion != Confederacion.UEFA).ToList();
                var confederacionesRepetidas = noUefa
                    .GroupBy(e => e.Confederacion)
                    .Any(g => g.Count() > 1);
                Assert.IsFalse(confederacionesRepetidas, $"Grupo {grupo.Etiqueta} tiene confederación no-UEFA repetida");
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaCrear72Partidos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var partidos = _partidoRepositorio.ObtenerTodos();
            Assert.AreEqual(72, partidos.Count);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaAsignarFechasCorrectamente()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var partidos = _partidoRepositorio.ObtenerTodos();
            var primerGrupo = partidos.Where(p => p.Grupo.Etiqueta == "A").ToList();

            var fechaInicio = new DateTime(2026, 6, 1, 14, 0, 0);

            Assert.AreEqual(fechaInicio, primerGrupo[0].Fecha);
            Assert.AreEqual(fechaInicio, primerGrupo[1].Fecha);
            Assert.AreEqual(fechaInicio.AddDays(3), primerGrupo[2].Fecha);
            Assert.AreEqual(fechaInicio.AddDays(3), primerGrupo[3].Fecha);
            Assert.AreEqual(fechaInicio.AddDays(6), primerGrupo[4].Fecha);
            Assert.AreEqual(fechaInicio.AddDays(6), primerGrupo[5].Fecha);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaAsignarEstadiosPorRotacion()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var estadiosOrdenados = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => e.Nombre)
                .ToList();

            var partidos = _partidoRepositorio.ObtenerTodos();

            for (int i = 0; i < partidos.Count; i++)
            {
                var estadioEsperado = estadiosOrdenados[i % estadiosOrdenados.Count];
                Assert.AreEqual(estadioEsperado.Nombre, partidos[i].Estadio.Nombre,
                    $"Partido {i + 1} debería tener estadio {estadioEsperado.Nombre}");
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaMarcarComoGenerado()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture, CrearUsuarioValido());

            var resultado = _fixtureRepositorio.Obtener();
            Assert.IsTrue(resultado.EstaGenerado);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaRegistrarAuditoria()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            var usuario = CrearUsuarioValido();

            _servicio.GenerarFixture(fixture, usuario);

            var logs = _auditoriaServicio.ObtenerTodos();
            Assert.IsTrue(logs.Count > 0);
        }
        
        [TestMethod]
        public void GenerarFixture_ConEmpatesDeRanking_MismaSemillaDeberiaGenerarMismoOrden()
        {
            CargarEquiposConEmpate();
            CargarEstadios(4);

            var fixture1 = new Fixture();
            fixture1.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture1, CrearUsuarioValido());
            var grupos1 = _grupoRepositorio.ObtenerTodos();
            var primerEquipoGrupoA1 = grupos1[0].ListaPosiciones[0].Equipo.Nombre;

            Setup();

            CargarEquiposConEmpate();
            CargarEstadios(4);

            var fixture2 = new Fixture();
            fixture2.SemillaFixture = 42;

            _servicio.GenerarFixture(fixture2, CrearUsuarioValido());
            var grupos2 = _grupoRepositorio.ObtenerTodos();
            var primerEquipoGrupoA2 = grupos2[0].ListaPosiciones[0].Equipo.Nombre;

            Assert.AreEqual(primerEquipoGrupoA1, primerEquipoGrupoA2);
        }
    }
}