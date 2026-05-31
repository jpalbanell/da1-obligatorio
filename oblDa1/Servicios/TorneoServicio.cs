using Dominio.Entidades;
using Dominio;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class TorneoServicio : ITorneoServicio
    {
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IEstadioRepositorio _estadioRepositorio;
        private readonly IPartidoRepositorio _partidoRepositorio;
        private readonly IGrupoRepositorio _grupoRepositorio;
        private readonly IFixtureRepositorio _fixtureRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public TorneoServicio(
            IEquipoRepositorio equipoRepositorio,
            IEstadioRepositorio estadioRepositorio,
            IPartidoRepositorio partidoRepositorio,
            IGrupoRepositorio grupoRepositorio,
            IFixtureRepositorio fixtureRepositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _estadioRepositorio = estadioRepositorio;
            _partidoRepositorio = partidoRepositorio;
            _grupoRepositorio = grupoRepositorio;
            _fixtureRepositorio = fixtureRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = _fixtureRepositorio.Obtener() ?? new Fixture();
            fixture.AgregarEquipo(equipo);
            _equipoRepositorio.Agregar(equipo);
            _auditoriaServicio.Registrar($"Alta de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }
        public List<Equipo> ObtenerTodos()
        {
            return _equipoRepositorio.ObtenerTodos();
        }
    }
}