using Dominio.Entidades;

namespace Repositorios
{
    public interface IGrupoRepositorio
    {
        void Agregar(Grupo grupo);
        List<Grupo> ObtenerTodos();
    }
}