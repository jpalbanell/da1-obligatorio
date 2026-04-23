using Dominio.Entidades;

namespace Servicios
{
    public class CsvLineaMapeador
    {
        private const int IndiceNombre = 0;
        private const int IndiceConfederacion = 1;
        private const int IndiceRankingFifa = 2;
        private const int CantidadColumnas = 3;

        public Equipo Mapear(string[] columnas)
        {
            ValidarCantidadColumnas(columnas);

            var equipo = new Equipo();
            equipo.Nombre = columnas[IndiceNombre].Trim();
            equipo.Confederacion = ParsearConfederacion(columnas[IndiceConfederacion].Trim());
            equipo.RankingFifa = ParsearRanking(columnas[IndiceRankingFifa].Trim());
            return equipo;
        }

        private void ValidarCantidadColumnas(string[] columnas)
        {
            if (columnas.Length != CantidadColumnas)
                throw new Exception($"Se esperaban {CantidadColumnas} columnas.");
        }

        private Confederacion ParsearConfederacion(string valor)
        {
            if (!Enum.TryParse<Confederacion>(valor, out var confederacion))
                throw new Exception($"Confederación inválida: {valor}");
            return confederacion;
        }

        private int ParsearRanking(string valor)
        {
            if (!int.TryParse(valor, out var ranking))
                throw new Exception($"RankingFIFA inválido: {valor}");
            return ranking;
        }
    }
}