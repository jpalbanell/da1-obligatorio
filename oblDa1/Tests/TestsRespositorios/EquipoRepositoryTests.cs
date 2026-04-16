using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class EquipoRepositoryTests
    {
        private IEquipoRepository _equipoRepository;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepository = new EquipoRepository();
        }

        [TestMethod]
        public void Add_EquipoValido_GetAllRetornaUnEquipo()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepository.Add(equipo);

            Assert.AreEqual(1, _equipoRepository.GetAll().Count);
        }
        
        [TestMethod]
        public void Add_EquipoValido_GetByNombreRetornaEquipoCorrecto()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepository.Add(equipo);

            Assert.AreEqual(equipo, _equipoRepository.GetByNombre("Uruguay"));
        }
        
        [TestMethod]
        public void GetByNombre_ConNombreInexistente_RetornaNull()
        {
            var resultado = _equipoRepository.GetByNombre("Uruguay");

            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void Update_EquipoExistente_ActualizaCorrectamente()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            _equipoRepository.Add(equipo);

            equipo.Nombre = "Argentina";
            _equipoRepository.Update(equipo);

            Assert.AreEqual("Argentina", _equipoRepository.GetByNombre("Argentina").Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Update_EquipoInexistente_LanzaExcepcion()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";

            _equipoRepository.Update(equipo);
        }
        
        [TestMethod]
        public void Delete_EquipoExistente_GetAllRetornaListaVacia()
        {
            var equipo = new Equipo();
            equipo.Nombre = "Uruguay";
            _equipoRepository.Add(equipo);

            _equipoRepository.Delete("Uruguay");

            Assert.AreEqual(0, _equipoRepository.GetAll().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Delete_EquipoInexistente_LanzaExcepcion()
        {
            _equipoRepository.Delete("Uruguay");
        }
        
        
    }
}