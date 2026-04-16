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
    }
}