using Dominio.Entidades;

namespace Servicios
{
    public interface IUsuarioServicio
    {
        void AgregarUsuario(Usuario usuario);
        Usuario ObtenerUsuario(int id);
        List<Usuario> ObtenerTodos();
        void ModificarUsuario(Usuario usuario);
        void EliminarUsuario(int id);
        Usuario Login(string email, string contrasena);
    }
}