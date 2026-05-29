using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class PartidoTests
    {
        private Partido CrearPartidoConEquipos()
        {
            var local = new Equipo { Nombre = "Uruguay" };
            var visitante = new Equipo { Nombre = "Argentina" };
            var partido = new Partido(1);
            partido.EquipoLocal = local;
            partido.EquipoVisitante = visitante;
            return partido;
        }
        
        [TestMethod]
        public void CrearPartido_ConIdValido_AsignaIdCorrectamente()
        {
            var partido = new Partido(1);
            Assert.AreEqual(1, partido.Id);
        }

        [TestMethod]
        public void CrearPartido_ConCodigoValido_AsignaCodigoCorrectamente()
        {
            var partido = new Partido(1);
            partido.Codigo = "P001";
            Assert.AreEqual("P001", partido.Codigo);
        }

        [TestMethod]
        public void CrearPartido_ConFechaValida_AsignaFechaCorrectamente()
        {
            var partido = new Partido(1);
            partido.Fecha = new DateTime(2026, 6, 1);
            Assert.AreEqual(new DateTime(2026, 6, 1), partido.Fecha);
        }

        [TestMethod]
        public void CrearPartido_ConFaseValida_AsignaFaseCorrectamente()
        {
            var partido = new Partido(1);
            partido.Fase = FaseTorneo.FaseGrupos;
            Assert.AreEqual(FaseTorneo.FaseGrupos, partido.Fase);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConCodigoNulo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Codigo = null;
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConCodigoVacio_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Codigo = "";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConFechaVacia_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Fecha = default;
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoLocalValido_AsignaEquipoLocalCorrectamente()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
    
            partido.EquipoLocal = equipo;
    
            Assert.AreEqual(equipo, partido.EquipoLocal);
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoVisitanteValido_AsignaEquipoVisitanteCorrectamente()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Argentina";
    
            partido.EquipoVisitante = equipo;
    
            Assert.AreEqual(equipo, partido.EquipoVisitante);
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoLocalNulo_DeberiaPermitirlo()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = null;
            Assert.IsNull(partido.EquipoLocal);
        }
        
        [TestMethod]
        public void CrearPartido_ConEquipoVisitanteNulo_DeberiaPermitirlo()
        {
            var partido = new Partido(1);
            partido.EquipoVisitante = null;
            Assert.IsNull(partido.EquipoVisitante);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConEquipoVisitanteIgualALocal_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
    
            partido.EquipoLocal = equipo;
            partido.EquipoVisitante = equipo;
        }
        
        [TestMethod]
        public void CrearPartido_ConEstadioValido_AsignaEstadioCorrectamente()
        {
            var partido = new Partido(1);
            var estadio = new Estadio();
            estadio.Nombre = "Estadio Centenario";
    
            partido.Estadio = estadio;
    
            Assert.AreEqual(estadio, partido.Estadio);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConEstadioNulo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Estadio = null;
        }
        
        [TestMethod]
        public void CrearPartido_ConGrupoValido_AsignaGrupoCorrectamente()
        {
            var partido = new Partido(1);
            var grupo = new Grupo();
            grupo.Etiqueta = "A";
    
            partido.Grupo = grupo;
    
            Assert.AreEqual(grupo, partido.Grupo);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConGrupoNulo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.Grupo = null;
        }
        
        [TestMethod]
        public void CrearPartido_ConGolesLocalValido_AsignaGolesLocalCorrectamente()
        {
            var partido = new Partido(1);
    
            partido.GolesLocal = 2;
    
            Assert.AreEqual(2, partido.GolesLocal);
        }
        
        [TestMethod]
        public void CrearPartido_ConGolesVisitanteValido_AsignaGolesVisitanteCorrectamente()
        {
            var partido = new Partido(1);
    
            partido.GolesVisitante = 3;
    
            Assert.AreEqual(3, partido.GolesVisitante);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConGolesLocalNegativo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.GolesLocal = -1;
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConGolesVisitanteNegativo_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            partido.GolesVisitante = -1;
        }
        
        [TestMethod]
        public void CrearPartido_ConVencedorValido_AsignaVencedorCorrectamente()
        {
            var partido = new Partido(1);
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
    
            partido.EquipoLocal = equipo;
            partido.Vencedor = equipo;
    
            Assert.AreEqual(equipo, partido.Vencedor);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearPartido_ConVencedorDistintoDeLocalYVisitante_DeberiaLanzarExcepcion()
        {
            var partido = new Partido(1);
            var equipoLocal = new Equipo();
            equipoLocal.Nombre = "Uruguay";
            var equipoVisitante = new Equipo();
            equipoVisitante.Nombre = "Argentina";
            var equipoAjeno = new Equipo();
            equipoAjeno.Nombre = "Brasil";

            partido.EquipoLocal = equipoLocal;
            partido.EquipoVisitante = equipoVisitante;
            partido.Vencedor = equipoAjeno;
        }
        
        [TestMethod]
        public void CrearPartido_EstaBloqueadoPorDefecto_EsFalso()
        {
            var partido = new Partido(1);
            Assert.IsFalse(partido.EstaBloqueado);
        }
        
        [TestMethod]
        public void ObtenerPerdedor_VencedorEsLocal_DeberiaRetornarVisitante()
        {
            var partido = CrearPartidoConEquipos();
            partido.Vencedor = partido.EquipoLocal;

            var perdedor = partido.ObtenerPerdedor();

            Assert.AreEqual(partido.EquipoVisitante, perdedor);
        }

        [TestMethod]
        public void ObtenerPerdedor_VencedorEsVisitante_DeberiaRetornarLocal()
        {
            var partido = CrearPartidoConEquipos();
            partido.Vencedor = partido.EquipoVisitante;

            var perdedor = partido.ObtenerPerdedor();

            Assert.AreEqual(partido.EquipoLocal, perdedor);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ObtenerPerdedor_SinVencedor_DeberiaLanzarExcepcion()
        {
            var partido = CrearPartidoConEquipos();

            partido.ObtenerPerdedor();
        }
        
        [TestMethod]
        public void PuedeModificarse_PartidoNoBloqueado_RetornaTrue()
        {
            var partido = new Partido(1);
            partido.EstaBloqueado = false;

            var resultado = partido.PuedeModificarse();

            Assert.IsTrue(resultado);
        }
        
        [TestMethod]
        public void PuedeModificarse_PartidoBloqueado_RetornaFalse()
        {
            var partido = new Partido(1);
            partido.EstaBloqueado = true;

            var resultado = partido.PuedeModificarse();

            Assert.IsFalse(resultado);
        }
        
        [TestMethod]
        public void TieneEquiposCompletos_AmbosEquiposAsignados_RetornaTrue()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };
            partido.EquipoVisitante = new Equipo { Nombre = "Argentina", RankingFifa = 1600 };

            var resultado = partido.TieneEquiposCompletos();

            Assert.IsTrue(resultado);
        }
        
        [TestMethod]
        public void TieneEquiposCompletos_FaltaEquipoLocal_RetornaFalse()
        {
            var partido = new Partido(1);
            partido.EquipoVisitante = new Equipo { Nombre = "Argentina", RankingFifa = 1600 };

            var resultado = partido.TieneEquiposCompletos();

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void TieneEquiposCompletos_FaltaEquipoVisitante_RetornaFalse()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };

            var resultado = partido.TieneEquiposCompletos();

            Assert.IsFalse(resultado);
        }
        
        [TestMethod]
        public void EsEmpate_MismosGolesYTieneResultado_RetornaTrue()
        {
            var partido = new Partido(1);
            partido.GolesLocal = 1;
            partido.GolesVisitante = 1;
            partido.TieneResultado = true;

            var resultado = partido.EsEmpate();

            Assert.IsTrue(resultado);
        }
        
        [TestMethod]
        public void EsEmpate_DiferentesGoles_RetornaFalse()
        {
            var partido = new Partido(1);
            partido.GolesLocal = 2;
            partido.GolesVisitante = 1;
            partido.TieneResultado = true;

            var resultado = partido.EsEmpate();

            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void EsEmpate_MismosGolesPeroSinResultado_RetornaFalse()
        {
            var partido = new Partido(1);
            partido.GolesLocal = 0;
            partido.GolesVisitante = 0;
            partido.TieneResultado = false;

            var resultado = partido.EsEmpate();

            Assert.IsFalse(resultado);
        }
        
        [TestMethod]
        public void DeterminarVencedor_LocalTieneMasGoles_VencedorEsLocal()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };
            partido.EquipoVisitante = new Equipo { Nombre = "Argentina", RankingFifa = 1600 };
            partido.GolesLocal = 2;
            partido.GolesVisitante = 1;

            partido.DeterminarVencedor();

            Assert.AreEqual(partido.EquipoLocal, partido.Vencedor);
        }
        
        [TestMethod]
        public void DeterminarVencedor_VisitanteTieneMasGoles_VencedorEsVisitante()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };
            partido.EquipoVisitante = new Equipo { Nombre = "Argentina", RankingFifa = 1600 };
            partido.GolesLocal = 0;
            partido.GolesVisitante = 3;

            partido.DeterminarVencedor();

            Assert.AreEqual(partido.EquipoVisitante, partido.Vencedor);
        }
        
        [TestMethod]
        public void DeterminarVencedor_EmpateEnFaseGrupos_VencedorEsNull()
        {
            var partido = new Partido(1);
            partido.EquipoLocal = new Equipo { Nombre = "Uruguay", RankingFifa = 1500 };
            partido.EquipoVisitante = new Equipo { Nombre = "Argentina", RankingFifa = 1600 };
            partido.GolesLocal = 1;
            partido.GolesVisitante = 1;
            partido.Fase = FaseTorneo.FaseGrupos;

            partido.DeterminarVencedor();

            Assert.IsNull(partido.Vencedor);
        }
    }
}