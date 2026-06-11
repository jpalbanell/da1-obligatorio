using Dominio.Entidades;
using Dominio;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class ImportacionServicio : IImportacionServicio
    {
        private const int FilaEncabezado = 0;
        private const int CantidadColumnasMinimas = 3;
        private const int CantidadColumnasMaximas = 4;
        private const int ColumnaNombre = 0;
        private const int ColumnaConfederacion = 1;
        private const int ColumnaRankingFifa = 2;
        private const int ColumnaBandera = 3;
        private const char SeparadorDeColumnas = ',';

        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public ImportacionServicio(
            IEquipoRepositorio equipoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public ResultadoImportacion ImportarEquipos(string contenidoCsv)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var resultado = new ResultadoImportacion();
            var filasDeDatos = ObtenerFilasDeDatos(contenidoCsv);

            foreach (var (fila, numeroFila) in filasDeDatos)
                ImportarFila(fila, numeroFila, resultado);

            _auditoriaServicio.Registrar(
                $"Importación de equipos: {resultado.EquiposImportados} equipos importados, {resultado.Errores.Count} errores.",
                _sesionServicio.ObtenerUsuarioActual());

            return resultado;
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
                GuardarEquipoImportado(equipo);
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
            equipo.Bandera = LeerBandera(columnas);
            return equipo;
        }

        private void ValidarCantidadDeColumnas(string[] columnas)
        {
            if (columnas.Length < CantidadColumnasMinimas || columnas.Length > CantidadColumnasMaximas)
                throw new FormatException($"Se esperaban entre {CantidadColumnasMinimas} y {CantidadColumnasMaximas} columnas pero se encontraron {columnas.Length}.");
        }

        private string LeerNombre(string[] columnas)
        {
            return columnas[ColumnaNombre].Trim();
        }

        private Confederacion LeerConfederacion(string[] columnas)
        {
            var valorConfederacion = columnas[ColumnaConfederacion].Trim();
            if (!Enum.TryParse<Confederacion>(valorConfederacion, out var confederacion))
                throw new ArgumentException($"La confederación '{valorConfederacion}' no es válida.");
            return confederacion;
        }

        private int LeerRankingFifa(string[] columnas)
        {
            var valorRanking = columnas[ColumnaRankingFifa].Trim();
            if (!int.TryParse(valorRanking, out var ranking))
                throw new FormatException($"El ranking '{valorRanking}' no es un número válido.");
            return ranking;
        }

        private string? LeerBandera(string[] columnas)
        {
            if (columnas.Length <= ColumnaBandera)
                return null;

            var valor = columnas[ColumnaBandera].Trim();
            return string.IsNullOrEmpty(valor) ? null : valor;
        }

        private void GuardarEquipoImportado(Equipo equipo)
        {
            var fixture = ObtenerOCrearFixture();
            fixture.AgregarEquipo(equipo);
            _equipoRepositorio.Agregar(equipo);
            _fixtureRepositorio.Guardar(fixture);
        }

        private string FormatearMensajeDeError(int numeroFila, string descripcion)
        {
            return $"Fila {numeroFila}: {descripcion}";
        }

        private Fixture ObtenerOCrearFixture()
        {
            var fixture = _fixtureRepositorio.Obtener() ?? new Fixture();
            fixture.HidratarEquipos(_equipoRepositorio.ObtenerTodos());
            return fixture;
        }
    }
}
