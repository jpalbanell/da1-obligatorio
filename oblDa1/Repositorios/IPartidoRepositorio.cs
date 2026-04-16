using Dominio.Entidades;

namespace Repositorios
{
    public interface IPartidoRepositorio
    {
        void Agregar(Partido partido);
        Partido ObtenerPorId(int id);
        List<Partido> ObtenerTodos();
        void Actualizar(Partido partido);
    }
}