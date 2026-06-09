using Dominio.Entidades;
using IRepositorios;
using IServicios;

namespace Servicios
{
    public class AuditoriaServicio : IAuditoriaServicio
    {
        private IAuditoriaRepositorio _auditoriaRepositorio;

        public AuditoriaServicio(IAuditoriaRepositorio auditoriaRepositorio)
        {
            _auditoriaRepositorio = auditoriaRepositorio;
        }

        public void Registrar(string accion, Usuario usuario)
        {
            ValidarAccion(accion);
            ValidarUsuario(usuario);

            var log = new LogAuditoria();
            log.Timestamp = DateTime.UtcNow;
            log.Accion = accion;
            log.Usuario = usuario;
            _auditoriaRepositorio.Agregar(log);
        }

        public List<LogAuditoria> ObtenerTodos()
        {
            return _auditoriaRepositorio.ObtenerTodos();
        }

        private void ValidarAccion(string accion)
        {
            if (string.IsNullOrEmpty(accion))
                throw new ArgumentException("Accion no puede ser nula o vacía");
        }

        private void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException("Usuario no puede ser nulo");
        }
        
        public List<LogAuditoria> ObtenerEntreFechas(DateTime desde, DateTime hasta)
        {
            return _auditoriaRepositorio.ObtenerTodos()
                .Where(log => log.Timestamp >= desde && log.Timestamp <= hasta)
                .ToList();
        }
    }
}