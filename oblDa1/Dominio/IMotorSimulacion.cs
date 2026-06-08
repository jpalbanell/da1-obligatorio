namespace Dominio
{
    public interface IMotorSimulacion
    {
        string Nombre { get; }
        (int golesLocal, int golesVisitante) Simular(int rankingLocal, int rankingVisitante, Random random);
    }
}
