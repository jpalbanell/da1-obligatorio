using Dominio;

namespace Servicios.Simulacion
{
    public class MotorAleatorio : IMotorSimulacion
    {
        public string Nombre => "Aleatorio Puro";

        public (int golesLocal, int golesVisitante) Simular(int rankingLocal, int rankingVisitante, Random random)
        {
            return (0, 0);
        }
    }
}
