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
        
        public void EliminarEquipo(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEquipo(nombre);
            _equipoRepositorio.Eliminar(nombre);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Eliminación de equipo: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void CompletarEquiposAutomaticamente(int semillaCompletar)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var random = new Random(semillaCompletar);
            var resumen = new System.Text.StringBuilder();
            resumen.Append($"Generación automática de equipos con semilla: {semillaCompletar}. ");

            foreach (Confederacion confederacion in Enum.GetValues(typeof(Confederacion)))
            {
                int cupo = confederacion.CupoMaximo();
                int cantActual = _equipoRepositorio.ObtenerTodos()
                    .Count(e => e.Confederacion == confederacion);
                int cantGenerada = 0;
                int contador = cantActual + 1;

                for (int i = cantActual + 1; i <= cupo; i++)
                {
                    var equipo = new Equipo();
                    equipo.Nombre = $"{confederacion}_{contador:D2}";
                    equipo.Confederacion = confederacion;
                    equipo.RankingFifa = random.Next(300, 2501);
                    var fixture = ObtenerOCrearFixture();
                    fixture.AgregarEquipo(equipo);
                    _equipoRepositorio.Agregar(equipo);
                    _fixtureRepositorio.Guardar(fixture);
                    contador++;
                    cantGenerada++;
                }

                resumen.Append($"{confederacion}: {cantGenerada} equipos generados. ");
            }

            resumen.Append("Rangos: 300-2500.");
            _auditoriaServicio.Registrar(resumen.ToString(), _sesionServicio.ObtenerUsuarioActual());
        }
        
        public void AgregarEstadio(Estadio estadio)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.AgregarEstadio(estadio);
            _estadioRepositorio.Agregar(estadio);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Alta de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void ModificarEstadio(Estadio estadio, string nombreOriginal)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEstadio(nombreOriginal);
            fixture.AgregarEstadio(estadio);
            _estadioRepositorio.Actualizar(estadio, nombreOriginal);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Edición de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void EliminarEstadio(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var fixture = ObtenerOCrearFixture();
            fixture.EliminarEstadio(nombre);
            _estadioRepositorio.Eliminar(nombre);
            _fixtureRepositorio.Guardar(fixture);
            _auditoriaServicio.Registrar($"Eliminación de estadio: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Estadio ObtenerEstadio(string nombre)
        {
            return _estadioRepositorio.ObtenerPorNombre(nombre);
        }

        public List<Estadio> ObtenerTodosEstadios()
        {
            return _estadioRepositorio.ObtenerTodos();
        }
    }
}