using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class FixtureTests
    {
        private Equipo CrearEquipoValido(string nombre, Confederacion confederacion = Confederacion.CAF)
        {
            var equipo = new Equipo();
            equipo.Nombre = nombre;
            equipo.Confederacion = confederacion;
            equipo.RankingFifa = 500;
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
        
        private List<List<Partido>> CrearPartidosPorGrupo(int cantidadGrupos = 1)
        {
            var resultado = new List<List<Partido>>();
            for (int g = 0; g < cantidadGrupos; g++)
            {
                var equipos = new List<Equipo>();
                for (int i = 0; i < 4; i++)
                {
                    var equipo = new Equipo();
                    equipo.Nombre = $"Equipo_G{g}_{i}";
                    equipo.Confederacion = Confederacion.CAF;
                    equipo.RankingFifa = 500;
                    equipos.Add(equipo);
                }
                var cruces = new (int, int)[] { (0,3),(1,2),(0,2),(1,3),(0,1),(2,3) };
                var partidos = new List<Partido>();
                for (int i = 0; i < cruces.Length; i++)
                {
                    var partido = new Partido();
                    partido.EquipoLocal = equipos[cruces[i].Item1];
                    partido.EquipoVisitante = equipos[cruces[i].Item2];
                    partidos.Add(partido);
                }
                resultado.Add(partidos);
            }
            return resultado;
        }

        
        [TestMethod]
        public void CrearFixture_ConSemillaValida_DeberiaAsignarSemilla()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            Assert.AreEqual(42, fixture.SemillaFixture);
        }
        
        [TestMethod]
        public void CrearFixture_SinAsignarFecha_DeberiaSerPrimerDeJunio2026()
        {
            var fixture = new Fixture();
            Assert.AreEqual(new DateTime(2026, 6, 1), fixture.FechaInicioTorneo);
        }
        
        [TestMethod]
        public void CrearFixture_SinAsignarMaxPartidos_DeberiaSerTres()
        {
            var fixture = new Fixture();
            Assert.AreEqual(3, fixture.MaxPartidosPorDia);
        }
        
        [TestMethod]
        public void CrearFixture_SinAsignarSeparacion_DeberiaSerTres()
        {
            var fixture = new Fixture();
            Assert.AreEqual(3, fixture.SeparacionEntreFechas);
        }
        
        [TestMethod]
        public void CrearFixture_SinGenerar_EstaGeneradoDeberiaSerFalse()
        {
            var fixture = new Fixture();
            Assert.IsFalse(fixture.EstaGenerado);
        }
        
        [TestMethod]
        public void AgregarEquipo_FixtureVacio_AgregaCorrectamente()
        {
            var fixture = new Fixture();
            var equipo = CrearEquipoValido("Uruguay");

            fixture.AgregarEquipo(equipo);

            Assert.AreEqual(1, fixture.Equipos.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_CupoConfederacionCompleto_LanzaExcepcion()
        {
            var fixture = new Fixture();
            for (int i = 0; i < Confederacion.OFC.CupoMaximo(); i++)
                fixture.AgregarEquipo(CrearEquipoValido($"OFC_{i}", Confederacion.OFC));

            fixture.AgregarEquipo(CrearEquipoValido("OFC_Extra", Confederacion.OFC));
        }
        
        [TestMethod]
        public void EliminarEquipo_Existe_EliminaCorrectamente()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));

            fixture.EliminarEquipo("Uruguay");

            Assert.AreEqual(0, fixture.Equipos.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEquipo_NoExiste_LanzaExcepcion()
        {
            var fixture = new Fixture();

            fixture.EliminarEquipo("Uruguay");
        }
        
        [TestMethod]
        public void EditarEquipo_NombreValido_EditaCorrectamente()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            var equipoEditado = CrearEquipoValido("Uruguay2");

            fixture.EditarEquipo(equipoEditado, "Uruguay");

            Assert.AreEqual("Uruguay2", fixture.Equipos[0].Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditarEquipo_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            fixture.AgregarEquipo(CrearEquipoValido("Argentina"));
            var equipoEditado = CrearEquipoValido("Argentina");

            fixture.EditarEquipo(equipoEditado, "Uruguay");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EditarEquipo_CambioConfederacionSinCupo_LanzaExcepcion()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("OFC_1", Confederacion.OFC));
            fixture.AgregarEquipo(CrearEquipoValido("CAF_1", Confederacion.CAF));
            var equipoEditado = CrearEquipoValido("CAF_1", Confederacion.OFC);

            fixture.EditarEquipo(equipoEditado, "CAF_1");
        }
        
        [TestMethod]
        public void AgregarEstadio_NombreUnico_AgregaCorrectamente()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            fixture.AgregarEstadio(estadio);

            Assert.AreEqual(1, fixture.Estadios.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEstadio_NombreDuplicado_LanzaExcepcion()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;

            fixture.AgregarEstadio(estadio);
            fixture.AgregarEstadio(estadio);
        }
        
        [TestMethod]
        public void EliminarEstadio_Existe_EliminaCorrectamente()
        {
            var fixture = new Fixture();
            var estadio = new Estadio();
            estadio.Nombre = "Centenario";
            estadio.Ciudad = "Montevideo";
            estadio.Capacidad = 60000;
            fixture.AgregarEstadio(estadio);

            fixture.EliminarEstadio("Centenario");

            Assert.AreEqual(0, fixture.Estadios.Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void EliminarEstadio_NoExiste_LanzaExcepcion()
        {
            var fixture = new Fixture();

            fixture.EliminarEstadio("Centenario");
        }
        
        [TestMethod]
        public void PuedeGenerarse_48Equipos4Estadios_RetornaTrue()
        {
            var fixture = new Fixture();
            CargarEquipos(fixture);
            CargarEstadios(fixture);

            Assert.IsTrue(fixture.PuedeGenerarse());
        }
        
        private void CargarEstadios(Fixture fixture, int cantidad = 4)
        {
            for (int i = 0; i < cantidad; i++)
                fixture.AgregarEstadio(CrearEstadioValido($"Estadio_{i}"));
        }
        
        private void CargarEquipos(Fixture fixture)
        {
            var cupos = new Dictionary<Confederacion, int>
            {
                { Confederacion.UEFA, 16 }, { Confederacion.CONMEBOL, 7 },
                { Confederacion.CONCACAF, 7 }, { Confederacion.CAF, 9 },
                { Confederacion.AFC, 8 }, { Confederacion.OFC, 1 }
            };
            foreach (var (conf, cantidad) in cupos)
                for (int i = 0; i < cantidad; i++)
                    fixture.AgregarEquipo(CrearEquipoValido($"{conf}_{i}", conf));
        }
        
        [TestMethod]
        public void PuedeGenerarse_MenosEquipos_RetornaFalse()
        {
            var fixture = new Fixture();
            fixture.AgregarEquipo(CrearEquipoValido("Uruguay"));
            CargarEstadios(fixture);

            Assert.IsFalse(fixture.PuedeGenerarse());
        }
        
        [TestMethod]
        public void PuedeGenerarse_MenosEstadios_RetornaFalse()
        {
            var fixture = new Fixture();
            CargarEquipos(fixture);
            CargarEstadios(fixture, 3);

            Assert.IsFalse(fixture.PuedeGenerarse());
        }
        
        [TestMethod]
        public void AsignarFechas_MaxPartidosPorDiaRespetado()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            var partidosPorGrupo = CrearPartidosPorGrupo(4);

            fixture.AsignarFechasAPartidos(partidosPorGrupo);

            var todosLosPartidos = partidosPorGrupo.SelectMany(p => p).ToList();
            var agrupadosPorDia = todosLosPartidos.GroupBy(p => p.Fecha.Date);
            foreach (var dia in agrupadosPorDia)
                Assert.IsTrue(dia.Count() <= fixture.MaxPartidosPorDia,
                    $"El día {dia.Key:dd/MM} tiene {dia.Count()} partidos, máximo es {fixture.MaxPartidosPorDia}");
        }
        
        [TestMethod]
        public void AsignarFechas_RespetoDescansoEntrePartidos()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            var partidosPorGrupo = CrearPartidosPorGrupo(4);

            fixture.AsignarFechasAPartidos(partidosPorGrupo);

            var todosLosPartidos = partidosPorGrupo.SelectMany(p => p).ToList();
            var equipos = todosLosPartidos
                .SelectMany(p => new[] { p.EquipoLocal.Nombre, p.EquipoVisitante.Nombre })
                .Distinct();

            foreach (var equipo in equipos)
            {
                var partidosDelEquipo = todosLosPartidos
                    .Where(p => p.EquipoLocal.Nombre == equipo || p.EquipoVisitante.Nombre == equipo)
                    .OrderBy(p => p.Fecha)
                    .ToList();

                for (int i = 0; i < partidosDelEquipo.Count - 1; i++)
                {
                    var dias = (partidosDelEquipo[i + 1].Fecha.Date - partidosDelEquipo[i].Fecha.Date).Days;
                    Assert.IsTrue(dias >= fixture.SeparacionEntreFechas,
                        $"Equipo {equipo}: solo {dias} días entre partidos, mínimo {fixture.SeparacionEntreFechas}");
                }
            }
        }
        
        [TestMethod]
        public void AsignarFechas_UltimaJornadaSimultanea()
        {
            var fixture = new Fixture();
            fixture.SemillaFixture = 42;
            var partidosPorGrupo = CrearPartidosPorGrupo(4);

            fixture.AsignarFechasAPartidos(partidosPorGrupo);

            foreach (var partidos in partidosPorGrupo)
            {
                var fechaPartidoA = partidos[4].Fecha.Date;
                var fechaPartidoB = partidos[5].Fecha.Date;
                Assert.AreEqual(fechaPartidoA, fechaPartidoB,
                    "Los dos partidos de la última jornada deben jugarse el mismo día");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Fixture_NombreMotorSimulacion_Vacio_LanzaArgumentException()
        {
            var fixture = new Fixture();

            fixture.NombreMotorSimulacion = string.Empty;
        }
    }
}