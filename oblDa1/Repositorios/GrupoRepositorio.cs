using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class GrupoRepositorio : IGrupoRepositorio
    {
        private readonly SqlContext _context;

        public GrupoRepositorio(SqlContext context)
        {
            _context = context;
        }

        public void Agregar(Grupo grupo)
        {
            _context.Grupos.Add(grupo);
            _context.SaveChanges();
        }

        public List<Grupo> ObtenerTodos()
        {
            return _context.Grupos.ToList();
        }

        public Grupo ObtenerPorEtiqueta(string etiqueta)
        {
            return _context.Grupos.FirstOrDefault(g => g.Etiqueta == etiqueta);
        }
    }
}
