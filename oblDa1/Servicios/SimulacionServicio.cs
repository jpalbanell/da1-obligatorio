using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class SimulacionServicio : ISimulacionServicio
    {
        private IPartidoRepositorio _partidoRepositorio;

        public SimulacionServicio(IPartidoRepositorio partidoRepositorio)
        {
            _partidoRepositorio = partidoRepositorio;
        }

        public void SimularPartido(int partidoId, int semillaSimulation)
        {
            var partido = _partidoRepositorio.ObtenerPorId(partidoId);
            var random = new Random(semillaSimulation);

            partido.GolesLocal = random.Next(0, 4);
            partido.GolesVisitante = random.Next(0, 4);

            if (partido.EquipoVisitante.RankingFifa > partido.EquipoLocal.RankingFifa)
                partido.Vencedor = partido.EquipoVisitante;
            else
                partido.Vencedor = partido.EquipoLocal;

            _partidoRepositorio.Actualizar(partido);
        }
    }
}