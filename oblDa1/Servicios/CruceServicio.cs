using Dominio.Entidades;
using Dominio;
using Repositorios;

namespace Servicios
{
    public class CruceServicio : ICruceServicio
    {
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IEstadioRepositorio _estadioRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        
        private DateTime _fechaActualEliminatorias;
        private int _partidosEnFechaActual;
        
        private const int CantidadMejoresPrimeros = 8;
        private const int CantidadSegundosRestantes = 8;
        private const int CantidadMejoresTerceros = 8;

        public CruceServicio(
            IGrupoRepositorio grupoRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _grupoRepositorio = grupoRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

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

            _auditoriaServicio.Registrar($"Generación de cruces con SemillaCrucesFase: {semillaCrucesFase}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private void InicializarFechaEliminatorias(Fixture fixture)
        {
            var ultimaFechaGrupos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .Max(p => p.Fecha.Date);

            _fechaActualEliminatorias = ultimaFechaGrupos.AddDays(fixture.SeparacionEntreFechas);
            _partidosEnFechaActual = 0;
        }

        private void BloquearPartidosDeFaseGrupos()
        {
            var partidosFaseGrupos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .ToList();

            foreach (var partido in partidosFaseGrupos)
            {
                partido.EstaBloqueado = true;
            }
        }
        private void ValidarFixtureGenerado(Fixture fixture)
        {
            if (fixture == null || !fixture.EstaGenerado)
                throw new Exception("No se puede generar cruces si el fixture no fue generado.");
        }
        
        private void ValidarCrucesNoGenerados(Fixture fixture)
        {
            if (fixture.CrucesGenerados)
                throw new Exception("Los cruces ya fueron generados.");
        }
        
        private void ValidarTodosLosPartidosTienenResultado()
        {
            var partidosFaseGrupos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == FaseTorneo.FaseGrupos)
                .ToList();

            if (partidosFaseGrupos.Count == 0)
                throw new Exception("No hay partidos de fase de grupos cargados.");

            foreach (var partido in partidosFaseGrupos)
            {
                if (!partido.TieneResultado)
                    throw new Exception($"El partido {partido.Codigo} no tiene resultado cargado.");
            }
        }
        
        private List<PosicionesGrupo> CalcularPosicionesGrupo(Grupo grupo)
        {
            var posiciones = new Dictionary<Equipo, PosicionesGrupo>();

            foreach (var partido in grupo.ListaPartidos)
            {
                if (!posiciones.ContainsKey(partido.EquipoLocal))
                    posiciones[partido.EquipoLocal] = CrearPosicionVacia(partido.EquipoLocal, grupo);
                if (!posiciones.ContainsKey(partido.EquipoVisitante))
                    posiciones[partido.EquipoVisitante] = CrearPosicionVacia(partido.EquipoVisitante, grupo);

                ActualizarPosicion(posiciones[partido.EquipoLocal], partido.GolesLocal, partido.GolesVisitante);
                ActualizarPosicion(posiciones[partido.EquipoVisitante], partido.GolesVisitante, partido.GolesLocal);
            }

            return posiciones.Values.ToList();
        }

        private PosicionesGrupo CrearPosicionVacia(Equipo equipo, Grupo grupo)
        {
            var posicion = new PosicionesGrupo();
            posicion.Equipo = equipo;
            posicion.Grupo = grupo;
            return posicion;
        }

        private void ActualizarPosicion(PosicionesGrupo posicion, int golesFavor, int golesContra)
        {
            posicion.GolesFavor += golesFavor;
            posicion.GolesContra += golesContra;
            posicion.DiferenciaGoles = posicion.GolesFavor - posicion.GolesContra;
            posicion.Puntos += PosicionesGrupo.CalcularPuntos(golesFavor, golesContra);
        }
        
        private List<PosicionesGrupo> ObtenerClasificados(Random random)
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var todasLasPosiciones = new List<PosicionesGrupo>();

            foreach (var grupo in grupos)
            {
                var posicionesGrupo = CalcularPosicionesGrupo(grupo);
                var ordenadas = OrdenarPosiciones(posicionesGrupo, random);
                todasLasPosiciones.AddRange(ordenadas);
            }

            return todasLasPosiciones;
        }

        private List<PosicionesGrupo> OrdenarPosiciones(List<PosicionesGrupo> posiciones, Random random)
        {
            var ordenadas = posiciones
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .GroupBy(p => new { p.Puntos, p.DiferenciaGoles, p.GolesFavor })
                .SelectMany(g => g.OrderBy(_ => random.Next()))
                .ToList();

            return ordenadas;
        }
        
        private (List<PosicionesGrupo> primeros, List<PosicionesGrupo> segundos, List<PosicionesGrupo> mejoresTerceros) SeleccionarClasificados(Random random)
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var primeros = new List<PosicionesGrupo>();
            var segundos = new List<PosicionesGrupo>();
            var terceros = new List<PosicionesGrupo>();

