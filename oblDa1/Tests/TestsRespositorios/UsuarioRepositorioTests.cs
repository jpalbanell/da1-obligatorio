using Dominio.Entidades;
using Repositorios;

namespace Tests
{
    [TestClass]
    public class UsuarioRepositorioTests
    {
        private IUsuarioRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new UsuarioRepositorio();
        }

        private Usuario CrearUsuarioValido(string nombre, string apellido, string email)
        {
            var usuario = new Usuario();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Email = email;
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
    }
}