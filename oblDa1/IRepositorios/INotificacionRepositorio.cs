using Dominio.Entidades;

namespace IRepositorios
{
    public interface INotificacionRepositorio
    {
        void Agregar(Notificacion notificacion);
        Notificacion ObtenerPorId(int id);
        List<Notificacion> ObtenerNoLeidasPorUsuario(Usuario usuario);
        List<Notificacion> ObtenerPorUsuario(Usuario usuario);
        void Actualizar(Notificacion notificacion);
    }
}
