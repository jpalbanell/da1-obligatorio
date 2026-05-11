using Dominio.Entidades;
using IRepositorios;

namespace Repositorios
{
    public class FixtureRepositorio : IFixtureRepositorio
    {
        private Fixture _fixture;

        public void Guardar(Fixture fixture)
        {
            _fixture = fixture;
        }

        public Fixture Obtener()
        {
            return _fixture;
        }
    }
}