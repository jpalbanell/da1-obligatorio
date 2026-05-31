using Dominio.Entidades;

namespace IServicios
{
    public interface ITorneoServicio
    {
        void AgregarEquipo(Equipo equipo);
        List<Equipo> ObtenerTodos();
        void EditarEquipo(Equipo equipo, string nombreOriginal);
        Equipo ObtenerPorNombre(string nombre);
    }
}