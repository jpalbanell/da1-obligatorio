using Dominio.Entidades;

namespace IServicios
{
    public interface IUsuarioServicio
    {
        void AgregarUsuario(Usuario usuario);
        Usuario ObtenerUsuario(int id);
        List<Usuario> ObtenerTodos();
        void ModificarUsuario(Usuario usuario);
        void EliminarUsuario(int id);
    }
}