using Dominio.Entidades;
using Servicios;
using IRepositorios;
using Repositorios;
using IServicios;
using Microsoft.EntityFrameworkCore;

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
            _auditoriaRepositorio = CrearAuditoriaRepositorio();
            _auditoriaServicio = new AuditoriaServicio(_auditoriaRepositorio);
        }

        private AuditoriaRepositorio CrearAuditoriaRepositorio()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AuditoriaRepositorio(new SqlContext(options));
        }
        
        private Usuario CrearUsuarioValido()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            usuario.Apellido = "Garcia";
            usuario.Email = "santiago@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }

        [TestMethod]
        public void Registrar_AccionValidaConUsuario_AgregaLogCorrectamente()
        {
            var usuario = CrearUsuarioValido(); 

            _auditoriaServicio.Registrar("Alta de equipo", usuario);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Registrar_AccionNula_LanzaExcepcion()
        {
            var usuario = CrearUsuarioValido(); 

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
        
        [TestMethod]
        public void ObtenerTodos_ConLogs_RetornaListaCorrecta()
        {
            var usuario = CrearUsuarioValido(); 

            _auditoriaServicio.Registrar("Alta de equipo", usuario);

            Assert.AreEqual(1, _auditoriaServicio.ObtenerTodos().Count);
        }
    }
}