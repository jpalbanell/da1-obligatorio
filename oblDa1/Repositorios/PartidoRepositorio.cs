using Dominio.Entidades;

namespace Repositorios
{
    public class PartidoRepositorio : IPartidoRepositorio
    {
        private List<Partido> _partidos = new List<Partido>();

        public void Agregar(Partido partido)
        {
            _partidos.Add(partido);
        }

        public Partido ObtenerPorId(int id)
        {
            return _partidos.FirstOrDefault(p => p.Id == id);
        }
    }
}