            foreach (var grupo in grupos)
            {
                var posicionesOrdenadas = OrdenarPosiciones(CalcularPosicionesGrupo(grupo), random);
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
            
            AplicarFisherYates(ochoMejoresPrimeros, random);
            AplicarFisherYates(mejoresTerceros, random);
            AplicarFisherYates(cuatroRestantesPrimeros, random);
            AplicarFisherYates(cuatroSegundosMenorPuntaje, random);
            AplicarFisherYates(ochoSegundosRestantes, random);

            var emparejamientos = new List<(PosicionesGrupo, PosicionesGrupo, string)>();

            emparejamientos.AddRange(EmparejarListas(ochoMejoresPrimeros, mejoresTerceros, "A"));
            emparejamientos.AddRange(EmparejarListas(cuatroRestantesPrimeros, cuatroSegundosMenorPuntaje, "B"));
            emparejamientos.AddRange(EmparejarEntreSi(ochoSegundosRestantes, "B", 5));

            return emparejamientos;
        }

        private void AplicarFisherYates<T>(List<T> lista, Random random)
        {
            for (int i = lista.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }

        private List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> EmparejarListas(
            List<PosicionesGrupo> locals, List<PosicionesGrupo> visitantes, string prefijo)
        {
            var resultado = new List<(PosicionesGrupo, PosicionesGrupo, string)>();
            var visitantesDisponibles = new List<PosicionesGrupo>(visitantes);

            for (int i = 0; i < locals.Count; i++)
            {
                var local = locals[i];
                var visitante = visitantesDisponibles
                    .FirstOrDefault(v => v.Grupo.Etiqueta != local.Grupo.Etiqueta);

                if (visitante == null)
                    visitante = visitantesDisponibles.First();

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

                var visitante = disponibles
                    .FirstOrDefault(v => v.Grupo.Etiqueta != local.Grupo.Etiqueta);

                if (visitante == null)
                    visitante = disponibles.First();

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
                var partido = new Partido(ObtenerProximoIdPartido());
                partido.Codigo = codigo;
                partido.Fase = FaseTorneo.Dieciseisavos;
                partido.EquipoLocal = local.Equipo;
                partido.EquipoVisitante = visitante.Equipo;
                partido.Fecha = ObtenerProximaFechaEliminatoria(fixture.MaxPartidosPorDia);
                partido.Estadio = ObtenerEstadioRotado(indiceEstadio++);
                partido.Grupo = ObtenerGrupoPorEtiqueta(local.Grupo.Etiqueta);
                _partidoRepositorio.Agregar(partido);
                partidos.Add(partido);
            }
            return partidos;
        }

        private List<Partido> CrearPartidosOctavos(List<Partido> dieciseisavos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < dieciseisavos.Count; i += 2)
            {
                var partido = CrearPartidoEliminatorio(
                    $"C{i / 2 + 1}",
                    FaseTorneo.Octavos,
                    dieciseisavos[i],
                    dieciseisavos[i + 1],
                    fixture
                );
                partidos.Add(partido);
            }
            return partidos;
        }
        
        private List<Partido> CrearPartidosCuartos(List<Partido> octavos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < octavos.Count; i += 2)
            {
                var partido = CrearPartidoEliminatorio(
                    $"D{i / 2 + 1}",
                    FaseTorneo.Cuartos,
                    octavos[i],
                    octavos[i + 1],
                    fixture
                );
                partidos.Add(partido);
            }
            return partidos;
        }

        private List<Partido> CrearPartidosSemifinales(List<Partido> cuartos, Fixture fixture)
        {
            var partidos = new List<Partido>();
            for (int i = 0; i < cuartos.Count; i += 2)
            {
                var partido = CrearPartidoEliminatorio(
                    $"S{i / 2 + 1}",
                    FaseTorneo.Semifinal,
                    cuartos[i],
                    cuartos[i + 1],
                    fixture
                );
                partidos.Add(partido);
            }
            return partidos;
        }

        private void CrearPartidosTercerPuestoYFinal(List<Partido> semifinales, Fixture fixture)
        {
            var tercerPuesto = CrearPartidoEliminatorio("TP", FaseTorneo.TercerPuesto, semifinales[0], semifinales[1], fixture);
            tercerPuesto.EsPorPerdedor = true;
            AvanzarDeFase(fixture);
            CrearPartidoEliminatorio("F", FaseTorneo.Final, semifinales[0], semifinales[1], fixture);
        }
        
        private Partido CrearPartidoEliminatorio(string codigo, FaseTorneo fase, Partido origenLocal, Partido origenVisitante, Fixture fixture)
        {
            var indiceEstadio = ContarPartidosEliminatorios();
            var partido = new Partido(ObtenerProximoIdPartido());
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

        private int ObtenerProximoIdPartido()
        {
            var partidos = _partidoRepositorio.ObtenerTodos();
            if (partidos.Count == 0) return 1;
            return partidos.Max(p => p.Id) + 1;
        }

        private Estadio ObtenerEstadioRotado(int indice)
        {
            var estadios = _estadioRepositorio.ObtenerTodos()
                .OrderBy(e => UtilTexto.Normalizar(e.Nombre), StringComparer.Ordinal)
                .ToList();

            if (estadios.Count == 0)
                throw new Exception("No hay estadios cargados.");

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
        
        private Grupo ObtenerGrupoPorEtiqueta(string etiqueta)
        {
            return _grupoRepositorio.ObtenerPorEtiqueta(etiqueta);
        }
    }
}