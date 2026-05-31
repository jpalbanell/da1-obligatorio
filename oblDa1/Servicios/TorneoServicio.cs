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
        
        private const int CantidadBombos = 4;
        private const int TamanoBombo = 12;
        private int _proximoIdGrupo = 1;

        public void GenerarFixture(Fixture fixture)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            ValidarFixtureNoGenerado(fixture);
            ValidarCantidadEquipos();
            ValidarCantidadEstadios();

            _proximoIdGrupo = 1;
            var grupos = CrearYPersistirGrupos();
            var equiposOrdenados = OrdenarEquiposConDesempate(fixture.SemillaFixture);
            DistribuirEquiposEnGrupos(equiposOrdenados, grupos);

            var partidosPorGrupo = new List<List<Partido>>();
            foreach (var grupo in grupos)
                partidosPorGrupo.Add(grupo.GenerarPartidosFaseGrupos());

            fixture.AsignarFechasAPartidos(partidosPorGrupo);
            AsignarEstadiosEnRotacion(partidosPorGrupo);
            PersistirPartidos(partidosPorGrupo);
            PersistirGrupos(grupos);

            fixture.EstaGenerado = true;
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar(
                $"Generación de fixture con SemillaFixture: {fixture.SemillaFixture}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        private void ValidarCantidadEquipos()
        {
            if (_equipoRepositorio.ObtenerTodos().Count != 48)
                throw new InvalidOperationException("Se necesitan exactamente 48 equipos para generar el fixture.");
        }

        private void ValidarCantidadEstadios()
        {
            if (_estadioRepositorio.ObtenerTodos().Count < 4)
                throw new InvalidOperationException("Se necesitan al menos 4 estadios para generar el fixture.");
        }

        private void ValidarFixtureNoGenerado(Fixture fixture)
        {
            if (fixture.EstaGenerado)
                throw new InvalidOperationException("El fixture ya fue generado.");
        }

        private List<Grupo> CrearYPersistirGrupos()
        {
            var grupos = new List<Grupo>();
            foreach (var etiqueta in Grupo.EtiquetasValidas)
            {
                var grupo = new Grupo();
                grupo.Id = _proximoIdGrupo++;
                grupo.Etiqueta = etiqueta;
                grupos.Add(grupo);
            }
            return grupos;
        }

        private List<Equipo> OrdenarEquiposConDesempate(int semilla)
        {
            var equipos = _equipoRepositorio.ObtenerTodos()
                .OrderByDescending(e => e.RankingFifa)
                .ToList();

            var random = new Random(semilla);
            var empates = equipos.GroupBy(e => e.RankingFifa).Where(g => g.Count() > 1);

            foreach (var grupo in empates)
            {
                var indices = equipos
                    .Select((e, i) => new { Equipo = e, Indice = i })
                    .Where(x => x.Equipo.RankingFifa == grupo.Key)
                    .Select(x => x.Indice)
                    .ToList();

                for (int i = indices.Count - 1; i > 0; i--)
                {
                    int j = random.Next(0, i + 1);
                    var temp = equipos[indices[i]];
                    equipos[indices[i]] = equipos[indices[j]];
                    equipos[indices[j]] = temp;
                }
            }

            return equipos;
        }

        private void DistribuirEquiposEnGrupos(List<Equipo> equiposOrdenados, List<Grupo> grupos)
        {
            var bombos = Enumerable.Range(0, CantidadBombos)
                .Select(i => equiposOrdenados.Skip(i * TamanoBombo).Take(TamanoBombo).ToList())
                .ToList();
            foreach (var bombo in bombos)
                AsignarBomboAGrupos(bombo, grupos);
        }

        private void AsignarBomboAGrupos(List<Equipo> bombo, List<Grupo> grupos)
        {
            var gruposDisponibles = new List<Grupo>(grupos);
            var asignaciones = new List<(Equipo, Grupo)>();

            if (!AsignarBomboConBacktracking(bombo, 0, gruposDisponibles, asignaciones))
                throw new InvalidOperationException("No se encontró una distribución válida para el bombo actual.");

            foreach (var (equipo, grupo) in asignaciones)
                grupo.AgregarEquipo(equipo);
        }

        private bool AsignarBomboConBacktracking(
            List<Equipo> bombo, int indice,
            List<Grupo> gruposDisponibles,
            List<(Equipo, Grupo)> asignaciones)
        {
            if (indice == bombo.Count) return true;

            var equipo = bombo[indice];
            foreach (var grupo in gruposDisponibles.ToList())
            {
                if (!grupo.PuedeAgregarEquipo(equipo)) continue;

                gruposDisponibles.Remove(grupo);
                asignaciones.Add((equipo, grupo));

                if (AsignarBomboConBacktracking(bombo, indice + 1, gruposDisponibles, asignaciones))
                    return true;

                gruposDisponibles.Add(grupo);
                asignaciones.RemoveAt(asignaciones.Count - 1);
            }
            return false;
        }

        private void AsignarEstadiosEnRotacion(List<List<Partido>> partidosPorGrupo)
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => UtilTexto.Normalizar(e.Nombre), StringComparer.Ordinal)
                .ToList();

            var todosLosPartidos = partidosPorGrupo.SelectMany(p => p).ToList();
            for (int i = 0; i < todosLosPartidos.Count; i++)
                todosLosPartidos[i].Estadio = estadios[i % estadios.Count];
        }

        private void PersistirPartidos(List<List<Partido>> partidosPorGrupo)
        {
            int proximoId = 1;
            foreach (var partidosGrupo in partidosPorGrupo)
                foreach (var partido in partidosGrupo)
                {
                    partido.Id = proximoId++;
                    _partidoRepositorio.Agregar(partido);
                }
        }

        private void PersistirGrupos(List<Grupo> grupos)
        {
            foreach (var grupo in grupos)
                _grupoRepositorio.Agregar(grupo);
        }

        public List<Grupo> ObtenerGrupos()
        {
            return _grupoRepositorio.ObtenerTodos();
        }

        public Fixture ObtenerFixture()
        {
            return _fixtureRepositorio.Obtener();
        }
    }
}