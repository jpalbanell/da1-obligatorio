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
    }
}