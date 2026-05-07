using Dominio.Entidades;

namespace Web.DTOs
{
    public class LogAuditoriaResponse
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Accion { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public string EmailUsuario { get; set; } = "";

        public static LogAuditoriaResponse FromEntity(LogAuditoria log)
        {
            return new LogAuditoriaResponse
            {
                Id = log.Id,
                Timestamp = log.Timestamp,
                Accion = log.Accion,
                NombreUsuario = $"{log.Usuario.Nombre} {log.Usuario.Apellido}",
                EmailUsuario = log.Usuario.Email
            };
        }
    }
}