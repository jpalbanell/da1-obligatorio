using Dominio.Entidades;

namespace IServicios
{
    public interface IEstadioServicio
    {
        void AgregarEstadio(Estadio estadio);
        Estadio ObtenerEstadio(string nombre);
        List<Estadio> ObtenerTodos();
        void ModificarEstadio(Estadio estadio, string nombreOriginal);
        void EliminarEstadio(string nombre);
    }
}