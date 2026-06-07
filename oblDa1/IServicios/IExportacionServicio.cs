using Dominio.Entidades;

namespace IServicios
{
    public interface IExportacionServicio
    {
        byte[] ExportarFixture(FormatoExportacion formato);
        byte[] ExportarAuditoria(FormatoExportacion formato, DateTime desde, DateTime hasta);
    }
}