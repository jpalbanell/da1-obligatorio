using Dominio.Entidades;

namespace Servicios
{
    public interface ISimulacionServicio
    {
        void SimularPartido(int partidoId, int semillaSimulation);
        void SimularFase(FaseTorneo fase, int semillaSimulation);
    }
}