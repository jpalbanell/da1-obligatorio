using Dominio.Entidades;

namespace Servicios
{
    public interface IAuditoriaServicio
    {
        void Registrar(string accion, Usuario usuario);
    }
}