using Dominio.Entidades;

namespace Servicios
{
    public interface IAutenticacionServicio
    {
        Usuario Login(string email, string contrasena);
    }
}