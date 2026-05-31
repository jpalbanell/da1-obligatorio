using Dominio.Entidades;

namespace IServicios
{
    public interface ITorneoServicio
    {
        void AgregarEquipo(Equipo equipo);
        List<Equipo> ObtenerTodos();
    }
}