using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class EstadioServicio : IEstadioServicio
    {
        private readonly IEstadioRepositorio _repositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;

        public EstadioServicio(IEstadioRepositorio repositorio, 
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _repositorio = repositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void AgregarEstadio(Estadio estadio)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarNombreUnico(estadio.Nombre);
            _repositorio.Agregar(estadio);
            _auditoriaServicio.Registrar($"Alta de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public Estadio ObtenerEstadio(string nombre)
        {
            return _repositorio.ObtenerPorNombre(nombre);
        }

        public List<Estadio> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }
        
        private void ValidarEstadioExiste(string nombre)
        {
            if (_repositorio.ObtenerPorNombre(nombre) == null)
                throw new Exception("Estadio no encontrado.");
        }

        public void ModificarEstadio(Estadio estadio)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            ValidarNombreUnicoEnEdicion(estadio);
            ValidarEstadioExiste(estadio.Nombre);
            _repositorio.Actualizar(estadio);
            _auditoriaServicio.Registrar($"Edición de estadio: {estadio.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private void ValidarNombreUnicoEnEdicion(Estadio estadio)
        {
            var existente = _repositorio.ObtenerPorNombre(estadio.Nombre);
            if (existente != null && existente != estadio)
                throw new Exception("Ya existe un estadio con ese nombre.");
        }

        public void EliminarEstadio(string nombre)
        {
            _sesionServicio.ValidarRol(Rol.Administrador);
            _repositorio.Eliminar(nombre);
            _auditoriaServicio.Registrar($"Eliminación de estadio: {nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        private void ValidarNombreUnico(string nombre)
        {
            var existente = _repositorio.ObtenerPorNombre(nombre);
            if (existente != null)
                throw new Exception("Ya existe un estadio con ese nombre.");
        }
        
        private void ValidarEstadioExistente(string nombre)
        {
            if (_repositorio.ObtenerPorNombre(nombre) == null)
                throw new Exception("No existe un estadio con ese nombre");
        }
    }
}