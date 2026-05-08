namespace Dominio.Entidades
{
    public class ResultadoImportacion
    {
        public int EquiposImportados { get; set; }
        public List<string> Errores { get; set; } = new List<string>();
    }
}