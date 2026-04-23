using Dominio.Entidades;

namespace Servicios
{
    public interface IPartidoServicio
    {
        void AgregarPartido(Partido partido);
        List<Partido> ObtenerTodos();
        Partido ObtenerPartido(int id);
        void ModificarPartido(Partido partido);
        List<Partido> ObtenerPorFecha(DateTime fecha);
        List<Partido> ObtenerPorEstadio(string nombreEstadio);
        List<Partido> ObtenerPorGrupo(string etiquetaGrupo);
    }
    
}