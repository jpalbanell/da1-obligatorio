using Dominio.Entidades;

namespace IServicios
{
    public interface IAuditoriaServicio
    {
        void Registrar(string accion, Usuario usuario);
        List<LogAuditoria> ObtenerTodos();
        List<LogAuditoria> ObtenerEntreFechas(DateTime desde, DateTime hasta);
    }
}