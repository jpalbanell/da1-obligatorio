using Dominio.Entidades;

namespace IServicios
{
    public interface IEquipoServicio
    {
        void AgregarEquipo(Equipo equipo);
        void EditarEquipo(Equipo equipo, string nombreOriginal);
        void EliminarEquipo(string nombre);
        List<Equipo> ObtenerTodos();
        Equipo ObtenerPorNombre(string nombre);
        void CompletarEquiposAutomaticamente(int semillaCompletar);

    }
}