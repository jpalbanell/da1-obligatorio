using Dominio.Entidades;

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

        public Estadio ObtenerPorId(int id)
        {
            return _estadios.FirstOrDefault(e => e.Id == id);
        }

        public void Actualizar(Estadio estadio)
        {
            var indice = _estadios.FindIndex(e => e.Id == estadio.Id);
            ValidarEstadioExistente(indice);
            _estadios[indice] = estadio;
        }

        private void ValidarEstadioExistente(int indice)
        {
            if (indice == -1)
                throw new Exception("Estadio no encontrado");
        }

        public void Eliminar(int id)
        {
            var estadio = ObtenerPorId(id);
            ValidarEstadioNoNulo(estadio);
            _estadios.Remove(estadio);
        }

        private void ValidarEstadioNoNulo(Estadio estadio)
        {
            if (estadio == null)
                throw new Exception("Estadio no encontrado");
        }
    }
}