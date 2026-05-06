using Dominio.Entidades;
using Servicios;

namespace Tests.TestsServicios
{
    [TestClass]
    public class SesionServicioTests
    {
        private ISesionServicio _sesionServicio;

        [TestInitialize]
        public void Setup()
        {
            _sesionServicio = new SesionServicio();
        }

        [TestMethod]
        public void IniciarSesion_UsuarioValido_ObtenerUsuarioActualRetornaUsuario()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";

            _sesionServicio.IniciarSesion(usuario);

            Assert.AreEqual(usuario, _sesionServicio.ObtenerUsuarioActual());
        }
        
        [TestMethod]
        public void CerrarSesion_UsuarioLogueado_ObtenerUsuarioActualRetornaNull()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Santiago";
            _sesionServicio.IniciarSesion(usuario);

            _sesionServicio.CerrarSesion();

            Assert.IsNull(_sesionServicio.ObtenerUsuarioActual());
        }
        
        [TestMethod]
        public void ObtenerUsuarioActual_SinSesionIniciada_RetornaNull()
        {
            Assert.IsNull(_sesionServicio.ObtenerUsuarioActual());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IniciarSesion_UsuarioNulo_LanzaExcepcion()
        {
            _sesionServicio.IniciarSesion(null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidarRol_SinSesionActiva_DeberiaLanzarExcepcion()
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ValidarRol_ConRolNoAsignado_DeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            _sesionServicio.IniciarSesion(usuario);

            _sesionServicio.ValidarRol(Rol.Administrador);
        }

        [TestMethod]
        public void ValidarRol_ConRolAsignado_NoDeberiaLanzarExcepcion()
        {
            var usuario = new Usuario();
            usuario.Nombre = "Juan";
            usuario.Apellido = "Perez";
            usuario.Email = "juan@ejemplo.com";
            usuario.FechaNacimiento = new DateTime(1990, 5, 15);
            usuario.Contrasena = "Abcdef1@";
            usuario.Roles.Add(Rol.Administrador);
            _sesionServicio.IniciarSesion(usuario);

            _sesionServicio.ValidarRol(Rol.Administrador);
        }
    }
}