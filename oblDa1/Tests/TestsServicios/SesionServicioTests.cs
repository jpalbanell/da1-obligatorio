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
    }
}