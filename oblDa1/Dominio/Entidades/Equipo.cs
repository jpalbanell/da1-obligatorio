namespace Dominio.Entidades
{
    public class Equipo
    {
        public const int RankingMinimo = 300;
        public const int RankingMaximo = 2500;

        private string _nombre;
        public Confederacion Confederacion { get; set; }
        private int _rankingFifa;
        public string? Bandera { get; set; }

        public string Nombre
        {
            get => _nombre;
            set
            {
                ValidarNombre(value);
                _nombre = value;
            }
        }
        
        public int RankingFifa
        {
            get => _rankingFifa;
            set
            {
                ValidarRankingFifa(value);
                _rankingFifa = value;
            }
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");
            if (nombre.Length > 60)
                throw new ArgumentException("El nombre no puede superar los 60 caracteres.");
        } 
        
        private void ValidarRankingFifa(int ranking)
        {
            if (ranking < RankingMinimo || ranking > RankingMaximo)
                throw new ArgumentException($"El ranking FIFA debe estar entre {RankingMinimo} y {RankingMaximo}.");
        }

        public int CalcularNuevoRanking(int rankingOponente, double resultado, double multiplicadorFase)
        {
            const double FactorDeDesarrollo = 30;

            double probabilidad = 1.0 / (1.0 + Math.Pow(10, (rankingOponente - RankingFifa) / 1000.0));
            double rankingNuevo = RankingFifa + (FactorDeDesarrollo * (resultado - probabilidad)) * multiplicadorFase;
            int redondeado = (int)Math.Round(rankingNuevo, MidpointRounding.AwayFromZero);

            return Math.Clamp(redondeado, RankingMinimo, RankingMaximo);
        }
    }
}