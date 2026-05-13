using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class EstadioRepositorio : IEstadioRepositorio
    {
        private List<Estadio> _estadios = new List<Estadio>();

        public void Agregar(Estadio estadio)
        {
            _estadios.Add(estadio);
        }

        public List<Estadio> ObtenerTodos()
        {
            return _estadios;
        }

        public Estadio ObtenerPorNombre(string nombre)
        {
            return _estadios.FirstOrDefault(e => e.Nombre == nombre);
        }

        public void Actualizar(Estadio estadio, string nombreOriginal)
        {
            var indice = _estadios.FindIndex(e => e.Nombre == nombreOriginal);
            ValidarEstadioExistente(indice);
            _estadios[indice] = estadio;
        }

        public void Eliminar(string nombre)
        {
            var estadio = ObtenerPorNombre(nombre);
            ValidarEstadioNoNulo(estadio);
            _estadios.Remove(estadio);
        }

        private void ValidarEstadioExistente(int indice)
        {
            if (indice == -1)
                throw new KeyNotFoundException("Estadio no encontrado");
        }

        private void ValidarEstadioNoNulo(Estadio estadio)
        {
            if (estadio == null)
                throw new KeyNotFoundException("Estadio no encontrado");
        }
    }
}