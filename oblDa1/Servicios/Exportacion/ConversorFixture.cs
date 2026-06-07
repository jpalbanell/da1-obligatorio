using Dominio.Entidades;

namespace Servicios.Exportacion
{
    public static class ConversorFixture
    {
        private const string FormatoFecha = "yyyy-MM-dd HH:mm:ss";
        private const string SinDefinir = "Por definir";

        public static TablaExportable ATabla(List<Partido> partidos)
        {
            var tabla = new TablaExportable
            {
                Encabezados = new List<string>
                {
                    "Codigo", "Fase", "Local", "Visitante",
                    "GolesLocal", "GolesVisitante", "Fecha", "Estadio"
                }
            };

            foreach (var partido in partidos)
            {
                tabla.Filas.Add(new List<string>
                {
                    partido.Codigo,
                    partido.Fase.ToString(),
                    partido.EquipoLocal?.Nombre ?? SinDefinir,
                    partido.EquipoVisitante?.Nombre ?? SinDefinir,
                    partido.GolesLocal.ToString(),
                    partido.GolesVisitante.ToString(),
                    partido.Fecha.ToString(FormatoFecha),
                    partido.Estadio?.Nombre ?? SinDefinir
                });
            }

            return tabla;
        }
    }
}