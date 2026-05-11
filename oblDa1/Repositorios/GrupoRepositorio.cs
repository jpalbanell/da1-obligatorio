using Dominio.Entidades;
using IRepositorios;

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
        
        public Grupo ObtenerPorEtiqueta(string etiqueta)
        {
            return _grupos.FirstOrDefault(g => g.Etiqueta == etiqueta);
        }
    }
}