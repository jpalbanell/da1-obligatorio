using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class EstadioServicio : IEstadioServicio
    {
        private readonly IEstadioRepositorio _repositorio;

        public EstadioServicio(IEstadioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public void AgregarEstadio(Estadio estadio)
        {
            ValidarNombreUnico(estadio.Nombre);
            _repositorio.Agregar(estadio);
        }

        public Estadio ObtenerEstadio(string nombre)
        {
            return _repositorio.ObtenerPorNombre(nombre);
        }

        public List<Estadio> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void ModificarEstadio(Estadio estadio)
        {
            _repositorio.Actualizar(estadio);
        }

        public void EliminarEstadio(string nombre)
        {
            _repositorio.Eliminar(nombre);
        }

        private void ValidarNombreUnico(string nombre)
        {
            var existente = _repositorio.ObtenerPorNombre(nombre);
            if (existente != null)
                throw new Exception("Ya existe un estadio con ese nombre.");
        }
    }
}