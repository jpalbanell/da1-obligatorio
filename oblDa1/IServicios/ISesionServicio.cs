using Dominio.Entidades;

namespace IServicios
{
    public interface ISesionServicio
    {
        void IniciarSesion(Usuario usuario);
        Usuario ObtenerUsuarioActual();
        void CerrarSesion();
        void ValidarRol(Rol rolRequerido);
        void ValidarAlgunRol(params Rol[] rolesPermitidos);
    }
}