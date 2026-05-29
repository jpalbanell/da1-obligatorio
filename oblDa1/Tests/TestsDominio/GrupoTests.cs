using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class GrupoTests
    {
        private Equipo CrearEquipo(Confederacion confederacion)
        {
            return new Equipo { Nombre = "Test", Confederacion = confederacion, RankingFifa = 500 };
        }

        private void LlenarGrupo(Grupo grupo)
        {
            for (int i = 0; i < 4; i++)
            {
                var pos = new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.CAF) };
                grupo.ListaPosiciones.Add(pos);
            }
        }
        
        [TestMethod]
        public void CrearGrupo_ConEtiquetaValida_DeberiaAsignarEtiqueta()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            Assert.AreEqual("A", grupo.Etiqueta);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearGrupo_ConEtiquetaInvalida_DeberiaLanzarExcepcion()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "Z";
        }
        
        [TestMethod]
        public void CrearGrupo_ListaPartidosDeberiaEstarVacia()
        {
            var grupo = new Grupo();
            Assert.IsNotNull(grupo.ListaPartidos);
            Assert.AreEqual(0, grupo.ListaPartidos.Count);
        }
        
        [TestMethod]
        public void CrearGrupo_ListaPosicionesDeberiaEstarVacia()
        {
            var grupo = new Grupo();
            Assert.IsNotNull(grupo.ListaPosiciones);
            Assert.AreEqual(0, grupo.ListaPosiciones.Count);
        }
        
        [TestMethod]
        public void PuedeAgregarEquipo_GrupoLleno_DeberiaRetornarFalse()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            LlenarGrupo(grupo);
            var equipo = CrearEquipo(Confederacion.UEFA);

            var resultado = grupo.PuedeAgregarEquipo(equipo);

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void PuedeAgregarEquipo_UEFAConDosDelMismo_DeberiaRetornarFalse()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.ListaPosiciones.Add(new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.UEFA) });
            grupo.ListaPosiciones.Add(new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.UEFA) });
            var equipo = CrearEquipo(Confederacion.UEFA);

            var resultado = grupo.PuedeAgregarEquipo(equipo);

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void PuedeAgregarEquipo_UEFAConUnoDelMismo_DeberiaRetornarTrue()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.ListaPosiciones.Add(new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.UEFA) });
            var equipo = CrearEquipo(Confederacion.UEFA);

            var resultado = grupo.PuedeAgregarEquipo(equipo);

            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void PuedeAgregarEquipo_NoUEFAConUnoDelMismo_DeberiaRetornarFalse()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.ListaPosiciones.Add(new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.CONMEBOL) });
            var equipo = CrearEquipo(Confederacion.CONMEBOL);

            var resultado = grupo.PuedeAgregarEquipo(equipo);

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void PuedeAgregarEquipo_NoUEFASinDelMismo_DeberiaRetornarTrue()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.ListaPosiciones.Add(new PosicionesGrupo { Equipo = CrearEquipo(Confederacion.UEFA) });
            var equipo = CrearEquipo(Confederacion.CONMEBOL);

            var resultado = grupo.PuedeAgregarEquipo(equipo);

            Assert.IsTrue(resultado);
        }
        
        [TestMethod]
        public void AgregarEquipo_GrupoVacio_AgregaPosicionConEquipo()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            var equipo = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1500 };

            grupo.AgregarEquipo(equipo);

            Assert.AreEqual(1, grupo.ListaPosiciones.Count);
            Assert.AreEqual(equipo, grupo.ListaPosiciones[0].Equipo);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_GrupoLleno_LanzaExcepcion()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.AgregarEquipo(new Equipo { Nombre = "E1", Confederacion = Confederacion.CAF, RankingFifa = 500 });
            grupo.AgregarEquipo(new Equipo { Nombre = "E2", Confederacion = Confederacion.AFC, RankingFifa = 500 });
            grupo.AgregarEquipo(new Equipo { Nombre = "E3", Confederacion = Confederacion.OFC, RankingFifa = 500 });
            grupo.AgregarEquipo(new Equipo { Nombre = "E4", Confederacion = Confederacion.CONCACAF, RankingFifa = 500 });

            grupo.AgregarEquipo(new Equipo { Nombre = "E5", Confederacion = Confederacion.CONMEBOL, RankingFifa = 500 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AgregarEquipo_CupoConfederacionExcedido_LanzaExcepcion()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            grupo.AgregarEquipo(new Equipo { Nombre = "E1", Confederacion = Confederacion.CONMEBOL, RankingFifa = 500 });

            grupo.AgregarEquipo(new Equipo { Nombre = "E2", Confederacion = Confederacion.CONMEBOL, RankingFifa = 500 });
        }
        
        [TestMethod]
        public void ObtenerEquipos_GrupoConEquipos_RetornaListaDeEquipos()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            var equipo1 = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1500 };
            var equipo2 = new Equipo { Nombre = "Alemania", Confederacion = Confederacion.UEFA, RankingFifa = 1800 };
            grupo.AgregarEquipo(equipo1);
            grupo.AgregarEquipo(equipo2);

            var equipos = grupo.ObtenerEquipos();

            Assert.AreEqual(2, equipos.Count);
            Assert.IsTrue(equipos.Contains(equipo1));
            Assert.IsTrue(equipos.Contains(equipo2));
        }

        [TestMethod]
        public void ObtenerEquipos_GrupoVacio_RetornaListaVacia()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            var equipos = grupo.ObtenerEquipos();

            Assert.AreEqual(0, equipos.Count);
        }
        
        [TestMethod]
        public void ObtenerPosicionDeEquipo_EquipoExiste_RetornaPosicion()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            var equipo = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1500 };
            grupo.AgregarEquipo(equipo);

            var posicion = grupo.ObtenerPosicionDeEquipo("Uruguay");

            Assert.IsNotNull(posicion);
            Assert.AreEqual(equipo, posicion.Equipo);
        }

        [TestMethod]
        public void ObtenerPosicionDeEquipo_EquipoNoExiste_RetornaNull()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";

            var posicion = grupo.ObtenerPosicionDeEquipo("Uruguay");

            Assert.IsNull(posicion);
        }
        
        [TestMethod]
        public void ActualizarPosiciones_VictoriaLocal_ActualizaPuntosDeAmbos()
        {
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
            var local = new Equipo { Nombre = "Uruguay", Confederacion = Confederacion.CONMEBOL, RankingFifa = 1500 };
            var visitante = new Equipo { Nombre = "Alemania", Confederacion = Confederacion.UEFA, RankingFifa = 1800 };
            grupo.AgregarEquipo(local);
            grupo.AgregarEquipo(visitante);

            var partido = new Partido(1);
            partido.EquipoLocal = local;
            partido.EquipoVisitante = visitante;
            partido.RegistrarResultado(2, 0);

            grupo.ActualizarPosiciones(partido);

            Assert.AreEqual(3, grupo.ObtenerPosicionDeEquipo("Uruguay").Puntos);
            Assert.AreEqual(0, grupo.ObtenerPosicionDeEquipo("Alemania").Puntos);
        }
    }
}