namespace Dominio.Entidades;

public class Grupo
{
    public int Id { get; set; }
    private string _etiqueta;
    
    public string Etiqueta
    {
        get => _etiqueta;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La etiqueta es obligatoria.");
            if (!new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" }.Contains(value))
                throw new ArgumentException("La etiqueta debe ser una letra entre A y L.");
            _etiqueta = value;
        }
    }
}