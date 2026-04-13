namespace Dominio
{
    public class Partido
    {
        private string _codigo;
        private DateTime _fecha;

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

        public Partido(int id)
        {
            Id = id;
        }
        
        private Equipo _equipoLocal;

        public Equipo EquipoLocal
        {
            get => _equipoLocal;
            set
            {
                if (value == null) throw new ArgumentException("EquipoLocal no puede ser nulo");
                _equipoLocal = value;
            }
        }
    }
}