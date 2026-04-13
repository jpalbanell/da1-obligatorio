namespace Dominio;

public class Partido
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public DateTime Fecha { get; set; }

    public Partido(int id, string codigo = "", DateTime fecha = default)
    {
        Id = id;
        Codigo = codigo;
        Fecha = fecha;
    }
}