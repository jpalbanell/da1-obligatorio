using Dominio.Entidades;

namespace Servicios
{
    public interface IPartidoServicio
    {
        void AgregarPartido(Partido partido);
        List<Partido> ObtenerTodos();
        Partido ObtenerPartido(int id);
    }
    
}