namespace Dominio.Entidades
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        private DateTime _timestamp;

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
    }
}