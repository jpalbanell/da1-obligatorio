using Dominio.Entidades;

namespace Servicios
{
    public interface IEstadioServicio
    {
        void AgregarEstadio(Estadio estadio);
        Estadio ObtenerEstadio(string nombre);
        List<Estadio> ObtenerTodos();
        void ModificarEstadio(Estadio estadio);
        void EliminarEstadio(string nombre);
    }
}