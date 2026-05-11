using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class EquipoRepositorio : IEquipoRepositorio
    {
        private List<Equipo> _equipos = new List<Equipo>();

        public void Agregar(Equipo equipo)
        {
            _equipos.Add(equipo);
        }

        public List<Equipo> ObtenerTodos()
        {
            return _equipos;
        }
        
        public Equipo ObtenerPorNombre(string nombre)
        {
            return _equipos.FirstOrDefault(e => e.Nombre == nombre);
        }
        
        public void Actualizar(Equipo equipo, string nombreOriginal)
        {
            var index = _equipos.FindIndex(e => e.Nombre == nombreOriginal);
            ValidarEquipoExistente(index);
            _equipos[index] = equipo;
        }

        public void Eliminar(string nombre)
        {
            var equipo = ObtenerPorNombre(nombre);
            ValidarEquipoNoNulo(equipo);
            _equipos.Remove(equipo);
        }

        private void ValidarEquipoExistente(int index)
        {
            if (index == -1)
                throw new Exception("Equipo no encontrado");
        }

        private void ValidarEquipoNoNulo(Equipo equipo)
        {
            if (equipo == null)
                throw new Exception("Equipo no encontrado");
        }
    }
}