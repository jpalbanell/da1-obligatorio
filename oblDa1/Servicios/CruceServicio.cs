using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class CruceServicio : ICruceServicio
    {
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public CruceServicio(
            IGrupoRepositorio grupoRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _grupoRepositorio = grupoRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void GenerarCruces(int semillaCrucesFase, Usuario usuario)
        {
            var fixture = _fixtureRepositorio.Obtener();
            ValidarFixtureGenerado(fixture);
        }

        private void ValidarFixtureGenerado(Fixture fixture)
        {
            if (fixture == null || !fixture.EstaGenerado)
                throw new Exception("No se puede generar cruces si el fixture no fue generado.");
        }
    }
}