using Dominio.Entidades;
using IRepositorios;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class EquipoRepositorioTests
    {
        private IEquipoRepositorio _equipoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
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
            _equipoRepositorio.Agregar(equipo);

            var equipoEditado = new Equipo();
            equipoEditado.Nombre = "Argentina";
            _equipoRepositorio.Actualizar(equipoEditado, "Uruguay");

            Assert.AreEqual("Argentina", _equipoRepositorio.ObtenerPorNombre("Argentina").Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
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
        [ExpectedException(typeof(Exception))]
        public void Eliminar_EquipoInexistente_LanzaExcepcion()
        {
            _equipoRepositorio.Eliminar("Uruguay");
        }
        
        
    }
}