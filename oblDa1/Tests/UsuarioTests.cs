using Dominio.Entidades;

namespace Tests
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void CrearUsuario_ConRolAdministrador_DeberiaContenerRol()
        {
            var usuario = new Usuario();
            usuario.Roles.Add(Rol.Administrador);
            Assert.IsTrue(usuario.Roles.Contains(Rol.Administrador));
        }
        
        [TestMethod]
        public void CrearUsuario_ConNombreValido_DeberiaAsignarNombre()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            Assert.AreEqual("Juan", usuario.Nombre);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConNombreVacio_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = "";
        }
    }
}