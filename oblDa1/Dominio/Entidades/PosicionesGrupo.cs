namespace Dominio.Entidades
{
    public class PosicionesGrupo
    {
        private int _puntos;
        private int _golesFavor;
        private int _golesContra;
        private int _posicionFinal;
        private Equipo _equipo;
        private Grupo _grupo;

        public int Id { get; set; }
        public int DiferenciaGoles { get; set; }

        public Equipo Equipo
        {
            get => _equipo;
            set
            {
                ValidarNoNulo(value, "Equipo");
                _equipo = value;
            }
        }

        public Grupo Grupo
        {
            get => _grupo;
            set
            {
                ValidarNoNulo(value, "Grupo");
                _grupo = value;
            }
        }

        public int Puntos
        {
            get => _puntos;
            set
            {
                ValidarNoNegativo(value, "Puntos");
                _puntos = value;
            }
        }

        public int GolesFavor
        {
            get => _golesFavor;
            set
            {
                ValidarNoNegativo(value, "GolesFavor");
                _golesFavor = value;
            }
        }

        public int GolesContra
        {
            get => _golesContra;
            set
            {
                ValidarNoNegativo(value, "GolesContra");
                _golesContra = value;
            }
        }

        public int PosicionFinal
        {
            get => _posicionFinal;
            set
            {
                ValidarPosicionFinal(value);
                _posicionFinal = value;
            }
        }
        
        public PosicionesGrupo() { }
        public PosicionesGrupo(int id)
        {
            Id = id;
        }

        private void ValidarNoNulo(object valor, string campo)
        {
            if (valor == null)
                throw new ArgumentException($"{campo} no puede ser nulo");
        }

        private void ValidarNoNegativo(int valor, string campo)
        {
            if (valor < 0)
                throw new ArgumentException($"{campo} no puede ser negativo");
        }

        private void ValidarPosicionFinal(int valor)
        {
            if (valor < 1 || valor > 4)
                throw new ArgumentException("PosicionFinal debe estar entre 1 y 4");
        }
        
        public static int CalcularPuntos(int golesFavor, int golesContra)
        {
            if (golesFavor > golesContra) return 3;
            if (golesFavor == golesContra) return 1;
            return 0;
        }
        
        public void AplicarResultado(int golesFavor, int golesContra)
        {
            GolesFavor += golesFavor;
            GolesContra += golesContra;
            DiferenciaGoles = GolesFavor - GolesContra;
            Puntos += CalcularPuntos(golesFavor, golesContra);
        }
        
    }
}