using Dominio.Entidades;

namespace IRepositorios
{
    public interface IGrupoRepositorio
    {
        void Agregar(Grupo grupo);
        List<Grupo> ObtenerTodos();
        Grupo ObtenerPorEtiqueta(string etiqueta);
    }
}