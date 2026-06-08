using Dominio.Entidades;
using IServicios;
using Servicios.Exportacion;

namespace Servicios
{
    public class ExportacionServicio : IExportacionServicio
    {
        private readonly ITorneoServicio _torneoServicio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public ExportacionServicio(
            ITorneoServicio torneoServicio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _torneoServicio = torneoServicio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        private Exportador ObtenerExportador(FormatoExportacion formato)
        {
            switch (formato)
            {
                case FormatoExportacion.CSV:
                    return new ExportadorCSV();
                case FormatoExportacion.XLSX:
                    return new ExportadorXLSX();
                default:
                    throw new ArgumentException("Formato de exportación no soportado.");
            }
        }
        
        public byte[] ExportarFixture(FormatoExportacion formato)
        {
            _sesionServicio.ValidarAlgunRol(Rol.Administrador, Rol.Periodista);
            var partidos = _torneoServicio.ObtenerTodosPartidos();
            var tabla = ConversorFixture.ATabla(partidos);
            byte[] resultado = ObtenerExportador(formato).Exportar(tabla);

            _auditoriaServicio.Registrar(
                $"Exportación de fixture en {formato}",
                _sesionServicio.ObtenerUsuarioActual());

            return resultado;
        }

        public byte[] ExportarAuditoria(FormatoExportacion formato, DateTime desde, DateTime hasta)
        {
            _sesionServicio.ValidarAlgunRol(Rol.Administrador, Rol.Periodista);
            var logs = _auditoriaServicio.ObtenerEntreFechas(desde, hasta);
            var tabla = ConversorAuditoria.ATabla(logs);
            byte[] resultado = ObtenerExportador(formato).Exportar(tabla);

            _auditoriaServicio.Registrar(
                $"Exportación de auditoría en {formato} ({desde:yyyy-MM-dd} a {hasta:yyyy-MM-dd})",
                _sesionServicio.ObtenerUsuarioActual());

            return resultado;
        }
    }
}