using Dominio.Entidades;
using IRepositorios;
using Microsoft.EntityFrameworkCore;

namespace Repositorios
{
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private readonly SqlContext _context;

        public AuditoriaRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(LogAuditoria log)
        {
            _context.Entry(log).State = EntityState.Added;
            _context.Entry(log).Property("UsuarioId").CurrentValue = log.Usuario.Id;
            _context.SaveChanges();
        }

        public List<LogAuditoria> ObtenerTodos()
        {
            return _context.LogsAuditoria.ToList();
        }
    }
}
