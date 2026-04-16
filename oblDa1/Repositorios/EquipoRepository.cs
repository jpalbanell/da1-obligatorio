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
        
        public void Update(Equipo equipo)
        {
            var index = _equipos.FindIndex(e => e.Nombre == equipo.Nombre);
            _equipos[index] = equipo;
        }
        
        public void Delete(string nombre)
        {
            var equipo = GetByNombre(nombre);
            _equipos.Remove(equipo);
        }
    }
}