namespace Dominio.Entidades;

public class Incidencia
{
    private int _cantidad;

    public TipoIncidencia Tipo { get; set; }
    public Equipo Equipo { get; set; }

    public int Cantidad
    {
        get => _cantidad;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Cantidad debe ser mayor a 0");
            _cantidad = value;
        }
    }
}
    