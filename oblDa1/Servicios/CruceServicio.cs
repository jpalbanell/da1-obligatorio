using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class CruceServicio : ICruceServicio
    {
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public CruceServicio(
            IGrupoRepositorio grupoRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _grupoRepositorio = grupoRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void GenerarCruces(int semillaCrucesFase, Usuario usuario)
        {
            var fixture = _fixtureRepositorio.Obtener();
            ValidarFixtureGenerado(fixture);
            ValidarCrucesNoGenerados(fixture);
            ValidarTodosLosPartidosTienenResultado();

            var emparejamientos = GenerarEmparejamientos(semillaCrucesFase);
            GenerarPartidosEliminatorios(emparejamientos);
            BloquearPartidosDeFaseGrupos();

            fixture.CrucesGenerados = true;
            _fixtureRepositorio.Guardar(fixture);

            _auditoriaServicio.Registrar("Generación de cruces para segunda fase", usuario);
        }

        private void BloquearPartidosDeFaseGrupos()
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            foreach (var grupo in grupos)
            {
                foreach (var partido in grupo.ListaPartidos)
                {
                    partido.EstaBloqueado = true;
                }
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
            var grupos = _grupoRepositorio.ObtenerTodos();
            foreach (var grupo in grupos)
            {
                foreach (var partido in grupo.ListaPartidos)
                {
                    if (!partido.TieneResultado)
                        throw new Exception($"El partido {partido.Codigo} del grupo {grupo.Etiqueta} no tiene resultado cargado.");
                }
            }
        }
        
        public List<PosicionesGrupo> CalcularPosicionesGrupo(Grupo grupo)
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
            posicion.Puntos += CalcularPuntos(golesFavor, golesContra);
        }

        private int CalcularPuntos(int golesFavor, int golesContra)
        {
            if (golesFavor > golesContra) return 3;
            if (golesFavor == golesContra) return 1;
            return 0;
        }
        
        public List<PosicionesGrupo> ObtenerClasificados()
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var todasLasPosiciones = new List<PosicionesGrupo>();

            foreach (var grupo in grupos)
            {
                var posicionesGrupo = CalcularPosicionesGrupo(grupo);
                var ordenadas = OrdenarPosiciones(posicionesGrupo);
                todasLasPosiciones.AddRange(ordenadas);
            }

            return todasLasPosiciones;
        }

        private List<PosicionesGrupo> OrdenarPosiciones(List<PosicionesGrupo> posiciones)
        {
            return posiciones
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .ToList();
        }
        public (List<PosicionesGrupo> primeros, List<PosicionesGrupo> segundos, List<PosicionesGrupo> mejoresTerceros) SeleccionarClasificados()
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            var primeros = new List<PosicionesGrupo>();
            var segundos = new List<PosicionesGrupo>();
            var terceros = new List<PosicionesGrupo>();

            foreach (var grupo in grupos)
            {
                var posicionesOrdenadas = OrdenarPosiciones(CalcularPosicionesGrupo(grupo));
                primeros.Add(posicionesOrdenadas[0]);
                segundos.Add(posicionesOrdenadas[1]);
                terceros.Add(posicionesOrdenadas[2]);
            }

            var mejoresTerceros = terceros
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .Take(8)
                .ToList();

            return (primeros, segundos, mejoresTerceros);
        }
        
        public List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> GenerarEmparejamientos(int semilla)
        {
            var (primeros, segundos, mejoresTerceros) = SeleccionarClasificados();

            var primerosOrdenados = primeros
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .ToList();

            var ochoMejoresPrimeros = primerosOrdenados.Take(8).ToList();
            var cuatroRestantesPrimeros = primerosOrdenados.Skip(8).ToList();

            var segundosOrdenados = segundos
                .OrderByDescending(p => p.Puntos)
                .ThenByDescending(p => p.DiferenciaGoles)
                .ThenByDescending(p => p.GolesFavor)
                .ToList();

            var cuatroSegundosMenorPuntaje = segundosOrdenados.Skip(8).ToList();
            var ochoSegundosRestantes = segundosOrdenados.Take(8).ToList();

            var random = new Random(semilla);
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
        
        public void GenerarPartidosEliminatorios(
            List<(PosicionesGrupo local, PosicionesGrupo visitante, string codigo)> emparejamientos)
        {
            foreach (var (local, visitante, codigo) in emparejamientos)
            {
                var partido = new Partido(_proximoIdPartido++);
                partido.Codigo = codigo;
                partido.Fase = FaseTorneo.Dieciseisavos;
                partido.EquipoLocal = local.Equipo;
                partido.EquipoVisitante = visitante.Equipo;
                partido.Fecha = new DateTime(2026, 7, 1, 14, 0, 0);
                partido.Estadio = ObtenerPrimerEstadioDisponible();
                partido.Grupo = ObtenerGrupoPorEtiqueta(local.Grupo.Etiqueta);
                _partidoRepositorio.Agregar(partido);
            }
        }

        private int _proximoIdPartido = 1000;

        private Estadio ObtenerPrimerEstadioDisponible()
        {
            var grupos = _grupoRepositorio.ObtenerTodos();
            return grupos
                .SelectMany(g => g.ListaPartidos)
                .First()
                .Estadio;
        }

        private Grupo ObtenerGrupoPorEtiqueta(string etiqueta)
        {
            return _grupoRepositorio.ObtenerPorEtiqueta(etiqueta);
        }
    }
}