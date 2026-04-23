using Dominio.Entidades;
using Repositorios;

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
            if (string.IsNullOrEmpty(accion))
                throw new ArgumentException("Accion no puede ser nula o vacía");

            var log = new LogAuditoria();
            log.Timestamp = DateTime.Now;
            log.Accion = accion;
            log.Usuario = usuario;
            _auditoriaRepositorio.Agregar(log);
        }
    }
}