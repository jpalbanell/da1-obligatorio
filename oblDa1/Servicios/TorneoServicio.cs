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
            var fixture = ObtenerOCrearFixture();
            fixture.AgregarEquipo(equipo);
            _equipoRepositorio.Agregar(equipo);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Alta de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private Fixture ObtenerOCrearFixture()
        {
            return _fixtureRepositorio.Obtener() ?? new Fixture();
        }
        
        public List<Equipo> ObtenerTodos()
        {
            return _equipoRepositorio.ObtenerTodos();
        }
        
        public void EditarEquipo(Equipo equipo, string nombreOriginal)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EditarEquipo(equipo, nombreOriginal);
            _equipoRepositorio.Actualizar(equipo, nombreOriginal);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Edición de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Equipo ObtenerPorNombre(string nombre)
        {
            return _equipoRepositorio.ObtenerPorNombre(nombre);
        }
    }
}