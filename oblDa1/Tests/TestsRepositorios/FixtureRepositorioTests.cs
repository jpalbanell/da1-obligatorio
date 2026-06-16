using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class FixtureRepositorioTests
    {
        private SqlContext _context;
        private IFixtureRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new SqlContext(options);
            _repositorio = new FixtureRepositorio(_context);
        }

        private Equipo CrearEquipoValido(string nombre)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.RankingFifa = 1000;
            equipo.Confederacion = Confederacion.CONMEBOL;
            return equipo;
        }

        private Estadio CrearEstadioValido(string nombre)
        {
            var estadio = new Estadio();
            estadio.Nombre = nombre;
            estadio.Ciudad = "Ciudad Test";
            estadio.Capacidad = 50000;
            return estadio;
        }

        [TestMethod]
        public void Guardar_FixtureNuevo_LoInserta()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _repositorio.Guardar(fixture);

            Assert.AreEqual(1, _context.Fixtures.Count());
        }

        [TestMethod]
        public void Guardar_FixtureExistente_LoActualiza()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            _repositorio.Guardar(fixture);

            var fixtureActualizado = new Fixture();
            fixtureActualizado.SemillaFixture = 99;
            _repositorio.Guardar(fixtureActualizado);

            Assert.AreEqual(1, _context.Fixtures.Count());
            Assert.AreEqual(99, _repositorio.Obtener().SemillaFixture);
        }

        [TestMethod]
        public void Obtener_SinFixtureGuardado_RetornaNull()
        {
            var resultado = _repositorio.Obtener();

            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void Obtener_ConFixtureGuardado_RetornaFixture()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            _repositorio.Guardar(fixture);

            var resultado = _repositorio.Obtener();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(42, resultado.SemillaFixture);
        }

        [TestMethod]
        public void Guardar_FixtureExistente_ActualizaTodasLasPropiedadesEscalares()
        {
            var fixtureInicial = new Fixture();
            fixtureInicial.SemillaFixture = 42;
            _repositorio.Guardar(fixtureInicial);

            var fixtureActualizado = new Fixture();
            fixtureActualizado.SemillaFixture = 99;
            fixtureActualizado.EstaGenerado = true;
            fixtureActualizado.CrucesGenerados = true;
            fixtureActualizado.MaxPartidosPorDia = 5;
            fixtureActualizado.SeparacionEntreFechas = 5;
            fixtureActualizado.NombreMotorSimulacion = "Personalizado";
            fixtureActualizado.FechaInicioTorneo = new DateTime(2027, 1, 1);
            _repositorio.Guardar(fixtureActualizado);

            var resultado = _repositorio.Obtener();
            Assert.AreEqual(99, resultado.SemillaFixture);
            Assert.IsTrue(resultado.EstaGenerado);
            Assert.IsTrue(resultado.CrucesGenerados);
            Assert.AreEqual(5, resultado.MaxPartidosPorDia);
            Assert.AreEqual(5, resultado.SeparacionEntreFechas);
            Assert.AreEqual("Personalizado", resultado.NombreMotorSimulacion);
            Assert.AreEqual(new DateTime(2027, 1, 1), resultado.FechaInicioTorneo);
        }

        [TestMethod]
        public void Guardar_FixtureExistenteConMenosEquipos_SincronizaEquiposEnBD()
        {
            var equipo1 = CrearEquipoValido("Argentina");
            var equipo2 = CrearEquipoValido("Brasil");
            _context.Equipos.AddRange(equipo1, equipo2);
            _context.SaveChanges();

            var fixtureInicial = new Fixture();
            fixtureInicial.Equipos.Add(equipo1);
            fixtureInicial.Equipos.Add(equipo2);
            _repositorio.Guardar(fixtureInicial);

            var fixtureActualizado = new Fixture();
            fixtureActualizado.Equipos.Add(equipo1);
            _repositorio.Guardar(fixtureActualizado);

            var resultado = _repositorio.Obtener();
            Assert.AreEqual(1, resultado.Equipos.Count);
            Assert.AreEqual("Argentina", resultado.Equipos[0].Nombre);
        }

        [TestMethod]
        public void Guardar_FixtureExistenteConEquipoSinPersistir_DescartaEquipoSilenciosamente()
        {
            var equipoReal = CrearEquipoValido("Argentina");
            _context.Equipos.Add(equipoReal);
            _context.SaveChanges();

            var fixtureInicial = new Fixture();
            _repositorio.Guardar(fixtureInicial);

            var equipoFantasma = CrearEquipoValido("EquipoFantasma");
            var fixtureActualizado = new Fixture();
            fixtureActualizado.Equipos.Add(equipoReal);
            fixtureActualizado.Equipos.Add(equipoFantasma);
            _repositorio.Guardar(fixtureActualizado);

            var resultado = _repositorio.Obtener();
            Assert.AreEqual(1, resultado.Equipos.Count);
            Assert.AreEqual("Argentina", resultado.Equipos[0].Nombre);
        }

        [TestMethod]
        public void Guardar_FixtureExistenteConMenosEstadios_SincronizaEstadiosEnBD()
        {
            var estadio1 = CrearEstadioValido("Estadio A");
            var estadio2 = CrearEstadioValido("Estadio B");
            _context.Estadios.AddRange(estadio1, estadio2);
            _context.SaveChanges();

            var fixtureInicial = new Fixture();
            fixtureInicial.Estadios.Add(estadio1);
            fixtureInicial.Estadios.Add(estadio2);
            _repositorio.Guardar(fixtureInicial);

            var fixtureActualizado = new Fixture();
            fixtureActualizado.Estadios.Add(estadio1);
            _repositorio.Guardar(fixtureActualizado);

            var resultado = _repositorio.Obtener();
            Assert.AreEqual(1, resultado.Estadios.Count);
            Assert.AreEqual("Estadio A", resultado.Estadios[0].Nombre);
        }

        [TestMethod]
        public void Guardar_FixtureExistenteConEstadioSinPersistir_DescartaEstadioSilenciosamente()
        {
            var estadioReal = CrearEstadioValido("Estadio A");
            _context.Estadios.Add(estadioReal);
            _context.SaveChanges();

            var fixtureInicial = new Fixture();
            _repositorio.Guardar(fixtureInicial);

            var estadioFantasma = CrearEstadioValido("Estadio Fantasma");
            var fixtureActualizado = new Fixture();
            fixtureActualizado.Estadios.Add(estadioReal);
            fixtureActualizado.Estadios.Add(estadioFantasma);
            _repositorio.Guardar(fixtureActualizado);

            var resultado = _repositorio.Obtener();
            Assert.AreEqual(1, resultado.Estadios.Count);
            Assert.AreEqual("Estadio A", resultado.Estadios[0].Nombre);
        }
    }
}
