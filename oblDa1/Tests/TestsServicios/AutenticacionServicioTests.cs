using Dominio.Entidades;
using Repositorios;
using Servicios;

namespace Tests
{
    [TestClass]
    public class AutenticacionServicioTests
    {
        private IUsuarioRepositorio _repositorio;
        private IUsuarioServicio _usuarioServicio;
        private IAutenticacionServicio _autenticacionServicio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new UsuarioRepositorio();
            _usuarioServicio = new UsuarioServicio(_repositorio);
            _autenticacionServicio = new AutenticacionServicio(_repositorio);
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
        public void Login_ConCredencialesValidas_DeberiaRetornarUsuario()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(usuario);

            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "Abcdef1@");

            Assert.IsNotNull(resultado);
            Assert.AreEqual("Juan", resultado.Nombre);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Login_ConEmailInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.Login("noexiste@ejemplo.com", "Abcdef1@");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Login_ConContrasenaIncorrecta_DeberiaLanzarExcepcion()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(usuario);

            _autenticacionServicio.Login("juan@ejemplo.com", "Incorrecta@1");
        }
        
        [TestMethod]
        public void CambiarContrasena_ConContrasenaValida_DeberiaActualizar()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(usuario);

            _autenticacionServicio.CambiarContrasena(usuario.Id, "NuevaPass@1");
            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "NuevaPass@1");

            Assert.IsNotNull(resultado);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CambiarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.CambiarContrasena(999, "NuevaPass@1");
        }
        
        [TestMethod]
        public void ReiniciarContrasena_ConUsuarioExistente_DeberiaAsignarContrasenaPorDefecto()
        {
            var usuario = CrearUsuarioValido("Juan", "Pérez", "juan@ejemplo.com");
            _usuarioServicio.AgregarUsuario(usuario);

            _autenticacionServicio.ReiniciarContrasena(usuario.Id);
            var resultado = _autenticacionServicio.Login("juan@ejemplo.com", "Password@1");

            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReiniciarContrasena_ConUsuarioInexistente_DeberiaLanzarExcepcion()
        {
            _autenticacionServicio.ReiniciarContrasena(999);
        }
    }
}