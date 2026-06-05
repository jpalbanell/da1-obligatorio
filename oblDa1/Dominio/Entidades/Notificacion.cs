namespace Dominio.Entidades
{
    public class Notificacion
    {
        private string _mensaje;

        public string Mensaje
        {
            get => _mensaje;
            set
            {
                ValidarMensaje(value);
                _mensaje = value;
            }
        }

        private void ValidarMensaje(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                throw new ArgumentException("El mensaje es obligatorio.");
        }
    }
}
