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
    }
}