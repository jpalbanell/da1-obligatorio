using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore; 

namespace Tests
{
    [TestClass]
    public class EquipoRepositorioTests
    {
        private IEquipoRepositorio _equipoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);
            _equipoRepositorio = new EquipoRepositorio(context);
        }

        [TestMethod]
        public void Agregar_EquipoValido_GetAllRetornaUnEquipo()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Agregar(equipo);

            Assert.AreEqual(1, _equipoRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        public void Agregar_EquipoValido_GetByNombreRetornaEquipoCorrecto()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Agregar(equipo);

            Assert.AreEqual(equipo, _equipoRepositorio.ObtenerPorNombre("Uruguay"));
        }
        
        [TestMethod]
        public void ObtenerPorNombre_ConNombreInexistente_RetornaNull()
        {
            var resultado = _equipoRepositorio.ObtenerPorNombre("Uruguay");

            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void Actualizar_EquipoExistente_ActualizaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            equipo.RankingFifa = 1000;
            equipo.Confederacion = Confederacion.CONMEBOL;
            _equipoRepositorio.Agregar(equipo);

            var equipoEditado = new Equipo();
            equipoEditado.Nombre = "Uruguay"; // mismo nombre
            equipoEditado.RankingFifa = 1500; // cambia el ranking
            equipoEditado.Confederacion = Confederacion.UEFA;
            _equipoRepositorio.Actualizar(equipoEditado, "Uruguay");

            var resultado = _equipoRepositorio.ObtenerPorNombre("Uruguay");
            Assert.AreEqual(1500, resultado.RankingFifa);
            Assert.AreEqual(Confederacion.UEFA, resultado.Confederacion);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Actualizar_EquipoInexistente_LanzaExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Actualizar(equipo, "Uruguay");
        }
        
        [TestMethod]
        public void Eliminar_EquipoExistente_GetAllRetornaListaVacia()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            _equipoRepositorio.Agregar(equipo);

            _equipoRepositorio.Eliminar("Uruguay");

            Assert.AreEqual(0, _equipoRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Eliminar_EquipoInexistente_LanzaExcepcion()
        {
            _equipoRepositorio.Eliminar("Uruguay");
        }
        
        
    }
}