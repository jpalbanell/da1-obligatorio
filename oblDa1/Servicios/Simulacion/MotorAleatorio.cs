using Dominio;

namespace Servicios.Simulacion
{
    public class MotorAleatorio : IMotorSimulacion
    {
        private const int MaxGoles = 5;

        public string Nombre => "Aleatorio Puro";

        public (int golesLocal, int golesVisitante) Simular(int rankingLocal, int rankingVisitante, Random random)
        {
            int golesLocal = random.Next(0, MaxGoles + 1);
            int golesVisitante = random.Next(0, MaxGoles + 1);
            return (golesLocal, golesVisitante);
        }
    }
}
