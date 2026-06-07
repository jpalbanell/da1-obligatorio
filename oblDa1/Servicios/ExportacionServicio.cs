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

        public byte[] ExportarFixture(FormatoExportacion formato)
        {
            _sesionServicio.ValidarAlgunRol(Rol.Administrador, Rol.Periodista);
            var partidos = _torneoServicio.ObtenerTodosPartidos();
            var tabla = ConversorFixture.ATabla(partidos);
            return ObtenerExportador(formato).Exportar(tabla);
        }

        public byte[] ExportarAuditoria(FormatoExportacion formato, DateTime desde, DateTime hasta)
        {
            _sesionServicio.ValidarAlgunRol(Rol.Administrador, Rol.Periodista);
            var logs = _auditoriaServicio.ObtenerEntreFechas(desde, hasta);
            var tabla = ConversorAuditoria.ATabla(logs);
            return ObtenerExportador(formato).Exportar(tabla);
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
    }
}