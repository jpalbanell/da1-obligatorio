using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class NotificacionServicio : INotificacionServicio
    {
        private readonly INotificacionRepositorio _notificacionRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public NotificacionServicio(INotificacionRepositorio notificacionRepositorio, IUsuarioRepositorio usuarioRepositorio)
        {
            _notificacionRepositorio = notificacionRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public void Notificar(string mensaje, Usuario periodista)
        {
            var notificacion = new Notificacion();
            notificacion.Mensaje = mensaje;
            notificacion.FechaCreacion = DateTime.Now;
            notificacion.Periodista = periodista;
            _notificacionRepositorio.Agregar(notificacion);
        }

        public void NotificarPorRol(string mensaje, Rol rol)
        {
            var usuarios = _usuarioRepositorio.ObtenerPorRol(rol);
            foreach (var usuario in usuarios)
                Notificar(mensaje, usuario);
        }

        public List<Notificacion> ObtenerNoLeidas(Usuario usuario)
        {
            return _notificacionRepositorio.ObtenerNoLeidasPorUsuario(usuario);
        }

        public List<Notificacion> ObtenerTodas(Usuario usuario)
        {
            return _notificacionRepositorio.ObtenerPorUsuario(usuario);
        }

        public void MarcarLeida(int notificacionId)
        {
            var notificacion = _notificacionRepositorio.ObtenerPorId(notificacionId);
            if (notificacion == null)
                throw new KeyNotFoundException("Notificación no encontrada.");
            notificacion.MarcarLeida();
            _notificacionRepositorio.Actualizar(notificacion);
        }
    }
}
