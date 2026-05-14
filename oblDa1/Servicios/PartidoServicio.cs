using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class PartidoServicio : IPartidoServicio
    {
        private readonly IPartidoRepositorio _repositorio;
        private readonly IAuditoriaServicio _auditoriaServicio;
        private readonly ISesionServicio _sesionServicio;
        private int _proximoId = 1;

        public PartidoServicio(IPartidoRepositorio repositorio,
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _repositorio = repositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }

        public void AgregarPartido(Partido partido)
        {
            partido.Id = _proximoId++;
            _repositorio.Agregar(partido);
            _auditoriaServicio.Registrar($"Alta de partido: {partido.Codigo}", _sesionServicio.ObtenerUsuarioActual());
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
            ValidarRolEditor();
            ValidarPartidoNoBloqueado(partido);
            MarcarResultadoSiCorresponde(partido);
            ActualizarPosiciones(partido); 
            PropagarResultado(partido);
            _repositorio.Actualizar(partido);
            _auditoriaServicio.Registrar($"Modificación de partido: {partido.Id}", _sesionServicio.ObtenerUsuarioActual());
        }
        
        private void ValidarRolEditor()
        {
            var usuario = _sesionServicio.ObtenerUsuarioActual();
            if (!usuario.TieneRol(Rol.Editor))
                throw new UnauthorizedAccessException("Se requiere rol Editor para modificar partidos.");
        }

        private void MarcarResultadoSiCorresponde(Partido partido)
        {
            if (partido.GolesLocal >= 0 && partido.GolesVisitante >= 0)
                partido.TieneResultado = true;
        }

        private void ValidarPartidoNoBloqueado(Partido partido)
        {
            if (partido.EstaBloqueado)
                throw new InvalidOperationException("No se puede editar un partido bloqueado");
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
                .Where(p => p.Estadio != null && 
                            p.Estadio.Nombre.Equals(nombreEstadio, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        public List<Partido> ObtenerPorGrupo(string etiquetaGrupo)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Grupo != null && p.Grupo.Etiqueta == etiquetaGrupo)
                .ToList();
        }
        
        public List<Partido> ObtenerPorFase(FaseTorneo fase)
        {
            return _repositorio.ObtenerTodos()
                .Where(p => p.Fase == fase)
                .ToList();
        }
        
        public void ActualizarPartido(Partido partido)
        {
            _repositorio.Actualizar(partido);
        }
        
        public void PropagarResultado(Partido partido)
        {
            if (partido.Vencedor == null) return;
            var siguientes = _repositorio.ObtenerTodos()
                .Where(p => p.OrigenLocal?.Id == partido.Id || p.OrigenVisitante?.Id == partido.Id)
                .ToList();
            foreach (var siguiente in siguientes)
            {
                var equipo = siguiente.EsPorPerdedor ? partido.ObtenerPerdedor() : partido.Vencedor;
                if (siguiente.OrigenLocal?.Id == partido.Id)
                    siguiente.EquipoLocal = equipo;
                else
                    siguiente.EquipoVisitante = equipo;
                _repositorio.Actualizar(siguiente);
            }
        }
        
        private void ActualizarPosiciones(Partido partido)
        {
            if (partido.Grupo == null) return;

            var posLocal = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoLocal.Nombre);

            var posVisitante = partido.Grupo.ListaPosiciones
                .FirstOrDefault(p => p.Equipo.Nombre == partido.EquipoVisitante.Nombre);

            if (posLocal == null || posVisitante == null) return;

            posLocal.AplicarResultado(partido.GolesLocal, partido.GolesVisitante);
            posVisitante.AplicarResultado(partido.GolesVisitante, partido.GolesLocal);
        }
    }
}