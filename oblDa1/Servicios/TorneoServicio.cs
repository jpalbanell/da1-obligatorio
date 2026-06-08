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
        private readonly INotificacionServicio _notificacionServicio;
        private readonly IMotorSimulacionSelector _motorFactory;

        public TorneoServicio(
            IEquipoRepositorio equipoRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IGrupoRepositorio grupoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio,
            INotificacionServicio notificacionServicio,
            IMotorSimulacionSelector motorFactory)
        {
            _equipoRepositorio = equipoRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _grupoRepositorio = grupoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
            _notificacionServicio = notificacionServicio;
            _motorFactory = motorFactory;
        }

        public List<string> ObtenerMotoresDisponibles() => _motorFactory.ObtenerNombres().ToList();
        
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

        public void GenerarFixture(Fixture fixture)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            ValidarFixtureNoGenerado(fixture);
            ValidarCantidadEquipos();
            ValidarCantidadEstadios();

            var grupos = CrearYPersistirGrupos();
            var equiposOrdenados = OrdenarEquiposConDesempate(fixture.SemillaFixture);
            DistribuirEquiposEnGrupos(equiposOrdenados, grupos);

            var partidosPorGrupo = new List<List<Partido>>();
            foreach (var grupo in grupos)
                partidosPorGrupo.Add(grupo.GenerarPartidosFaseGrupos());

            fixture.AsignarFechasAPartidos(partidosPorGrupo);
            AsignarEstadiosEnRotacion(partidosPorGrupo);
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

                BarajadorDeterministico.BarajarSubLista(equipos, indices, random);
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
            foreach (var partidosGrupo in partidosPorGrupo)
                foreach (var partido in partidosGrupo)
                    _partidoRepositorio.Agregar(partido);
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
        
        private DateTime _fechaActualEliminatorias;
        private int _partidosEnFechaActual;
        private const int CantidadMejoresPrimeros = 8;
        private const int CantidadSegundosRestantes = 8;
        private const int CantidadMejoresTerceros = 8;

        public void GenerarCruces(int semillaCrucesFase)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var fixture = _fixtureRepositorio.Obtener();
            ValidarFixtureGenerado(fixture);
            ValidarCrucesNoGenerados(fixture);
            ValidarTodosLosPartidosTienenResultado();

            var emparejamientos = GenerarEmparejamientos(semillaCrucesFase);
            InicializarFechaEliminatorias(fixture);
            GenerarPartidosEliminatorios(emparejamientos, fixture);
            BloquearPartidosDeFaseGrupos();

            fixture.CrucesGenerados = true;
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar(
                $"Generación de cruces con SemillaCrucesFase: {semillaCrucesFase}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        private void InicializarFechaEliminatorias(Fixture fixture)
        {
            var ultimaFecha = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .Max(p => p.Fecha.Date);
            _fechaActualEliminatorias = ultimaFecha.AddDays(fixture.SeparacionEntreFechas);
            _partidosEnFechaActual = 0;
        }

        private void BloquearPartidosDeFaseGrupos()
        {
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .ToList();
            foreach (var partido in partidos)
                partido.EstaBloqueado = true;
        }

        private List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> GenerarEmparejamientos(int semilla)
        {
            var random = new Random(semilla);
            var (primeros, segundos, mejoresTerceros) = SeleccionarClasificados(random);

            var primerosOrdenados = primeros
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .ToList();

            var ochoMejoresPrimeros = primerosOrdenados.Take(CantidadMejoresPrimeros).ToList();
            var cuatroRestantesPrimeros = primerosOrdenados.Skip(CantidadMejoresPrimeros).ToList();

            var segundosOrdenados = segundos
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .ToList();

            var cuatroSegundosMenorPuntaje = segundosOrdenados.Skip(CantidadSegundosRestantes).ToList();
            var ochoSegundosRestantes = segundosOrdenados.Take(CantidadSegundosRestantes).ToList();

            BarajadorDeterministico.Barajar(ochoMejoresPrimeros, random);
            BarajadorDeterministico.Barajar(mejoresTerceros, random);
            BarajadorDeterministico.Barajar(cuatroRestantesPrimeros, random);
            BarajadorDeterministico.Barajar(cuatroSegundosMenorPuntaje, random);
            BarajadorDeterministico.Barajar(ochoSegundosRestantes, random);

            var emparejamientos = new List<(PosicionesGrupo, PosicionesGrupo, string)>();
            emparejamientos.AddRange(EmparejarListas(ochoMejoresPrimeros, mejoresTerceros, "A", 1));
            emparejamientos.AddRange(EmparejarListas(cuatroRestantesPrimeros, cuatroSegundosMenorPuntaje, "B", 1));
            emparejamientos.AddRange(EmparejarEntreSi(ochoSegundosRestantes, "B", 5));
            return emparejamientos;
        }

        private (List<PosicionesGrupo> primeros, List<PosicionesGrupo> segundos, List<PosicionesGrupo> mejoresTerceros) SeleccionarClasificados(Random random)
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var primeros = new List<PosicionesGrupo>();
            var segundos = new List<PosicionesGrupo>();
            var terceros = new List<PosicionesGrupo>();

            foreach (var grupo in grupos)
            {
                var posicionesOrdenadas = grupo.ObtenerPosicionesOrdenadas(random);
                primeros.Add(posicionesOrdenadas[0]);
                segundos.Add(posicionesOrdenadas[1]);
                terceros.Add(posicionesOrdenadas[2]);
            }

            var mejoresTerceros = terceros
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .Take(CantidadMejoresTerceros)
                .ToList();

            return (primeros, segundos, mejoresTerceros);
        }

        private List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> EmparejarListas(
            List<PosicionesGrupo> locales, List<PosicionesGrupo> visitantes, string prefijo, int numeroInicio)
        {
            var resultado = new List<(PosicionesGrupo, PosicionesGrupo, string)>();
            var visitantesDisponibles = new List<PosicionesGrupo>(visitantes);

            for (int i = 0; i < locales.Count; i++)
            {
                var local = locales[i];
                var visitante = visitantesDisponibles
                    .FirstOrDefault(v => v.EtiquetaGrupo != local.EtiquetaGrupo)
                    ?? visitantesDisponibles.First();
                visitantesDisponibles.Remove(visitante);
                resultado.Add((local, visitante, $"{prefijo}{i + 1}"));
            }

            return resultado;
        }

        private List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> EmparejarEntreSi(
            List<PosicionesGrupo> equipos, string prefijo, int numeroInicio)
        {
            var resultado = new List<(PosicionesGrupo, PosicionesGrupo, string)>();
            var disponibles = new List<PosicionesGrupo>(equipos);
            int numero = numeroInicio;

            while (disponibles.Count >= 2)
            {
                var local = disponibles[0];
                disponibles.RemoveAt(0);
                var visitante = disponibles.FirstOrDefault(v => v.EtiquetaGrupo != local.EtiquetaGrupo)
                    ?? disponibles.First();
                disponibles.Remove(visitante);
                resultado.Add((local, visitante, $"{prefijo}{numero++}"));
            }

            return resultado;
        }

        private void GenerarPartidosEliminatorios(
            List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> emparejamientos,
            Fixture fixture)
        {
            var dieciseisavos = CrearPartidosDieciseisavos(emparejamientos, fixture);
            AvanzarDeFase(fixture);
            var octavos = CrearPartidosOctavos(dieciseisavos, fixture);
            AvanzarDeFase(fixture);
            var cuartos = CrearPartidosCuartos(octavos, fixture);
            AvanzarDeFase(fixture);
            var semifinales = CrearPartidosSemifinales(cuartos, fixture);
            AvanzarDeFase(fixture);
            CrearPartidosTercerPuestoYFinal(semifinales, fixture);
        }

        private void AvanzarDeFase(Fixture fixture)
        {
            _fechaActualEliminatorias = _fechaActualEliminatorias.AddDays(fixture.SeparacionEntreFechas);
            _partidosEnFechaActual = 0;
        }

        private List<Partido> CrearPartidosDieciseisavos(
            List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> emparejamientos,
            Fixture fixture)
        {
            var partidos = new List<Partido>();
            int indiceEstadio = 0;
            foreach (var (local, visitante, codigo) in emparejamientos)
            {
                var partido = new Partido();
                partido.Codigo = codigo;
                partido.Fase = FaseTorneo.Dieciseisavos;
                partido.EquipoLocal = local.Equipo;
                partido.EquipoVisitante = visitante.Equipo;
                partido.Fecha = ObtenerProximaFechaEliminatoria(fixture.MaxPartidosPorDia);
                partido.Estadio = ObtenerEstadioRotado(indiceEstadio++);
                partido.Grupo = _grupoRepositorio.ObtenerPorEtiqueta(local.EtiquetaGrupo);
                _partidoRepositorio.Agregar(partido);
                partidos.Add(partido);
            }
            return partidos;
        }

        private List<Partido> CrearPartidosOctavos(List<Partido> dieciseisavos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < dieciseisavos.Count; i += 2)
                partidos.Add(CrearPartidoEliminatorio(
                    $"C{i / 2 + 1}", FaseTorneo.Octavos,
                    dieciseisavos[i], dieciseisavos[i + 1], fixture));
            return partidos;
        }

        private List<Partido> CrearPartidosCuartos(List<Partido> octavos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < octavos.Count; i += 2)
                partidos.Add(CrearPartidoEliminatorio(
                    $"D{i / 2 + 1}", FaseTorneo.Cuartos,
                    octavos[i], octavos[i + 1], fixture));
            return partidos;
        }

        private List<Partido> CrearPartidosSemifinales(List<Partido> cuartos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < cuartos.Count; i += 2)
                partidos.Add(CrearPartidoEliminatorio(
                    $"S{i / 2 + 1}", FaseTorneo.Semifinal,
                    cuartos[i], cuartos[i + 1], fixture));
            return partidos;
        }

        private void CrearPartidosTercerPuestoYFinal(List<Partido> semifinales, Fixture fixture)
        {
            var tercerPuesto = CrearPartidoEliminatorio(
                "TP", FaseTorneo.TercerPuesto,
                semifinales[0], semifinales[1], fixture);
            tercerPuesto.EsPorPerdedor = true;
            AvanzarDeFase(fixture);
            CrearPartidoEliminatorio("F", FaseTorneo.Final,
                semifinales[0], semifinales[1], fixture);
        }

        private Partido CrearPartidoEliminatorio(
            string codigo, FaseTorneo fase,
            Partido origenLocal, Partido origenVisitante, Fixture fixture)
        {
            var indiceEstadio = ContarPartidosEliminatorios();
            var partido = new Partido();
            partido.Codigo = codigo;
            partido.Fase = fase;
            partido.Fecha = ObtenerProximaFechaEliminatoria(fixture.MaxPartidosPorDia);
            partido.Estadio = ObtenerEstadioRotado(indiceEstadio);
            partido.Grupo = _grupoRepositorio.ObtenerTodos().First();
            partido.OrigenLocal = origenLocal;
            partido.OrigenVisitante = origenVisitante;
            _partidoRepositorio.Agregar(partido);
            return partido;
        }

        private int ContarPartidosEliminatorios()
        {
            return _partidoRepositorio.ObtenerTodos()
                .Count(p => p.Fase != FaseTorneo.FaseGrupos);
        }

        private Estadio ObtenerEstadioRotado(int indice)
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => UtilTexto.Normalizar(e.Nombre), StringComparer.Ordinal)
                .ToList();
            if (estadios.Count == 0)
                throw new InvalidOperationException("No hay estadios cargados.");
            return estadios[indice % estadios.Count];
        }

        private DateTime ObtenerProximaFechaEliminatoria(int maxPartidosPorDia)
        {
            if (_partidosEnFechaActual >= maxPartidosPorDia)
            {
                _fechaActualEliminatorias = _fechaActualEliminatorias.AddDays(1);
                _partidosEnFechaActual = 0;
            }
            var hora = 14 + (_partidosEnFechaActual * 4);
            var fecha = _fechaActualEliminatorias.AddHours(hora);
            _partidosEnFechaActual++;
            return fecha;
        }

        private void ValidarFixtureGenerado(Fixture fixture)
        {
            if (fixture == null || !fixture.EstaGenerado)
                throw new InvalidOperationException("No se puede generar cruces si el fixture no fue generado.");
        }

        private void ValidarCrucesNoGenerados(Fixture fixture)
        {
            if (fixture.CrucesGenerados)
                throw new InvalidOperationException("Los cruces ya fueron generados.");
        }

        private void ValidarTodosLosPartidosTienenResultado()
        {
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .ToList();

            if (partidos.Count == 0)
                throw new InvalidOperationException("No hay partidos de fase de grupos cargados.");

            foreach (var partido in partidos)
                if (!partido.TieneResultado)
                    throw new InvalidOperationException($"El partido {partido.Codigo} no tiene resultado cargado.");
        }
        
        public void EditarPartido(int partidoId, DateTime fecha, string nombreEstadio,
            bool cargarResultado, int golesLocal, int golesVisitante, List<Incidencia>? incidencias = null)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partido = _partidoRepositorio.ObtenerPorId(partidoId);
            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado.");
            if (!partido.PuedeModificarse())
                throw new InvalidOperationException("No se puede editar un partido bloqueado.");

            partido.Fecha = fecha;
            var estadio = _estadioRepositorio.ObtenerPorNombre(nombreEstadio);
            if (estadio == null)
                throw new InvalidOperationException("Estadio no encontrado.");
            partido.Estadio = estadio;

            if (cargarResultado)
            {
                partido.RegistrarResultado(golesLocal, golesVisitante);
                partido.Grupo?.ActualizarPosiciones(partido);
                PropagarResultado(partido);
                RecalcularRankings(partido);
                _notificacionServicio.NotificarPorRol(
                    $"El partido {partido.Id} tiene resultado cargado.",
                    Rol.Periodista);
            }

            if (incidencias != null)
            {
                partido.Incidencias.Clear();
                foreach (var i in incidencias)
                    partido.Incidencias.Add(i);
            }

            _partidoRepositorio.Actualizar(partido);
            _auditoriaServicio.Registrar(
                $"Modificación de partido: {partido.Id}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        private void RecalcularRankings(Partido partido)
        {
            int localAntes = partido.EquipoLocal.RankingFifa;
            int visitanteAntes = partido.EquipoVisitante.RankingFifa;

            partido.ActualizarRankings();

            _equipoRepositorio.Actualizar(partido.EquipoLocal, partido.EquipoLocal.Nombre);
            _equipoRepositorio.Actualizar(partido.EquipoVisitante, partido.EquipoVisitante.Nombre);

            var usuario = _sesionServicio.ObtenerUsuarioActual();
            _auditoriaServicio.Registrar(
                $"Ranking {partido.EquipoLocal.Nombre}: {localAntes} -> {partido.EquipoLocal.RankingFifa}",
                usuario);
            _auditoriaServicio.Registrar(
                $"Ranking {partido.EquipoVisitante.Nombre}: {visitanteAntes} -> {partido.EquipoVisitante.RankingFifa}",
                usuario);
        }
        
        public void SimularPartido(int partidoId, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partido = _partidoRepositorio.ObtenerPorId(partidoId);
            if (partido == null)
                throw new KeyNotFoundException("Partido no encontrado.");
            if (!partido.TieneEquiposCompletos())
                throw new InvalidOperationException("No se puede simular: faltan equipos asignados.");

            var random = new Random(semillaSimulation + partidoId);
            var motor = ObtenerMotorActual();
            var (golesLocal, golesVisitante) = motor.Simular(partido.EquipoLocal.RankingFifa, partido.EquipoVisitante.RankingFifa, random);

            partido.RegistrarResultado(golesLocal, golesVisitante, random);
            GenerarIncidencias(partido, random);
            partido.Grupo?.ActualizarPosiciones(partido);
            PropagarResultado(partido);
            RecalcularRankings(partido);
            _partidoRepositorio.Actualizar(partido);
            _auditoriaServicio.Registrar(
                $"Simulación de partido: {partidoId} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
            _notificacionServicio.NotificarPorRol(
                $"Se simuló el partido {partidoId}.",
                Rol.Periodista);
        }

        public void SimularFase(FaseTorneo fase, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == fase && !p.TieneResultado)
                .ToList();

            BloquearFaseAnterior(fase);

            var motor = ObtenerMotorActual();
            foreach (var partido in partidos)
            {
                if (!partido.TieneEquiposCompletos()) continue;
                var random = new Random(semillaSimulation + partido.Id);
                var (golesLocal, golesVisitante) = motor.Simular(partido.EquipoLocal.RankingFifa, partido.EquipoVisitante.RankingFifa, random);
                partido.RegistrarResultado(golesLocal, golesVisitante, random);
                GenerarIncidencias(partido, random);
                partido.Grupo?.ActualizarPosiciones(partido);
                PropagarResultado(partido);
                _partidoRepositorio.Actualizar(partido);
            }

            _auditoriaServicio.Registrar(
                $"Simulación de fase: {fase} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
            _notificacionServicio.NotificarPorRol(
                $"Se simuló la fase {fase}.",
                Rol.Periodista);
        }

        private IMotorSimulacion ObtenerMotorActual()
        {
            var fixture = _fixtureRepositorio.Obtener();
            var nombre = fixture?.NombreMotorSimulacion ?? "Probabilístico";
            return _motorFactory.Obtener(nombre);
        }

        private const int MaxTarjetasAmarillas = 4;
        private const int MaxTarjetasRojas = 2;

        private void GenerarIncidencias(Partido partido, Random random)
        {
            partido.Incidencias.Clear();
            AgregarIncidenciasEquipo(partido, partido.EquipoLocal, random);
            AgregarIncidenciasEquipo(partido, partido.EquipoVisitante, random);
        }

        private void AgregarIncidenciasEquipo(Partido partido, Equipo equipo, Random random)
        {
            var amarillas = random.Next(1, MaxTarjetasAmarillas + 1);
            partido.Incidencias.Add(new Incidencia { Tipo = TipoIncidencia.TarjetaAmarilla, Equipo = equipo, Cantidad = amarillas });

            var rojas = random.Next(0, MaxTarjetasRojas + 1);
            if (rojas > 0)
                partido.Incidencias.Add(new Incidencia { Tipo = TipoIncidencia.TarjetaRoja, Equipo = equipo, Cantidad = rojas });
        }

        private void PropagarResultado(Partido partido)
        {
            if (partido.Vencedor == null) return;
            var siguientes = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.OrigenLocal?.Id == partido.Id || p.OrigenVisitante?.Id == partido.Id)
                .ToList();
            foreach (var siguiente in siguientes)
            {
                var equipo = siguiente.EsPorPerdedor ? partido.ObtenerPerdedor() : partido.Vencedor;
                if (siguiente.OrigenLocal?.Id == partido.Id)
                    siguiente.EquipoLocal = equipo;
                else
                    siguiente.EquipoVisitante = equipo;
                _partidoRepositorio.Actualizar(siguiente);
            }
        }

        private void BloquearFaseAnterior(FaseTorneo faseActual)
        {
            if (faseActual == FaseTorneo.FaseGrupos) return;
            var faseAnterior = faseActual - 1;
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == faseAnterior)
                .ToList();
            foreach (var partido in partidos)
                partido.EstaBloqueado = true;
        }

        public Partido ObtenerPartido(int id)
        {
            return _partidoRepositorio.ObtenerPorId(id);
        }
        
            
        public List<Partido> ObtenerTodosPartidos()
        {
            return _partidoRepositorio.ObtenerTodos();
        }

        public List<Partido> ObtenerPartidosPorFase(FaseTorneo fase)
        {
            return _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == fase)
                .ToList();
        }

        public List<Partido> ObtenerPartidosPorFecha(DateTime fecha)
        {
            return _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fecha.Date == fecha.Date)
                .ToList();
        }

        public List<Partido> ObtenerPartidosPorGrupo(string etiquetaGrupo)
        {
            return _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Grupo != null && p.Grupo.Etiqueta == etiquetaGrupo)
                .ToList();
        }

        public List<Partido> ObtenerPartidosPorEstadio(string nombreEstadio)
        {
            return _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Estadio != null &&
                            p.Estadio.Nombre.Equals(nombreEstadio, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

}