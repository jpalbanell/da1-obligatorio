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
        
        [TestMethod]
        public void CrearUsuario_ConFechaNacimientoValida_DeberiaAsignarFecha()
        {
            var usuario = new Usuario();
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            Assert.AreEqual(new DateTime(1990, 5, 15), usuario.FechaNacimiento);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConFechaNacimientoVacia_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.FechaNacimiento = default;
        }
        
        [TestMethod]
        public void CrearUsuario_ConContrasenaValida_DeberiaAsignarContrasena()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "Abcdef1@";
            Assert.IsNotNull(usuario.Contrasena);
        }
        
        [TestMethod]
        public void CrearUsuario_ConContrasenaValida_DeberiaGuardarCifrada()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "Abcdef1@";
            Assert.AreNotEqual("Abcdef1@", usuario.Contrasena);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConContrasenaVacia_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConContrasenaMenorA8Caracteres_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "Abc1@";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConContrasenaSinMayuscula_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "abcdef1@";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConContrasenaSinMinuscula_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "ABCDEF1@";
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_ConContrasenaSinNumero_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Contrasena = "Abcdefg@";
        }
    }
}