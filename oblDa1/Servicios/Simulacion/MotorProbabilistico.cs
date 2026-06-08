using IServicios;

namespace Servicios.Simulacion
{
    public class MotorProbabilistico : IMotorSimulacion
    {
        private const double RankingMaximo = 2500.0;
        private const int MaxGolesBase = 5;
        private const int MinGolesMaximos = 1;

        public string Nombre => "Probabilístico";

        public (int golesLocal, int golesVisitante) Simular(int rankingLocal, int rankingVisitante, Random random)
        {
            int golesLocal = GenerarGoles(rankingLocal, random);
            int golesVisitante = GenerarGoles(rankingVisitante, random);
            return (golesLocal, golesVisitante);
        }

        private int GenerarGoles(int ranking, Random random)
        {
            double fuerza = ranking / RankingMaximo;
            int maxGoles = Math.Max(MinGolesMaximos, (int)(fuerza * MaxGolesBase));
            return random.Next(0, maxGoles + 1);
        }
    }
}
