using Dominio.Entidades;

namespace Repositorios
{
    public interface IUsuarioRepositorio
    {
        void Agregar(Usuario usuario);
        List<Usuario> ObtenerTodos();
        Usuario ObtenerPorId(int id);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);
    }
}