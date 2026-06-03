namespace Dominio.Entidades
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        private DateTime _timestamp;
        private string _accion;
        private Usuario _usuario;

        public DateTime Timestamp
        {
            get => _timestamp;
            set
            {
                ValidarTimestamp(value);
                _timestamp = value;
            }
        }

        private void ValidarTimestamp(DateTime timestamp)
        {
            if (timestamp == default)
                throw new ArgumentException("El timestamp es obligatorio.");
        }

        public string Accion
        {
            get => _accion;
            set
            {
                ValidarAccion(value);
                _accion = value;
            }
        }

        private void ValidarAccion(string accion)
        {
            if (string.IsNullOrWhiteSpace(accion))
                throw new ArgumentException("La acción es obligatoria.");
        }

        public virtual Usuario Usuario
        {
            get => _usuario;
            set
            {
                ValidarUsuario(value);
                _usuario = value;
            }
        }

        private void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentException("El usuario es obligatorio.");
        }
    }
}