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
        
        public List<Partido> ObtenerTodos()
        {
            return _partidos;
        }
        
        public void Actualizar(Partido partido)
        {
            var indice = _partidos.FindIndex(p => p.Id == partido.Id);
            ValidarPartidoExistente(indice);
            _partidos[indice] = partido;
        }

        private void ValidarPartidoExistente(int indice)
        {
            if (indice == -1)
                throw new Exception("Partido no encontrado");
        }
        
    }
}