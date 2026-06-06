using Dominio.Entidades;
using IRepositorios;
using Microsoft.EntityFrameworkCore;

namespace Repositorios
{
    public class NotificacionRepositorio : INotificacionRepositorio
    {
        private readonly SqlContext _context;

        public NotificacionRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Notificacion notificacion)
        {
            _context.Notificaciones.Add(notificacion);
            _context.SaveChanges();
        }

        public Notificacion ObtenerPorId(int id)
        {
            return _context.Notificaciones.Include(n => n.Periodista)
                .FirstOrDefault(n => n.Id == id);
        }

        public List<Notificacion> ObtenerNoLeidasPorUsuario(Usuario usuario)
        {
            return _context.Notificaciones.Include(n => n.Periodista)
                .Where(n => n.Periodista.Id == usuario.Id && !n.Leida)
                .ToList();
        }

        public List<Notificacion> ObtenerPorUsuario(Usuario usuario)
        {
            return _context.Notificaciones.Include(n => n.Periodista)
                .Where(n => n.Periodista.Id == usuario.Id)
                .ToList();
        }

        public void Actualizar(Notificacion notificacion)
        {
            _context.Notificaciones.Update(notificacion);
            _context.SaveChanges();
        }
    }
}
