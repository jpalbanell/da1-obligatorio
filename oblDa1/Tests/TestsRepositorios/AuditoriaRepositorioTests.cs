using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests.TestsRepositorios
{
    [TestClass]
    public class AuditoriaRepositorioTests
    {
        private IAuditoriaRepositorio _auditoriaRepositorio;

        [TestInitialize]
        public void Setup()
        {
            _auditoriaRepositorio = CrearAuditoriaRepositorio();
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
            usuario.Nombre = "Test";
            usuario.Apellido = "Usuario";
            usuario.Email = "test@test.com";
            usuario.FechaNacimiento = new DateTime(1990, 1, 1);
            usuario.Contrasena = "Password@1";
            return usuario;
        }

        [TestMethod]
        public void Agregar_LogValido_NoLanzaExcepcion()
        {
            var log = new LogAuditoria();
            log.Timestamp = DateTime.Now;
            log.Accion = "Alta de equipo";
            log.Usuario = CrearUsuarioValido(); 

            _auditoriaRepositorio.Agregar(log);
        }
        [TestMethod]
        public void ObtenerTodos_ListaVacia_RetornaListaVacia()
        {
            var resultado = _auditoriaRepositorio.ObtenerTodos();

            Assert.AreEqual(0, resultado.Count);
        }
        [TestMethod]
        public void Agregar_LogValido_ObtenerTodosRetornaUnLog()
        {
            var log = new LogAuditoria();
            log.Timestamp = DateTime.Now;
            log.Accion = "Alta de equipo";
            log.Usuario = CrearUsuarioValido(); 

            _auditoriaRepositorio.Agregar(log);

            Assert.AreEqual(1, _auditoriaRepositorio.ObtenerTodos().Count);
        }
        [TestMethod]
        public void Agregar_VariosLogs_ObtenerTodosRetornaTodos()
        {
            var log1 = new LogAuditoria();
            log1.Timestamp = DateTime.Now;
            log1.Accion = "Alta de equipo";
            log1.Usuario = CrearUsuarioValido(); 

            var log2 = new LogAuditoria();
            log2.Timestamp = DateTime.Now;
            log2.Accion = "Alta de estadio";
            log2.Usuario = CrearUsuarioValido(); 

            _auditoriaRepositorio.Agregar(log1);
            _auditoriaRepositorio.Agregar(log2);

            Assert.AreEqual(2, _auditoriaRepositorio.ObtenerTodos().Count);
            Assert.AreEqual(log1, _auditoriaRepositorio.ObtenerTodos()[0]);
            Assert.AreEqual(log2, _auditoriaRepositorio.ObtenerTodos()[1]);
        }
    }
}
