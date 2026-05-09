using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class ImportacionFacilidadComprensionServicio : IImportacionServicio
    {
        private const int FilaEncabezado = 0;
        private const int CantidadColumnasEsperadas = 3;
        private const int ColumnaNombre = 0;
        private const int ColumnaConfederacion = 1;
        private const int ColumnaRankingFifa = 2;
        private const char SeparadorDeColumnas = ',';
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public ImportacionFacilidadComprensionServicio(
            IEquipoRepositorio equipoRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public ResultadoImportacion ImportarEquipos(string contenidoCsv)
        {
            _sesionServicio.ValidarRol(Rol.Editor);

            var resultado = new ResultadoImportacion();
            var filasDeDatos = ObtenerFilasDeDatos(contenidoCsv);

            foreach (var (fila, numeroFila) in filasDeDatos)
            {
                ImportarFila(fila, numeroFila, resultado);
            }

            RegistrarAuditoria(resultado);

            return resultado;
        }

        private void RegistrarAuditoria(ResultadoImportacion resultado)
        {
            var mensaje = $"Importación de equipos: {resultado.EquiposImportados} equipos importados, {resultado.Errores.Count} errores.";
            _auditoriaServicio.Registrar(mensaje, _sesionServicio.ObtenerUsuarioActual());
        }

        private IEnumerable<(string fila, int numero)> ObtenerFilasDeDatos(string contenidoCsv)
        {
            return contenidoCsv
                .Split('\n')
                .Select(fila => fila.Trim())
                .Where(fila => !string.IsNullOrWhiteSpace(fila))
                .Skip(FilaEncabezado + 1)
                .Select((fila, indice) => (fila, indice + 2));
        }

        private void ImportarFila(string fila, int numeroFila, ResultadoImportacion resultado)
        {
            try
            {
                var equipo = ConstruirEquipoDesdeFila(fila);
                GuardarEquipo(equipo);
                resultado.EquiposImportados++;
            }
            catch (Exception excepcion)
            {
                resultado.Errores.Add(FormatearMensajeDeError(numeroFila, excepcion.Message));
            }
        }

        private Equipo ConstruirEquipoDesdeFila(string fila)
        {
            var columnas = fila.Split(SeparadorDeColumnas);
            ValidarCantidadDeColumnas(columnas);

            var equipo = new Equipo();
            equipo.Nombre = LeerNombre(columnas);
            equipo.Confederacion = LeerConfederacion(columnas);
            equipo.RankingFifa = LeerRankingFifa(columnas);
            return equipo;
        }

        private void ValidarCantidadDeColumnas(string[] columnas)
        {
            if (columnas.Length != CantidadColumnasEsperadas)
                throw new Exception($"Se esperaban {CantidadColumnasEsperadas} columnas pero se encontraron {columnas.Length}.");
        }

        private string LeerNombre(string[] columnas)
        {
            var nombre = columnas[ColumnaNombre].Trim();
            ValidarNombreUnico(nombre);
            return nombre;
        }

        private Confederacion LeerConfederacion(string[] columnas)
        {
            var valorConfederacion = columnas[ColumnaConfederacion].Trim();
            if (!Enum.TryParse<Confederacion>(valorConfederacion, out var confederacion))
                throw new Exception($"La confederación '{valorConfederacion}' no es válida.");
            return confederacion;
        }

        private int LeerRankingFifa(string[] columnas)
        {
            var valorRanking = columnas[ColumnaRankingFifa].Trim();
            if (!int.TryParse(valorRanking, out var ranking))
                throw new Exception($"El ranking '{valorRanking}' no es un número válido.");
            return ranking;
        }

        private void ValidarNombreUnico(string nombre)
        {
            var equipoExistente = _equipoRepositorio.ObtenerPorNombre(nombre);
            if (equipoExistente != null)
                throw new Exception($"Ya existe un equipo con el nombre '{nombre}'.");
        }

        private void GuardarEquipo(Equipo equipo)
        {
            _equipoRepositorio.Agregar(equipo);
        }

        private string FormatearMensajeDeError(int numeroFila, string descripcion)
        {
            return $"Fila {numeroFila}: {descripcion}";
        }
    }
}