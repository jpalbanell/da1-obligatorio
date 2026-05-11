using Dominio.Entidades;

namespace IServicios
{
    public interface ISimulacionServicio
    {
        void SimularPartido(int partidoId, int semillaSimulation);
        void SimularFase(FaseTorneo fase, int semillaSimulation);
    }
}