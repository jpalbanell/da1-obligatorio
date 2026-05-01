using Dominio.Entidades;

namespace Servicios
{
    public interface IFixtureServicio
    {
        void GenerarFixture(Fixture fixture, Usuario usuario);
    }
}