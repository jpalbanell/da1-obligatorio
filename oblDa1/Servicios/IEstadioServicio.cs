using Dominio.Entidades;

namespace Servicios
{
    public interface IEstadioServicio
    {
        void AgregarEstadio(Estadio estadio);
        Estadio ObtenerEstadio(int id);
        List<Estadio> ObtenerTodos();
        void ModificarEstadio(Estadio estadio);
        void EliminarEstadio(int id);
    }
}