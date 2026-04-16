using Dominio.Entidades;

namespace Repositorios
{
    public interface IEstadioRepositorio
    {
        void Agregar(Estadio estadio);
        List<Estadio> ObtenerTodos();
        Estadio ObtenerPorId(int id);
        void Actualizar(Estadio estadio);
        void Eliminar(int id);
    }
}