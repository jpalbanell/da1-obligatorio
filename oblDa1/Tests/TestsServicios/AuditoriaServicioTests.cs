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
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Registrar_AccionNula_LanzaExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";

            _auditoriaServicio.Registrar(null, usuario);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Registrar_UsuarioNulo_LanzaExcepcion()
        {
            _auditoriaServicio.Registrar("Alta de equipo", null);
        }
        
        [TestMethod]
        public void ObtenerTodos_ListaVacia_RetornaListaVacia()
        {
            var resultado = _auditoriaServicio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }
    }
}