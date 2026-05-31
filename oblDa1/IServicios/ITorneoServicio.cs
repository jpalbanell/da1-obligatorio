using Dominio.Entidades;

namespace IServicios
{
    public interface ITorneoServicio
    {
        void AgregarEquipo(Equipo equipo);
        List<Equipo> ObtenerTodos();
        void EditarEquipo(Equipo equipo, string nombreOriginal);
        Equipo ObtenerPorNombre(string nombre);
        
        void EliminarEquipo(string nombre);
        void CompletarEquiposAutomaticamente(int semillaCompletar);
        
        void AgregarEstadio(Estadio estadio);
        void ModificarEstadio(Estadio estadio, string nombreOriginal);
        void EliminarEstadio(string nombre);
        Estadio ObtenerEstadio(string nombre);
        List<Estadio> ObtenerTodosEstadios();
    }
}