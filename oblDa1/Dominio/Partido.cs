namespace Dominio;

public class Partido
{
    public int Id { get; set; }
    public string Codigo { get; set; }

    public Partido(int id, string codigo = "")
    {
        Id = id;
        Codigo = codigo;
    }
}