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
    }
}