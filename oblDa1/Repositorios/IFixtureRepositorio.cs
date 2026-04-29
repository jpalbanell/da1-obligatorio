using Dominio.Entidades;

namespace Repositorios
{
    public interface IFixtureRepositorio
    {
        void Guardar(Fixture fixture);
        Fixture Obtener();
    }
}