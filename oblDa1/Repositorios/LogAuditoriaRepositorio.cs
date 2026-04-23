using Dominio.Entidades;

namespace Repositorios
{
    public class LogAuditoriaRepositorio : ILogAuditoriaRepositorio
    {
        private List<LogAuditoria> _logs = new List<LogAuditoria>();

        public void Agregar(LogAuditoria log)
        {
            _logs.Add(log);
        }

        public List<LogAuditoria> ObtenerTodos()
        {
            return _logs;
        }

        public LogAuditoria ObtenerPorId(int id)
        {
            return _logs.FirstOrDefault(l => l.Id == id);
        }
    }
}