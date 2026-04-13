
namespace Dominio
{
    public class Partido
    {
        private string _codigo;
        private DateTime _fecha;
        private Equipo _equipoLocal;
        private Equipo _equipoVisitante;
        private Estadio _estadio;
        private Grupo _grupo;
        private int _golesLocal;
        private int _golesVisitante;
        private Equipo _vencedor;

        public int Id { get; set; }

        public string Codigo
        {
            get => _codigo;
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentException("Codigo no puede ser nulo o vacío");
                _codigo = value;
            }
        }

        public DateTime Fecha
        {
            get => _fecha;
            set
            {
                if (value == default) throw new ArgumentException("Fecha no puede ser vacía");
                _fecha = value;
            }
        }

        public FaseTorneo Fase { get; set; }

        public Equipo EquipoLocal
        {
            get => _equipoLocal;
            set
            {
                if (value == null) throw new ArgumentException("EquipoLocal no puede ser nulo");
                _equipoLocal = value;
            }
        }

        public Equipo EquipoVisitante
        {
            get => _equipoVisitante;
            set
            {
                if (value == null) throw new ArgumentException("EquipoVisitante no puede ser nulo");
                if (value == _equipoLocal) throw new ArgumentException("EquipoVisitante no puede ser igual al EquipoLocal");
                _equipoVisitante = value;
            }
        }

        public Estadio Estadio
        {
            get => _estadio;
            set
            {
                if (value == null) throw new ArgumentException("Estadio no puede ser nulo");
                _estadio = value;
            }
        }

        public Grupo Grupo
        {
            get => _grupo;
            set
            {
                if (value == null) throw new ArgumentException("Grupo no puede ser nulo");
                _grupo = value;
            }
        }

        public int GolesLocal
        {
            get => _golesLocal;
            set
            {
                if (value < 0) throw new ArgumentException("GolesLocal no puede ser negativo");
                _golesLocal = value;
            }
        }

        public int GolesVisitante
        {
            get => _golesVisitante;
            set
            {
                if (value < 0) throw new ArgumentException("GolesVisitante no puede ser negativo");
                _golesVisitante = value;
            }
        }

        public Equipo Vencedor
        {
            get => _vencedor;
            set
            {
                if (value != null && value != _equipoLocal && value != _equipoVisitante)
                    throw new ArgumentException("Vencedor debe ser EquipoLocal o EquipoVisitante");
                _vencedor = value;
            }
        }

        public Cruce Cruce { get; set; }

        public Partido(int id)
        {
            Id = id;
        }
    }
}