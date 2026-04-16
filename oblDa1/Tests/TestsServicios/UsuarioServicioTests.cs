using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class UsuarioServicioTests
    {
        private IUsuarioRepositorio _repositorio;
        private IUsuarioServicio _servicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new UsuarioRepositorio();
            _servicio = new UsuarioServicio(_repositorio);
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
        public void AgregarUsuario_ConDatosValidos_DeberiaAgregarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario);
            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(1, resultado.Count);
        }
        
        [TestMethod]
        public void AgregarUsuario_DeberiaAsignarIdAutomaticamente()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");

            _servicio.AgregarUsuario(usuario);

            Assert.AreEqual(1, usuario.Id);
        }
        
        [TestMethod]
        public void AgregarUsuario_VariosUsuarios_DeberiaAsignarIdsIncrementales()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            var usuario2 = CrearUsuarioValido("María", "López", "maria@ejemplo.com");

            _servicio.AgregarUsuario(usuario1);
            _servicio.AgregarUsuario(usuario2);

            Assert.AreEqual(1, usuario1.Id);
            Assert.AreEqual(2, usuario2.Id);
        }
        
        [TestMethod]
        public void ObtenerUsuario_ConIdExistente_DeberiaRetornarlo()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            var resultado = _servicio.ObtenerUsuario(usuario.Id);

            Assert.AreEqual("Juan", resultado.Nombre);
        }
        
        [TestMethod]
        public void ObtenerUsuario_ConIdInexistente_DeberiaRetornarNull()
        {
            var resultado = _servicio.ObtenerUsuario(999);
            Assert.IsNull(resultado);
        }
        
        [TestMethod]
        public void ObtenerTodos_ConVariosUsuarios_DeberiaRetornarTodos()
        {
            var usuario1 = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            var usuario2 = CrearUsuarioValido("María", "López", "maria@ejemplo.com");
            _servicio.AgregarUsuario(usuario1);
            _servicio.AgregarUsuario(usuario2);

            var resultado = _servicio.ObtenerTodos();

            Assert.AreEqual(2, resultado.Count);
        }
        
        [TestMethod]
        public void ModificarUsuario_ConDatosValidos_DeberiaActualizar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _servicio.AgregarUsuario(usuario);

            usuario.Nombre = "Juan Pablo";
            usuario.Apellido = "González";
            _servicio.ModificarUsuario(usuario);

            var resultado = _servicio.ObtenerUsuario(usuario.Id);
            Assert.AreEqual("Juan Pablo", resultado.Nombre);
            Assert.AreEqual("González", resultado.Apellido);
        }
    }
}