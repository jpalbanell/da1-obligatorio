using Dominio.Entidades;
using Servicios;
using Repositorios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class EquipoServicioTests
    {
        private IEquipoServicio _equipoServicio;
        private IEquipoRepositorio _equipoRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _equipoRepositorio = new EquipoRepositorio();
            _equipoServicio = new EquipoServicio(_equipoRepositorio);
        }
    }
}