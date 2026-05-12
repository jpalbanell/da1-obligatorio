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
    }
}