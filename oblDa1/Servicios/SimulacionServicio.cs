using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class SimulacionServicio : ISimulacionServicio
    {
        private IPartidoRepositorio _partidoRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private ISesionServicio _sesionServicio;
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
            var partido = _partidoRepositorio.ObtenerPorId(partidoId);
            ValidarPartidoExistente(partido);

            var random = new Random(semillaSimulation);

            partido.GolesLocal = GenerarGoles(partido.EquipoLocal.RankingFifa, random);
            partido.GolesVisitante = GenerarGoles(partido.EquipoVisitante.RankingFifa, random);

            AsignarVencedor(partido);

            _partidoRepositorio.Actualizar(partido);
            _auditoriaServicio.Registrar($"Simulación de partido: {partidoId}", _sesionServicio.ObtenerUsuarioActual());

        }

        public void SimularFase(FaseTorneo fase, int semillaSimulation)
        {
            var partidos = _partidoRepositorio.ObtenerTodos()
                .Where(p => p.Fase == fase)
                .ToList();

            foreach (var partido in partidos)
                SimularPartido(partido.Id, semillaSimulation);
            
            _auditoriaServicio.Registrar($"Simulación de fase: {fase}", _sesionServicio.ObtenerUsuarioActual());

        }

        private int GenerarGoles(int rankingFifa, Random random)
        {
            double fuerza = rankingFifa / RankingMaximo;
            int maxGoles = Math.Max(MinGolesMaximos, (int)(fuerza * MaxGolesBase));
            return random.Next(0, maxGoles + 1);
        }

        private void AsignarVencedor(Partido partido)
        {
            if (partido.GolesLocal > partido.GolesVisitante)
                partido.Vencedor = partido.EquipoLocal;
            else if (partido.GolesVisitante > partido.GolesLocal)
                partido.Vencedor = partido.EquipoVisitante;
        }

        private void ValidarPartidoExistente(Partido partido)
        {
            if (partido == null)
                throw new Exception("Partido no encontrado");
        }
    }
}