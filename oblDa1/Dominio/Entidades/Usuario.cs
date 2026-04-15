namespace Dominio.Entidades
{
    public class Usuario
    {
        public List<Rol> Roles { get; set; } = new List<Rol>();
        private string _nombre;
        private string _apellido;

        public string Nombre
        {
            get => _nombre;
            set
            {
                ValidarNombre(value);
                _nombre = value;
            }
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");
        }

        public string Apellido
        {
            get => _apellido;
            set
            {
                ValidarApellido(value);
                _apellido = value;
            }
        }

        private void ValidarApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es obligatorio.");
        }
    }
}