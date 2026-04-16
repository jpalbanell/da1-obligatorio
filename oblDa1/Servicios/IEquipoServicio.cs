using Dominio.Entidades;

namespace Servicios
{
    public interface IEquipoServicio
    {
        void AgregarEquipo(Equipo equipo);
        void EditarEquipo(Equipo equipo);
        void EliminarEquipo(string nombre);
        List<Equipo> ObtenerTodos();
        Equipo ObtenerPorNombre(string nombre);

    }
}