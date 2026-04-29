using Dominio.Entidades;

namespace Servicios
{
    public class SesionServicio : ISesionServicio
    {
        private Usuario _usuarioActual;

        public void IniciarSesion(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException("Usuario no puede ser nulo");
            _usuarioActual = usuario;
        }

        public Usuario ObtenerUsuarioActual()
        {
            return _usuarioActual;
        }
        
        public void CerrarSesion()
        {
            _usuarioActual = null;
        }
    }
}