using Dominio.Entidades;

namespace Web.DTOs
{
    public class PartidoResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public DateTime Fecha { get; set; }
        public string Fase { get; set; } = "";
        public string EquipoLocal { get; set; } = "";
        public string EquipoVisitante { get; set; } = "";
        public string Estadio { get; set; } = "";
        public string Grupo { get; set; } = "";
        public int GolesLocal { get; set; }
        public int GolesVisitante { get; set; }
        public string Vencedor { get; set; } = "";
        public bool TieneResultado { get; set; }
        public bool EstaBloqueado { get; set; }

        public static PartidoResponse FromEntity(Partido partido)
        {
            return new PartidoResponse
            {
                Id = partido.Id,
                Codigo = partido.Codigo,
                Fecha = partido.Fecha,
                Fase = partido.Fase.ToString(),
                EquipoLocal = partido.EquipoLocal?.Nombre ?? "",
                EquipoVisitante = partido.EquipoVisitante?.Nombre ?? "",
                Estadio = partido.Estadio?.Nombre ?? "",
                Grupo = partido.Grupo?.Etiqueta ?? "",
                GolesLocal = partido.GolesLocal,
                GolesVisitante = partido.GolesVisitante,
                Vencedor = partido.Vencedor?.Nombre ?? "",
                TieneResultado = partido.TieneResultado,
                EstaBloqueado = partido.EstaBloqueado
            };
        }
    }
}