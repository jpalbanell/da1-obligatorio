using Dominio.Entidades;

namespace Web.DTOs
{
    public class EstadioResponse
    {
        public string Nombre { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Capacidad { get; set; }

        public static EstadioResponse FromEntity(Estadio estadio) => new()
        {
            Nombre = estadio.Nombre,
            Ciudad = estadio.Ciudad,
            Descripcion = estadio.Descripcion ?? "",
            Capacidad = estadio.Capacidad
        };
    }
}