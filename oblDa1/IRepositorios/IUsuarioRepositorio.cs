using Dominio.Entidades;

namespace IRepositorios
{
    public interface IUsuarioRepositorio
    {
        void Agregar(Usuario usuario);
        List<Usuario> ObtenerTodos();
        Usuario ObtenerPorId(int id);
        List<Usuario> ObtenerPorRol(Rol rol);
        void Actualizar(Usuario usuario);
        void Eliminar(int id);
    }
}