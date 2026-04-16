using Dominio.Entidades;

namespace Repositorios
{
    public class EquipoRepository : IEquipoRepository
    {
        private List<Equipo> _equipos = new List<Equipo>();

        public void Add(Equipo equipo)
        {
            _equipos.Add(equipo);
        }

        public List<Equipo> GetAll()
        {
            return _equipos;
        }
        
        public Equipo GetByNombre(string nombre)
        {
            return _equipos.FirstOrDefault(e => e.Nombre == nombre);
        }
    }
}