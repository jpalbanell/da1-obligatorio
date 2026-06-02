using Dominio.Entidades;
using IRepositorios;
using Repositorios;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class UsuarioRepositorioTests
    {
        private IUsuarioRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);
            _repositorio = new UsuarioRepositorio(context);
        }

        private Usuario CrearUsuarioValido(string nombre, string apellido, string email)
        {
            var usuario = new Usuario();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Email = email;
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            return usuario;
        }

        [TestMethod]
        public void Agregar_ConUsuarioValido_DeberiaPoderObtenerPorId()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;

            _repositorio.Agregar(usuario);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.AreEqual(usuario, resultado);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConUsuariosAgregados_DeberiaRetornarTodos()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario1.Id = 1;
            var usuario2 = CrearUsuarioValido("María", "López", "maria@ejemplo.com");
            usuario2.Id = 2;

            _repositorio.Agregar(usuario1);
            _repositorio.Agregar(usuario2);
            var resultado = _repositorio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerTodos_SinUsuarios_DeberiaRetornarListaVacia()
        {
            var resultado = _repositorio.ObtenerTodos();
            Assert.AreEqual(0, resultado.Count);
        }
        
        [TestMethod]
        public void ObtenerPorId_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _repositorio.ObtenerPorId(999);
            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void Actualizar_ConUsuarioExistente_DeberiaActualizarDatos()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repositorio.Agregar(usuario);

            usuario.Nombre = "Juan Pablo";
            usuario.Apellido = "González";
            _repositorio.Actualizar(usuario);

            var resultado = _repositorio.ObtenerPorId(1);
            Assert.AreEqual("Juan Pablo", resultado.Nombre);
            Assert.AreEqual("González", resultado.Apellido);
        }
        
        [TestMethod]
        public void Eliminar_ConUsuarioExistente_DeberiaEliminarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 1;
            _repositorio.Agregar(usuario);

            _repositorio.Eliminar(1);
            var resultado = _repositorio.ObtenerPorId(1);

            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Eliminar_ConIdInexistente_DeberiaLanzarExcepcion()
        {
            _repositorio.Eliminar(999);
        }
        
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Actualizar_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            usuario.Id = 999;

            _repositorio.Actualizar(usuario);
        }
    }
}