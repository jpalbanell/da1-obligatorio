using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class EstadioServicio : IEstadioServicio
    {
        private readonly IEstadioRepositorio _repositorio;
        private int _proximoId = 1;

        public EstadioServicio(IEstadioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public void AgregarEstadio(Estadio estadio)
        {
            ValidarNombreUnico(estadio.Nombre);
            estadio.Id = _proximoId++;
            _repositorio.Agregar(estadio);
        }

        private void ValidarNombreUnico(string nombre)
        {
            var existente = _repositorio.ObtenerTodos()
                .Any(e => e.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (existente)
                throw new Exception("Ya existe un estadio con ese nombre.");
        }

        public Estadio ObtenerEstadio(int id)
        {
            return _repositorio.ObtenerPorId(id);
        }

        public List<Estadio> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }

        public void ModificarEstadio(Estadio estadio)
        {
            ValidarNombreUnicoAlModificar(estadio);
            _repositorio.Actualizar(estadio);
        }

        private void ValidarNombreUnicoAlModificar(Estadio estadio)
        {
            var existente = _repositorio.ObtenerTodos()
                .Any(e => e.Nombre.Equals(estadio.Nombre, StringComparison.OrdinalIgnoreCase)
                          && e.Id != estadio.Id);
            if (existente)
                throw new Exception("Ya existe un estadio con ese nombre.");
        }

        public void EliminarEstadio(int id)
        {
            throw new NotImplementedException();
        }
    }
}