using Dominio.Entidades;
using IRepositorios;
using Servicios;
using IServicios;
using Repositorios;

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
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _estadioRepositorio = new EstadioRepositorio();
            _partidoRepositorio = new PartidoRepositorio();
            _grupoRepositorio = new GrupoRepositorio();
            _fixtureRepositorio = new FixtureRepositorio();
            _auditoriaServicio = new AuditoriaServicio(new AuditoriaRepositorio());
            _sesionServicio = new SesionServicio();
            _servicio = new FixtureServicio(
                _equipoRepositorio,
                _estadioRepositorio,
                _partidoRepositorio,
                _grupoRepositorio,
                _fixtureRepositorio,
                _auditoriaServicio,
                _sesionServicio
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
        
        private void CargarEquiposParaForzarConflictoConfederacion()
        {
            var confederaciones = new[]
            {
                Confederacion.CONMEBOL, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.CONMEBOL, Confederacion.UEFA, Confederacion.UEFA, Confederacion.UEFA,
                Confederacion.UEFA, Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONMEBOL,
                Confederacion.CONMEBOL, Confederacion.CONMEBOL, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF, Confederacion.CONCACAF,
                Confederacion.CONCACAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.CAF, Confederacion.CAF,
                Confederacion.CAF, Confederacion.CAF, Confederacion.AFC, Confederacion.AFC,
                Confederacion.AFC, Confederacion.AFC, Confederacion.AFC, Confederacion.AFC,
                Confederacion.AFC, Confederacion.AFC, Confederacion.UEFA, Confederacion.OFC
            };

            for (int i = 0; i < 48; i++)
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

            int[] rankings = {
                2500, 2450, 2400, 2350, 2300, 2300, 2250, 2200,
                2150, 2100, 2050, 2000, 1950, 1900, 1850, 1800,
                1750, 1750, 1700, 1650, 1600, 1550, 1500,
                1450, 1400, 1350, 1300, 1300, 1250, 1200,
                1150, 1100, 1050, 1000, 950, 900, 850, 800, 750,
                700, 650, 600, 600, 550, 500, 450, 400, 350
            };

            for (int i = 0; i < 48; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"Equipo_{i + 1}";
                equipo.Confederacion = confederaciones[i];
                equipo.RankingFifa = rankings[i];
                _equipoRepositorio.Agregar(equipo);
            }
        }
        
        private void CargarEquiposEnRepositorio(IEquipoRepositorio repositorio)
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

            int[] rankings = {
                2500, 2450, 2400, 2350, 2300, 2300, 2250, 2200,
                2150, 2100, 2050, 2000, 1950, 1900, 1850, 1800,
                1750, 1750, 1700, 1650, 1600, 1550, 1500,
                1450, 1400, 1350, 1300, 1300, 1250, 1200,
                1150, 1100, 1050, 1000, 950, 900, 850, 800, 750,
                700, 650, 600, 600, 550, 500, 450, 400, 350
            };

            for (int i = 0; i < 48; i++)
            {
                var equipo = new Equipo();
                equipo.Nombre = $"Equipo_{i + 1}";
                equipo.Confederacion = confederaciones[i];
                equipo.RankingFifa = rankings[i];
                repositorio.Agregar(equipo);
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
        
        private void CargarEstadiosEnRepositorio(IEstadioRepositorio repositorio)
        {
            for (int i = 0; i < 4; i++)
            {
                var estadio = new Estadio();
                estadio.Nombre = $"Estadio_{i + 1}";
                estadio.Ciudad = $"Ciudad_{i + 1}";
                estadio.Capacidad = 40000;
                repositorio.Agregar(estadio);
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
            usuario.Roles.Add(Rol.Editor);
            return usuario;
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_SinEquipos_DeberiaLanzarExcepcion()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            
            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GenerarFixture_Sin4Estadios_DeberiaLanzarExcepcion()
        {
            CargarEquipos(48);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            
            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);
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
            
            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaCrear12Grupos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();
            Assert.AreEqual(72, partidos.Count);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaAsignarHorasValidas()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();
            var horasValidas = new[] { 14, 18, 22 };

            foreach (var partido in partidos)
            {
                Assert.IsTrue(horasValidas.Contains(partido.Fecha.Hour),
                    $"Hora {partido.Fecha.Hour} no es válida. Debe ser 14, 18 o 22");
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaAsignarEstadiosPorRotacion()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

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

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture1);
            var primerEquipoGrupoA1 = _grupoRepositorio.ObtenerTodos()[0]
                .ListaPosiciones[0].Equipo.Nombre;

            var equipoRepositorio2 = new EquipoRepositorio();
            var estadioRepositorio2 = new EstadioRepositorio();
            var partidoRepositorio2 = new PartidoRepositorio();
            var grupoRepositorio2 = new GrupoRepositorio();
            var fixtureRepositorio2 = new FixtureRepositorio();
            var auditoriaServicio2 = new AuditoriaServicio(new AuditoriaRepositorio());
            var sesionServicio2 = new SesionServicio();
            var servicio2 = new FixtureServicio(
                equipoRepositorio2, estadioRepositorio2, partidoRepositorio2,
                grupoRepositorio2, fixtureRepositorio2, auditoriaServicio2, sesionServicio2);

            sesionServicio2.IniciarSesion(CrearUsuarioValido());

            CargarEquiposEnRepositorio(equipoRepositorio2);
            CargarEstadiosEnRepositorio(estadioRepositorio2);

            var fixture2 = new Fixture();
            fixture2.SemillaFixture = 42;

            servicio2.GenerarFixture(fixture2);
            var primerEquipoGrupoA2 = grupoRepositorio2.ObtenerTodos()[0]
                .ListaPosiciones[0].Equipo.Nombre;

            Assert.AreEqual(primerEquipoGrupoA1, primerEquipoGrupoA2);
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_MaximoTresPartidosPorDia()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();
            var partidosPorDia = partidos.GroupBy(p => p.Fecha.Date);

            foreach (var dia in partidosPorDia)
            {
                Assert.IsTrue(dia.Count() <= 3, 
                    $"El día {dia.Key.ToShortDateString()} tiene {dia.Count()} partidos");
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConEstadiosConTildesYMayusculas_DeberiaOrdenarPorNombreNormalizado()
        {
            CargarEquipos(48);

            var nombres = new[] { "Tróccoli", "  CENTENARIO", "campeón del siglo", "Parque Viera" };
            foreach (var nombre in nombres)
            {
                var estadio = new Estadio();
                estadio.Nombre = nombre;
                estadio.Ciudad = "Montevideo";
                estadio.Capacidad = 40000;
                _estadioRepositorio.Agregar(estadio);
            }

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();

            Assert.AreEqual("campeón del siglo", partidos[0].Estadio.Nombre);
            Assert.AreEqual("  CENTENARIO", partidos[1].Estadio.Nombre);
            Assert.AreEqual("Parque Viera", partidos[2].Estadio.Nombre);
            Assert.AreEqual("Tróccoli", partidos[3].Estadio.Nombre);
        }
        
        [TestMethod]
        public void GenerarFixture_ConOrdenQueRompeRoundRobinPuro_NoDeberiaRepetirConfederacionNoUefa()
        {
            CargarEquiposParaForzarConflictoConfederacion();
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var grupos = _grupoRepositorio.ObtenerTodos();
            foreach (var grupo in grupos)
            {
                var equipos = grupo.ListaPosiciones.Select(p => p.Equipo).ToList();
                var noUefa = equipos.Where(e => e.Confederacion != Confederacion.UEFA).ToList();
                var confederacionesRepetidas = noUefa
                    .GroupBy(e => e.Confederacion)
                    .Any(g => g.Count() > 1);
                Assert.IsFalse(confederacionesRepetidas, $"Grupo {grupo.Etiqueta} tiene confederación no-UEFA repetida");
            }
        }
        
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_DeberiaAsignarCodigosConPrefijoGrupo()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;

            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();
            var grupos = _grupoRepositorio.ObtenerTodos();

            foreach (var grupo in grupos)
            {
                var partidosDelGrupo = partidos.Where(p => p.Grupo.Etiqueta == grupo.Etiqueta).ToList();
                Assert.AreEqual(6, partidosDelGrupo.Count);

                for (int i = 0; i < partidosDelGrupo.Count; i++)
                {
                    var codigoEsperado = $"G{grupo.Etiqueta}-{i + 1}";
                    Assert.AreEqual(codigoEsperado, partidosDelGrupo[i].Codigo,
                        $"Partido {i + 1} del grupo {grupo.Etiqueta} debería tener código {codigoEsperado}");
                }
            }
        }
      
        [TestMethod]
        public void GenerarFixture_ConDatosValidos_CadaEquipoDebeDescansarAlMenos3DiasEntrePartidos()
        {
            CargarEquipos(48);
            CargarEstadios(4);

            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            
            _sesionServicio.IniciarSesion(CrearUsuarioValido());
            _servicio.GenerarFixture(fixture);

            var partidos = _partidoRepositorio.ObtenerTodos();
            var equipos = _equipoRepositorio.ObtenerTodos();

            foreach (var equipo in equipos)
            {
                var partidosDelEquipo = partidos
                    .Where(p => p.EquipoLocal.Nombre == equipo.Nombre 
                                || p.EquipoVisitante.Nombre == equipo.Nombre)
                    .OrderBy(p => p.Fecha)
                    .ToList();

                for (int i = 0; i < partidosDelEquipo.Count - 1; i++)
                {
                    var diasEntrePartidos = (partidosDelEquipo[i + 1].Fecha.Date 
                                             - partidosDelEquipo[i].Fecha.Date).Days;

                    Assert.IsTrue(diasEntrePartidos >= fixture.SeparacionEntreFechas,
                        $"Equipo {equipo.Nombre}: partido el {partidosDelEquipo[i].Fecha.Date:dd/MM} " +
                        $"y otro el {partidosDelEquipo[i + 1].Fecha.Date:dd/MM}. " +
                        $"Solo {diasEntrePartidos} días de diferencia, mínimo {fixture.SeparacionEntreFechas}.");
                }
            }
        }
        
      [TestMethod]
      public void GenerarFixture_DeberiaRegistrarAuditoriaIncluyendoSemilla()
      {
          CargarEquipos(48);
          CargarEstadios(4);

          var fixture = new Fixture();

          fixture.SemillaFixture = 42;

          _sesionServicio.IniciarSesion(CrearUsuarioValido());
          _servicio.GenerarFixture(fixture);

          var logs = _auditoriaServicio.ObtenerTodos();

          Assert.IsTrue(logs.Any(l => l.Accion.Contains("42")),
              "El log de auditoría debe incluir el valor de SemillaFixture");
      }

      [TestMethod]
      [ExpectedException(typeof(Exception))]
      public void GenerarFixture_SinRolEditor_DeberiaLanzarExcepcion()
      {
          CargarEquipos(48);
          CargarEstadios(4);

          var fixture = new Fixture();

          fixture.SemillaFixture = 42;

          var usuarioSinRol = new Usuario();

          usuarioSinRol.Nombre = "Juan";
          usuarioSinRol.Apellido = "Perez";
          usuarioSinRol.Email = "juan@ejemplo.com";
          usuarioSinRol.FechaNacimiento = new DateTime(1990, 5, 15);
          usuarioSinRol.Contrasena = "Password@1";
          _sesionServicio.IniciarSesion(usuarioSinRol);
          _servicio.GenerarFixture(fixture);
      }
    }
}