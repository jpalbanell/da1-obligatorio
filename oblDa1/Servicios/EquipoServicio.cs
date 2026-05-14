using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class EquipoServicio : IEquipoServicio
    {
        private readonly IEquipoRepositorio _equipoRepositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public EquipoServicio(IEquipoRepositorio equipoRepositorio, 
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarNombreUnico(equipo.Nombre);
            ValidarCupoConfederacion(equipo.Confederacion);
            _equipoRepositorio.Agregar(equipo);
            _auditoriaServicio.Registrar($"Alta de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void EditarEquipo(Equipo equipo, string nombreOriginal)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarNombreUnicoEnEdicion(equipo, nombreOriginal);
            ValidarCupoConfederacionEnEdicion(equipo, nombreOriginal);
            _equipoRepositorio.Actualizar(equipo, nombreOriginal);
            _auditoriaServicio.Registrar($"Edición de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private void ValidarCupoConfederacionEnEdicion(Equipo equipo, string nombreOriginal)
        {
            var equipoOriginal = _equipoRepositorio.ObtenerPorNombre(nombreOriginal);
            if (equipoOriginal == null) return;
            if (equipoOriginal.Confederacion == equipo.Confederacion) return;

            int cupo = ObtenerCupoConfederacion(equipo.Confederacion);
            int cantActual = _equipoRepositorio.ObtenerTodos()
                .Count(e => e.Confederacion == equipo.Confederacion);
            if (cantActual >= cupo)
                throw new InvalidOperationException("Cupo de confederación completo");
        }

        public void EliminarEquipo(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarEquipoExistente(nombre);
            _equipoRepositorio.Eliminar(nombre);
            _auditoriaServicio.Registrar($"Eliminación de equipo: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        
        public List<Equipo> ObtenerTodos()
        {
            return _equipoRepositorio.ObtenerTodos();
        }
        
        public Equipo ObtenerPorNombre(string nombre)
        {
            return _equipoRepositorio.ObtenerPorNombre(nombre);
        }

        private int ObtenerCupoConfederacion(Confederacion confederacion)
        {
            return confederacion switch
            {
                Confederacion.UEFA => 16,
                Confederacion.CONMEBOL => 7,
                Confederacion.CONCACAF => 7,
                Confederacion.CAF => 9,
                Confederacion.AFC => 8,
                Confederacion.OFC => 1,
                _ => throw new ArgumentException("Confederación inválida")
            };
        }
        
        private void ValidarNombreUnico(string nombre)
        {
            if (_equipoRepositorio.ObtenerPorNombre(nombre) != null)
                throw new InvalidOperationException("Ya existe un equipo con ese nombre");
        }

        private void ValidarNombreUnicoEnEdicion(Equipo equipo, string nombreOriginal)
        {
            if (equipo.Nombre.Equals(nombreOriginal, StringComparison.OrdinalIgnoreCase)) return;
            if (_equipoRepositorio.ObtenerPorNombre(equipo.Nombre) != null)
                throw new InvalidOperationException("Ya existe un equipo con ese nombre");
        }

        public void ValidarCupoConfederacion(Confederacion confederacion)
        {
            int cupo = ObtenerCupoConfederacion(confederacion);
            int cantActual = _equipoRepositorio.ObtenerTodos()
                .Count(e => e.Confederacion == confederacion);
            if (cantActual >= cupo)
                throw new InvalidOperationException("Cupo de confederación completo");
        }
        
        private void ValidarEquipoExistente(string nombre)
        {
            if (_equipoRepositorio.ObtenerPorNombre(nombre) == null)
                throw new KeyNotFoundException("No existe un equipo con ese nombre");
        }
        
        public void CompletarEquiposAutomaticamente(int semillaCompletar)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            var random = new Random(semillaCompletar);
            var resumen = new System.Text.StringBuilder();
            resumen.Append($"Generación automática de equipos con semilla: {semillaCompletar}. ");

            foreach (Confederacion confederacion in Enum.GetValues(typeof(Confederacion)))
            {
                int cupo = ObtenerCupoConfederacion(confederacion);
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
                    _equipoRepositorio.Agregar(equipo);
                    contador++;
                    cantGenerada++;
                }

                resumen.Append($"{confederacion}: {cantGenerada} equipos generados. ");
            }

            resumen.Append("Rangos: 300-2500.");
            _auditoriaServicio.Registrar(resumen.ToString(), _sesionServicio.ObtenerUsuarioActual());
        }
    }
}