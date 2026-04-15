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
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConNombreNulo_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = null;
        }
        
        [TestMethod]
        public void CrearUsuario_ConApellidoValido_DeberiaAsignarApellido()
        {
            var usuario = new Usuario();
            usuario.Apellido = "Pérez";
            Assert.AreEqual("Pérez", usuario.Apellido);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConApellidoVacio_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Apellido = "";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConApellidoNulo_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Apellido = null;
        }
        
        [TestMethod]
        public void CrearUsuario_ConEmailValido_DeberiaAsignarEmail()
        {
            var usuario = new Usuario();
            usuario.Email = "juan@ejemplo.com";
            Assert.AreEqual("juan@ejemplo.com", usuario.Email);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConEmailVacio_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Email = "";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConEmailNulo_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Email = null;
        }
    }
}