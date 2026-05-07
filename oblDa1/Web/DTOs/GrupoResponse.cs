using Dominio.Entidades;

namespace Web.DTOs
{
    public class GrupoResponse
    {
        public string Etiqueta { get; set; } = "";
        public List<PosicionResponse> Posiciones { get; set; } = new();
        public List<PartidoGrupoResponse> Partidos { get; set; } = new();

        public static GrupoResponse FromEntity(Grupo grupo) => new()
        {
            Etiqueta = grupo.Etiqueta,
            Posiciones = grupo.ListaPosiciones
                .Select(PosicionResponse.FromEntity)
                .ToList(),
            Partidos = grupo.ListaPartidos
                .OrderBy(p => p.Fecha)
                .Select(PartidoGrupoResponse.FromEntity)
                .ToList()
        };
    }

    public class PosicionResponse
    {
        public string Equipo { get; set; } = "";
        public int Puntos { get; set; }
        public int GolesFavor { get; set; }
        public int GolesContra { get; set; }
        public int DiferenciaGoles { get; set; }

        public static PosicionResponse FromEntity(PosicionesGrupo p) => new()
        {
            Equipo = p.Equipo.Nombre,
            Puntos = p.Puntos,
            GolesFavor = p.GolesFavor,
            GolesContra = p.GolesContra,
            DiferenciaGoles = p.DiferenciaGoles
        };
    }

    public class PartidoGrupoResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = "";
        public string EquipoLocal { get; set; } = "";
        public string EquipoVisitante { get; set; } = "";
        public string Fecha { get; set; } = "";
        public string Estadio { get; set; } = "";
        public string Resultado { get; set; } = "";
        public string Vencedor { get; set; } = "";

        public static PartidoGrupoResponse FromEntity(Partido p) => new()
        {
            Id = p.Id,
            Codigo = p.Codigo,
            EquipoLocal = p.EquipoLocal?.Nombre ?? "",
            EquipoVisitante = p.EquipoVisitante?.Nombre ?? "",
            Fecha = p.Fecha != default ? p.Fecha.ToString("dd/MM/yyyy HH:mm") : "",
            Estadio = p.Estadio?.Nombre ?? "",
            Resultado = p.TieneResultado ? $"{p.GolesLocal} - {p.GolesVisitante}" : "Por jugar",
            Vencedor = p.Vencedor?.Nombre ?? (p.TieneResultado ? "Empate" : "")
        };
    }
}