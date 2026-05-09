using Dominio.Entidades;
namespace Web.DTOs;

public class ImportarEquiposResponse
{
    public int EquiposImportados { get; set; }
    public List<string> Errores { get; set; } = new();

    public static ImportarEquiposResponse FromEntity(ResultadoImportacion resultado) => new()
    {
        EquiposImportados = resultado.EquiposImportados,
        Errores = resultado.Errores
    };
}