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
        public bool CargarIncidencias { get; set; }
        public int TarjetasAmarillasLocal { get; set; }
        public int TarjetasRojasLocal { get; set; }
        public int TarjetasAmarillasVisitante { get; set; }
        public int TarjetasRojasVisitante { get; set; }

        public static EditarPartidoRequest FromEntity(Partido partido)
        {
            var amarillasLocal = partido.Incidencias
                .FirstOrDefault(i => i.Tipo == TipoIncidencia.TarjetaAmarilla && i.Equipo == partido.EquipoLocal)?.Cantidad ?? 0;
            var rojasLocal = partido.Incidencias
                .FirstOrDefault(i => i.Tipo == TipoIncidencia.TarjetaRoja && i.Equipo == partido.EquipoLocal)?.Cantidad ?? 0;
            var amarillasVisitante = partido.Incidencias
                .FirstOrDefault(i => i.Tipo == TipoIncidencia.TarjetaAmarilla && i.Equipo == partido.EquipoVisitante)?.Cantidad ?? 0;
            var rojasVisitante = partido.Incidencias
                .FirstOrDefault(i => i.Tipo == TipoIncidencia.TarjetaRoja && i.Equipo == partido.EquipoVisitante)?.Cantidad ?? 0;

            return new EditarPartidoRequest
            {
                Id = partido.Id,
                Fecha = partido.Fecha,
                EstadioNombre = partido.Estadio?.Nombre ?? "",
                GolesLocal = partido.GolesLocal,
                GolesVisitante = partido.GolesVisitante,
                CargarResultado = partido.TieneResultado,
                CargarIncidencias = partido.Incidencias.Any(),
                TarjetasAmarillasLocal = amarillasLocal,
                TarjetasRojasLocal = rojasLocal,
                TarjetasAmarillasVisitante = amarillasVisitante,
                TarjetasRojasVisitante = rojasVisitante
            };
        }
    }
}
