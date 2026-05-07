using Dominio.Entidades;

namespace Web.DTOs
{
    public class EquipoResponse
    {
        public string Nombre { get; set; } = "";
        public string Confederacion { get; set; } = "";
        public int RankingFifa { get; set; }

        public static EquipoResponse FromEntity(Equipo equipo) => new()
        {
            Nombre = equipo.Nombre,
            Confederacion = equipo.Confederacion.ToString(),
            RankingFifa = equipo.RankingFifa
        };
    }
}