using Dominio.Entidades;

namespace Repositorios
{
    public interface IEstadioRepositorio
    {
        void Agregar(Estadio estadio);
        List<Estadio> ObtenerTodos();
        Estadio ObtenerPorNombre(string nombre);
        void Actualizar(Estadio estadio);
        void Eliminar(string nombre);
    }
}