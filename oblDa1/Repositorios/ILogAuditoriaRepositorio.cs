using Dominio.Entidades;

namespace Repositorios
{
    public interface ILogAuditoriaRepositorio
    {
        void Agregar(LogAuditoria log);
        List<LogAuditoria> ObtenerTodos();
        LogAuditoria ObtenerPorId(int id);
    }
}