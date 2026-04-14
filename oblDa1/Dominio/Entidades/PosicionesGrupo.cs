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
    }
}