using Dominio.Entidades;

namespace Repositorios
{
    public class AuditoriaRepositorio : IAuditoriaRepositorio
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
    }
}