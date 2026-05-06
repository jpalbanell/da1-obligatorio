using Dominio.Entidades;

namespace Web.DTOs
{
    public class EstadioRequest
    {
        public string Nombre { get; set; } = "";
        public string Ciudad { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Capacidad { get; set; }

        public Estadio ToEntity()
        {
            var estadio = new Estadio();
            estadio.Nombre = Nombre;
            estadio.Ciudad = Ciudad;
            if (!string.IsNullOrEmpty(Descripcion))
                estadio.Descripcion = Descripcion;
            estadio.Capacidad = Capacidad;
            return estadio;
        }
    }
}