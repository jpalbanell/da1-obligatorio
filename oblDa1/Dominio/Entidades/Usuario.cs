namespace Dominio.Entidades
{
    public class Usuario
    {
        private string _nombre;
        private string _apellido;
        private string _email;
        private DateTime _fechaNacimiento;
        private string _contrasena;

        public int Id { get; set; }
        public List<Rol> Roles { get; set; } = new List<Rol>();

        public string Nombre
        {
            get => _nombre;
            set
            {
                ValidarNombre(value);
                _nombre = value;
            }
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

        public string Email
        {
            get => _email;
            set
            {
                ValidarEmail(value);
                _email = value;
            }
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

        public string Contrasena
        {
            get => _contrasena;
            set
            {
                ValidarContrasena(value);
                _contrasena = CifrarContrasena(value);
            }
        }
        
        public bool VerificarContrasena(string contrasena)
        {
            return _contrasena == CifrarContrasena(contrasena);
        }

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");
        }

        private void ValidarApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es obligatorio.");
        }

        private void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es obligatorio.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El formato del email no es válido.");
        }

        private void ValidarFechaNacimiento(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("La fecha de nacimiento es obligatoria.");
        }

        private void ValidarContrasena(string contrasena)
        {
            if (string.IsNullOrWhiteSpace(contrasena))
                throw new ArgumentException("La contraseña es obligatoria.");
            if (contrasena.Length < 8)
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            if (!contrasena.Any(char.IsUpper))
                throw new ArgumentException("La contraseña debe incluir al menos una letra mayúscula.");
            if (!contrasena.Any(char.IsLower))
                throw new ArgumentException("La contraseña debe incluir al menos una letra minúscula.");
            if (!contrasena.Any(char.IsDigit))
                throw new ArgumentException("La contraseña debe incluir al menos un número.");
            if (!contrasena.Any(c => "@#$.".Contains(c)))
                throw new ArgumentException("La contraseña debe incluir al menos un carácter especial (@, #, $, .).");
        }

        private string CifrarContrasena(string contrasena)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(contrasena);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        
        public bool TieneRol(Rol rol)
        {
            return Roles.Contains(rol);
        }
    }
}

