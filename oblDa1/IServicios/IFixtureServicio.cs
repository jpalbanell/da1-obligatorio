using Dominio.Entidades;

namespace IServicios
{
    public interface IFixtureServicio
    {
        void GenerarFixture(Fixture fixture);
        List<Grupo> ObtenerGrupos();
        Fixture ObtenerFixture();
    }
}