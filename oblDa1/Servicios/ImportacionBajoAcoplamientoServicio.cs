using Dominio.Entidades;

namespace Servicios
{
    public class ImportacionBajoAcoplamientoServicio : IImportacionServicio
    {
        private readonly Action<Equipo> _persistirEquipo;
        private readonly Func<string, bool> _existeEquipo;

        private const string SeparadorCsv = ",";
        private const int IndiceNombre = 0;
        private const int IndiceConfederacion = 1;
        private const int IndiceRanking = 2;
        private const int CantidadColumnas = 3;

        public ImportacionBajoAcoplamientoServicio(
            Action<Equipo> persistirEquipo,
            Func<string, bool> existeEquipo)
        {
            _persistirEquipo = persistirEquipo;
            _existeEquipo = existeEquipo;
        }

        public ResultadoImportacion ImportarEquipos(string contenidoCsv)
        {
            var resultado = new ResultadoImportacion();
            var lineas = ExtraerLineasDeDatos(contenidoCsv);

            foreach (var (linea, numeroLinea) in lineas)
            {
                ProcesarLinea(linea, numeroLinea, resultado);
            }

            return resultado;
        }

        private IEnumerable<(string linea, int numero)> ExtraerLineasDeDatos(string contenidoCsv)
        {
            return contenidoCsv
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Skip(1)
                .Select((linea, indice) => (linea, indice + 2));
        }

        private void ProcesarLinea(string linea, int numeroLinea, ResultadoImportacion resultado)
        {
            try
            {
                var equipo = ParsearLinea(linea);
                ValidarNombreUnico(equipo.Nombre);
                _persistirEquipo(equipo);
                resultado.EquiposImportados++;
            }
            catch (Exception ex)
            {
                resultado.Errores.Add($"Línea {numeroLinea}: {ex.Message}");
            }
        }

        private Equipo ParsearLinea(string linea)
        {
            var columnas = linea.Split(SeparadorCsv);
            ValidarCantidadColumnas(columnas);

            var equipo = new Equipo();
            equipo.Nombre = columnas[IndiceNombre].Trim();
            equipo.Confederacion = ParsearConfederacion(columnas[IndiceConfederacion].Trim());
            equipo.RankingFifa = ParsearRanking(columnas[IndiceRanking].Trim());
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

        private void ValidarNombreUnico(string nombre)
        {
            if (_existeEquipo(nombre))
                throw new Exception($"Ya existe un equipo con el nombre '{nombre}'.");
        }
    }
}