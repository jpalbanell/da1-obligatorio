using Dominio.Entidades;

namespace IRepositorios
{
    public interface IAuditoriaRepositorio
    {
        void Agregar(LogAuditoria log);
        List<LogAuditoria> ObtenerTodos();
    }
}