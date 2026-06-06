namespace Dominio.Entidades;

public class Incidencia
{
    private int _cantidad;
    private Equipo _equipo;

    public int Id { get; set; }

    public TipoIncidencia Tipo { get; set; }

    public virtual Equipo Equipo
    {
        get => _equipo;
        set
        {
            if (value == null)
                throw new ArgumentException("Equipo no puede ser nulo");
            _equipo = value;
        }
    }

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
    