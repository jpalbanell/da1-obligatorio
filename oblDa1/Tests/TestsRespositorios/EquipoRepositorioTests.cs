using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidades;
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
        public void Add_EquipoValido_GetAllRetornaUnEquipo()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Add(equipo);

            Assert.AreEqual(1, _equipoRepositorio.GetAll().Count);
        }
        
        [TestMethod]
        public void Add_EquipoValido_GetByNombreRetornaEquipoCorrecto()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Add(equipo);

            Assert.AreEqual(equipo, _equipoRepositorio.GetByNombre("Uruguay"));
        }
        
        [TestMethod]
        public void GetByNombre_ConNombreInexistente_RetornaNull()
        {
            var resultado = _equipoRepositorio.GetByNombre("Uruguay");

            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void Update_EquipoExistente_ActualizaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            _equipoRepositorio.Add(equipo);

            equipo.Nombre = "Argentina";
            _equipoRepositorio.Update(equipo);

            Assert.AreEqual("Argentina", _equipoRepositorio.GetByNombre("Argentina").Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Update_EquipoInexistente_LanzaExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepositorio.Update(equipo);
        }
        
        [TestMethod]
        public void Delete_EquipoExistente_GetAllRetornaListaVacia()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            _equipoRepositorio.Add(equipo);

            _equipoRepositorio.Delete("Uruguay");

            Assert.AreEqual(0, _equipoRepositorio.GetAll().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Delete_EquipoInexistente_LanzaExcepcion()
        {
            _equipoRepositorio.Delete("Uruguay");
        }
        
        
    }
}