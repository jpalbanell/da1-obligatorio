namespace Dominio.Entidades
{
    public class Usuario
    {
        public List<Rol> Roles { get; set; } = new List<Rol>();
        private string _nombre;
        private string _apellido;
        private string _email;
        private DateTime _fechaNacimiento;

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

        public string Email
        {
            get => _email;
            set
            {
                ValidarEmail(value);
                _email = value;
            }
        }

        private void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es obligatorio.");
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                ValidarFechaNacimiento(value);
                _fechaNacimiento = value;
            }
        }

        private void ValidarFechaNacimiento(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("La fecha de nacimiento es obligatoria.");
        }
    }
}