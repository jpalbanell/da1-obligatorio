using Dominio.Entidades;
using Servicios;
using Repositorios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class AuditoriaServicioTests
    {
        private IAuditoriaServicio _auditoriaServicio;
        private IAuditoriaRepositorio _auditoriaRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _auditoriaRepositorio = new AuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
        }

        [TestMethod]
        public void Registrar_AccionValidaConUsuario_AgregaLogCorrectamente()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";

            _auditoriaServicio.Registrar("Alta de equipo", usuario);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
    }
}