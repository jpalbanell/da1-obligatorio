namespace Dominio;

public class Partido
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public DateTime Fecha { get; set; }
    public FaseTorneo Fase { get; set; }

    public Partido(int id, string codigo = "", DateTime fecha = default, FaseTorneo fase = default)
    {
        Id = id;
        Codigo = codigo;
        Fecha = fecha;
        Fase = fase;
    }
}