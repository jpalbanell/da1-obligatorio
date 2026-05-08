using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class SimulacionServicio : ISimulacionServicio
    {
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        private readonly IGrupoRepositorio _grupoRepositorio;
        private const double RankingMaximo = 2500.0;
        private const int MaxGolesBase = 5;
        private const int MinGolesMaximos = 1;

        public SimulacionServicio(IPartidoRepositorio partidoRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _partidoRepositorio = partidoRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void SimularPartido(int partidoId, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partido = _partidoRepositorio.ObtenerPorId(partidoId);
            ValidarPartidoExistente(partido);

            var random = new Random(semillaSimulation + partidoId);
            partido.GolesLocal = GenerarGoles(partido.EquipoLocal.RankingFifa, random);
            partido.GolesVisitante = GenerarGoles(partido.EquipoVisitante.RankingFifa, random);
            partido.TieneResultado = true;
            AsignarVencedor(partido, random);
            ActualizarPosiciones(partido);
            PropagrarResultado(partido);
            _partidoRepositorio.Actualizar(partido);
            _auditoriaServicio.Registrar(
                $"Simulación de partido: {partidoId} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        public void SimularFase(FaseTorneo fase, int semillaSimulation)
        {
            _sesionServicio.ValidarRol(Rol.Editor);
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == fase)
                .ToList();

            foreach (var partido in partidos)
                SimularPartidoDeFase(partido, semillaSimulation);

            _auditoriaServicio.Registrar(
                $"Simulación de fase: {fase} con SemillaSimulation: {semillaSimulation}",
                _sesionServicio.ObtenerUsuarioActual());
        }

        private void SimularPartidoDeFase(Partido partido, int semillaSimulation)
        {
            var random = new Random(semillaSimulation + partido.Id);
            partido.GolesLocal = GenerarGoles(partido.EquipoLocal.RankingFifa, random);
            partido.GolesVisitante = GenerarGoles(partido.EquipoVisitante.RankingFifa, random);
            partido.TieneResultado = true;
            AsignarVencedor(partido, random);
            ActualizarPosiciones(partido);
            PropagrarResultado(partido);
            _partidoRepositorio.Actualizar(partido);
        }

        private int GenerarGoles(int rankingFifa, Random random)
        {
            double fuerza = rankingFifa / RankingMaximo;
            int maxGoles = Math.Max(MinGolesMaximos, (int)(fuerza * MaxGolesBase));
            return random.Next(0, maxGoles + 1);
        }

        private void AsignarVencedor(Partido partido, Random random)
        {
            if (partido.GolesLocal > partido.GolesVisitante)
                partido.Vencedor = partido.EquipoLocal;
            else if (partido.GolesVisitante > partido.GolesLocal)
                partido.Vencedor = partido.EquipoVisitante;
            else if (partido.Fase != FaseTorneo.FaseGrupos)
                partido.Vencedor = random.Next(2) == 0
                    ? partido.EquipoLocal
                    : partido.EquipoVisitante;
        }

        private void ValidarPartidoExistente(Partido partido)
        {
            if (partido == null)
                throw new Exception("Partido no encontrado");
        }
        
        private void ActualizarPosiciones(Partido partido)
        {
            if (partido.Grupo == null) return;

            var posLocal = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoLocal.Nombre);

            var posVisitante = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoVisitante.Nombre);

            if (posLocal == null || posVisitante == null) return;

            posLocal.GolesFavor += partido.GolesLocal;
            posLocal.GolesContra += partido.GolesVisitante;
            posLocal.DiferenciaGoles = posLocal.GolesFavor - posLocal.GolesContra;

            posVisitante.GolesFavor += partido.GolesVisitante;
            posVisitante.GolesContra += partido.GolesLocal;
            posVisitante.DiferenciaGoles = posVisitante.GolesFavor - posVisitante.GolesContra;
            
            posLocal.Puntos += CalcularPuntos(partido.GolesLocal, partido.GolesVisitante);
            posVisitante.Puntos += CalcularPuntos(partido.GolesVisitante, partido.GolesLocal);
        }
        
        private int CalcularPuntos(int golesFavor, int golesContra)
        {
            if (golesFavor > golesContra) return 3;
            if (golesFavor == golesContra) return 1;
            return 0;
        }
        
        private void PropagrarResultado(Partido partido)
        {
            if (partido.Vencedor == null) return;
            var siguientes = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.OrigenLocal?.Id == partido.Id || p.OrigenVisitante?.Id == partido.Id)
                .ToList();
            foreach (var siguiente in siguientes)
            {
                var equipo = siguiente.EsPorPerdedor ? ObtenerPerdedor(partido) : partido.Vencedor;
                if (siguiente.OrigenLocal?.Id == partido.Id)
                    siguiente.EquipoLocal = equipo;
                else
                    siguiente.EquipoVisitante = equipo;
                _partidoRepositorio.Actualizar(siguiente);
            }
        }
        
        private Equipo ObtenerPerdedor(Partido partido)
        {
            return partido.Vencedor == partido.EquipoLocal
                ? partido.EquipoVisitante
                : partido.EquipoLocal;
        }
    }
}