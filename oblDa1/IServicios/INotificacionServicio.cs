using Dominio.Entidades;

namespace IServicios
{
    public interface INotificacionServicio
    {
        void Notificar(string mensaje, Usuario periodista);
        void NotificarPorRol(string mensaje, Rol rol);
        List<Notificacion> ObtenerNoLeidas(Usuario usuario);
        List<Notificacion> ObtenerTodas(Usuario usuario);
        void MarcarLeida(int notificacionId);
    }
}
