namespace Dominio.Entidades
{
    public class Notificacion
    {
        private string _mensaje;
        private DateTime _fechaCreacion;

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
