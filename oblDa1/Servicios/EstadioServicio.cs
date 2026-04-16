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
            estadio.Id = _proximoId++;
            _repositorio.Agregar(estadio);
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
            throw new NotImplementedException();
        }

        public void EliminarEstadio(int id)
        {
            throw new NotImplementedException();
        }
    }
}