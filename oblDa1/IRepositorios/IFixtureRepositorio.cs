using Dominio.Entidades;

namespace IRepositorios
{
    public interface IFixtureRepositorio
    {
        void Guardar(Fixture fixture);
        Fixture Obtener();
    }
}