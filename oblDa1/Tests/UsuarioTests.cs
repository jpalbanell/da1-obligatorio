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
    }
}