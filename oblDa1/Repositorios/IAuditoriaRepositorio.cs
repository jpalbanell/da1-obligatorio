using Dominio.Entidades;

namespace Repositorios
{
    public interface IAuditoriaRepositorio
    {
        void Agregar(LogAuditoria log);
    }
}