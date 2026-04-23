using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class ImportacionServicio : IImportacionServicio
    {
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly CsvLineaMapeador _mapeador;

        private const string EncabezadoEsperado = "Nombre,Confederación,RankingFIFA";

        public ImportacionServicio(IEquipoRepositorio equipoRepositorio)
        {
            _equipoRepositorio = equipoRepositorio;
            _mapeador = new CsvLineaMapeador();
        }

        public ResultadoImportacion ImportarEquipos(string contenidoCsv)
        {
            var resultado = new ResultadoImportacion();
            var lineas = ObtenerLineasDeDatos(contenidoCsv);

            foreach (var (linea, numeroLinea) in lineas)
            {
                ProcesarLinea(linea, numeroLinea, resultado);
            }

            return resultado;
        }

        private IEnumerable<(string linea, int numero)> ObtenerLineasDeDatos(string contenidoCsv)
        {
            var todasLasLineas = contenidoCsv
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            return todasLasLineas
                .Skip(1)
                .Select((linea, indice) => (linea, indice + 2));
        }

        private void ProcesarLinea(string linea, int numeroLinea, ResultadoImportacion resultado)
        {
            try
            {
                var equipo = _mapeador.Mapear(linea.Split(','));
                ValidarNombreUnico(equipo.Nombre);
                _equipoRepositorio.Agregar(equipo);
                resultado.EquiposImportados++;
            }
            catch (Exception ex)
            {
                resultado.Errores.Add($"Línea {numeroLinea}: {ex.Message}");
            }
        }

        private void ValidarNombreUnico(string nombre)
        {
            if (_equipoRepositorio.ObtenerPorNombre(nombre) != null)
                throw new Exception($"Ya existe un equipo con el nombre '{nombre}'.");
        }
    }
}