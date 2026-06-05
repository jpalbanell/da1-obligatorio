namespace Dominio.Entidades
{
    public class Notificacion
    {
        private string _mensaje;
        private DateTime _fechaCreacion;
        private Usuario _periodista;

        public string Mensaje
        {
            get => _mensaje;
            set
            {
                ValidarMensaje(value);
                _mensaje = value;
            }
        }

        public DateTime FechaCreacion
        {
            get => _fechaCreacion;
            set
            {
                ValidarFechaCreacion(value);
                _fechaCreacion = value;
            }
        }

        public virtual Usuario Periodista
        {
            get => _periodista;
            set
            {
                if (value == null)
                    throw new ArgumentException("El periodista es obligatorio.");
                _periodista = value;
            }
        }

        public bool Leida { get; private set; } = false;

        public void MarcarLeida() => Leida = true;

        private void ValidarMensaje(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                throw new ArgumentException("El mensaje es obligatorio.");
            if (mensaje.Length > 500)
                throw new ArgumentException("El mensaje no puede superar los 500 caracteres.");
        }

        private void ValidarFechaCreacion(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("La fecha de creación es obligatoria.");
        }
    }
}
