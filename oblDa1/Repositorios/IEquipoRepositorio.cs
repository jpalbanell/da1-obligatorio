using Dominio.Entidades;

namespace Repositorios
{
    public interface IEquipoRepositorio
    {
        void Add(Equipo equipo);
        List<Equipo> GetAll();
        Equipo GetByNombre(string nombre);
        void Update(Equipo equipo);
        void Delete(string nombre);

    }
}