using Dominio.Entidades;
using Dominio;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class TorneoServicio : ITorneoServicio
    {
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IEstadioRepositorio _estadioRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public TorneoServicio(
            IEquipoRepositorio equipoRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IGrupoRepositorio grupoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _grupoRepositorio = grupoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.AgregarEquipo(equipo);
            _equipoRepositorio.Agregar(equipo);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Alta de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private Fixture ObtenerOCrearFixture()
        {
            return _fixtureRepositorio.Obtener() ?? new Fixture();
        }
        
        public List<Equipo> ObtenerTodos()
        {
            return _equipoRepositorio.ObtenerTodos();
        }
        
        public void EditarEquipo(Equipo equipo, string nombreOriginal)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EditarEquipo(equipo, nombreOriginal);
            _equipoRepositorio.Actualizar(equipo, nombreOriginal);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Edición de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Equipo ObtenerPorNombre(string nombre)
        {
            return _equipoRepositorio.ObtenerPorNombre(nombre);
        }
        
        public void EliminarEquipo(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEquipo(nombre);
            _equipoRepositorio.Eliminar(nombre);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Eliminación de equipo: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void CompletarEquiposAutomaticamente(int semillaCompletar)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var random = new Random(semillaCompletar);
            var resumen = new System.Text.StringBuilder();
            resumen.Append($"Generación automática de equipos con semilla: {semillaCompletar}. ");

            foreach (Confederacion confederacion in Enum.GetValues(typeof(Confederacion)))
            {
                int cupo = confederacion.CupoMaximo();
                int cantActual = _equipoRepositorio.ObtenerTodos()
                    .Count(e => e.Confederacion == confederacion);
                int cantGenerada = 0;
                int contador = cantActual + 1;

                for (int i = cantActual + 1; i <= cupo; i++)
                {
                    var equipo = new Equipo();
                    equipo.Nombre = $"{confederacion}_{contador:D2}";
                    equipo.Confederacion = confederacion;
                    equipo.RankingFifa = random.Next(300, 2501);
                    var fixture = ObtenerOCrearFixture();
                    fixture.AgregarEquipo(equipo);
                    _equipoRepositorio.Agregar(equipo);
                    _fixtureRepositorio.Guardar(fixture);
                    contador++;
                    cantGenerada++;
                }

                resumen.Append($"{confederacion}: {cantGenerada} equipos generados. ");
            }

            resumen.Append("Rangos: 300-2500.");
            _auditoriaServicio.Registrar(resumen.ToString(), _sesionServicio.ObtenerUsuarioActual());
        }
        
        public void AgregarEstadio(Estadio estadio)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.AgregarEstadio(estadio);
            _estadioRepositorio.Agregar(estadio);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Alta de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void ModificarEstadio(Estadio estadio, string nombreOriginal)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEstadio(nombreOriginal);
            fixture.AgregarEstadio(estadio);
            _estadioRepositorio.Actualizar(estadio, nombreOriginal);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Edición de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void EliminarEstadio(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEstadio(nombre);
            _estadioRepositorio.Eliminar(nombre);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Eliminación de estadio: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Estadio ObtenerEstadio(string nombre)
        {
            return _estadioRepositorio.ObtenerPorNombre(nombre);
        }

        public List<Estadio> ObtenerTodosEstadios()
        {
            return _estadioRepositorio.ObtenerTodos();
        }
        
        private const int FilaEncabezado = 0;
        private const int CantidadColumnasEsperadas = 3;
        private const int ColumnaNombre = 0;
        private const int ColumnaConfederacion = 1;
        private const int ColumnaRankingFifa = 2;
        private const char SeparadorDeColumnas = ',';
        
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
            return equipo;
        }

        private void ValidarCantidadDeColumnas(string[] columnas)
        {
            if (columnas.Length != CantidadColumnasEsperadas)
                throw new FormatException($"Se esperaban {CantidadColumnasEsperadas} columnas pero se encontraron {columnas.Length}.");
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
    }
}