namespace Dominio.Entidades
{
    public class PosicionesGrupo
    {
        public int Id { get; set; }

        public PosicionesGrupo(int id)
        {
            Id = id;
        }
        
        private Equipo _equipo;

        public Equipo Equipo
        {
            get => _equipo;
            set
            {
                if (value == null) throw new ArgumentException("Equipo no puede ser nulo");
                _equipo = value;
            }
        }
        
        private Grupo _grupo;

        public Grupo Grupo
        {
            get => _grupo;
            set
            {
                if (value == null) throw new ArgumentException("Grupo no puede ser nulo");
                _grupo = value;
            }
        }
        
        private int _puntos;

        public int Puntos
        {
            get => _puntos;
            set
            {
                if (value < 0) throw new ArgumentException("Puntos no puede ser negativo");
                _puntos = value;
            }
        }
        
        private int _golesFavor;

        public int GolesFavor
        {
            get => _golesFavor;
            set
            {
                if (value < 0) throw new ArgumentException("GolesFavor no puede ser negativo");
                _golesFavor = value;
            }
        }
        
        private int _golesContra;

        public int GolesContra
        {
            get => _golesContra;
            set
            {
                if (value < 0) throw new ArgumentException("GolesContra no puede ser negativo");
                _golesContra = value;
            }
        }
        
        public int DiferenciaGoles { get; set; }
        
        private int _posicionFinal;

        public int PosicionFinal
        {
            get => _posicionFinal;
            set
            {
                if (value < 1 || value > 4) throw new ArgumentException("PosicionFinal debe estar entre 1 y 4");
                _posicionFinal = value;
            }
        }
    }
}