using Dominio.Entidades;

namespace Web.DTOs
{
    public class EditarPartidoRequest
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string EstadioNombre { get; set; } = "";
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public bool CargarResultado { get; set; }

        public static EditarPartidoRequest FromEntity(Partido partido)
        {
            return new EditarPartidoRequest
            {
                Id = partido.Id,
                Fecha = partido.Fecha,
                EstadioNombre = partido.Estadio?.Nombre ?? "",
                GolesLocal = partido.GolesLocal,
                GolesVisitante = partido.GolesVisitante,
                CargarResultado = partido.TieneResultado
            };
        }
    }
}