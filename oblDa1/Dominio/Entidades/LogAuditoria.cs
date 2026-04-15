namespace Dominio.Entidades
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        private DateTime _timestamp;
        private string _accion;

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
    }
}