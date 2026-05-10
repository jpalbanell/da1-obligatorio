using Dominio.Entidades;

namespace Repositorios
{
    public interface IEquipoRepositorio
    {
        void Agregar(Equipo equipo);
        List<Equipo> ObtenerTodos();
        Equipo ObtenerPorNombre(string nombre);
        void Actualizar(Equipo equipo, string nombreOriginal);
        void Eliminar(string nombre);

    }
}