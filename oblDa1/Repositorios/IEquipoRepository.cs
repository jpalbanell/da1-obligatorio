using Dominio.Entidades;

namespace Repositorios
{
    public interface IEquipoRepository
    {
        void Add(Equipo equipo);
        List<Equipo> GetAll();
    }
}