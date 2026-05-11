using Dominio.Entidades;

namespace IRepositorios
{
    public interface IEstadioRepositorio
    {
        void Agregar(Estadio estadio);
        List<Estadio> ObtenerTodos();
        Estadio ObtenerPorNombre(string nombre);
        void Actualizar(Estadio estadio, string nombreOriginal);
        void Eliminar(string nombre);
    }
}