using Dominio.Entidades;

namespace Servicios.Exportacion
{
    public static class ConversorAuditoria
    {
        private const string FormatoFecha = "yyyy-MM-dd HH:mm:ss";

        public static TablaExportable ATabla(List<LogAuditoria> logs)
        {
            var tabla = new TablaExportable
            {
                Encabezados = new List<string> { "Fecha", "Accion", "Usuario" }
            };

            foreach (var log in logs)
            {
                tabla.Filas.Add(new List<string>
                {
                    log.Timestamp.ToString(FormatoFecha),
                    log.Accion,
                    log.Usuario.Email
                });
            }

            return tabla;
        }
    }
}