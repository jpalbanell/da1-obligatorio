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
    
            partido.Vencedor = partido.EquipoLocal;
    
            _partidoRepositorio.Actualizar(partido);
        }
    }
}