using Dominio.Entidades;

namespace Repositorios
{
    public class GrupoRepositorio : IGrupoRepositorio
    {
        private List<Grupo> _grupos = new List<Grupo>();

        public void Agregar(Grupo grupo)
        {
            _grupos.Add(grupo);
        }

        public List<Grupo> ObtenerTodos()
        {
            return _grupos;
        }
    }
}