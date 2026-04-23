using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class PartidoServicio : IPartidoServicio
    {
        private readonly IPartidoRepositorio _repositorio;
        private int _proximoId = 1;

        public PartidoServicio(IPartidoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public void AgregarPartido(Partido partido)
        {
            partido.Id = _proximoId++;
            _repositorio.Agregar(partido);
        }

        public List<Partido> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }
        
        public Partido ObtenerPartido(int id)
        {
            return _repositorio.ObtenerPorId(id);
        }
        
        public void ModificarPartido(Partido partido)
        {
            _repositorio.Actualizar(partido);
        }
        
        public List<Partido> ObtenerPorFecha(DateTime fecha)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Fecha.Date == fecha.Date)
                .ToList();
        }
        
        public List<Partido> ObtenerPorEstadio(string nombreEstadio)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Estadio.Nombre.Equals(nombreEstadio, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        public List<Partido> ObtenerPorGrupo(string etiquetaGrupo)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Grupo.Etiqueta == etiquetaGrupo)
                .ToList();
        }
        
        public List<Partido> ObtenerPorFase(FaseTorneo fase)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Fase == fase)
                .ToList();
        }
    }
}