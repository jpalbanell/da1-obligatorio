using Dominio.Entidades;
using IServicios;

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
        
        public void ValidarRol(Rol rolRequerido)
        {
            var usuario = ObtenerUsuarioActual();
            if (usuario == null)
                throw new Exception("No hay una sesión activa.");
            if (!usuario.TieneRol(rolRequerido))
                throw new Exception("No tiene permisos para realizar esta acción.");
        }
    }
}