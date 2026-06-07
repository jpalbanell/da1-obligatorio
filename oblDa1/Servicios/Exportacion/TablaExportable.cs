namespace Servicios.Exportacion
{
    public class TablaExportable
    {
        public List<string> Encabezados { get; set; } = new List<string>();
        public List<List<string>> Filas { get; set; } = new List<List<string>>();
    }
}