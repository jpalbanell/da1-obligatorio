using Dominio.Entidades;

namespace Servicios
{
    public interface ISesionServicio
    {
        void IniciarSesion(Usuario usuario);
        Usuario ObtenerUsuarioActual();
        void CerrarSesion();
        void ValidarRol(Rol rolRequerido);
    }
}