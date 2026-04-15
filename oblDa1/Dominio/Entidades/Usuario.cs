namespace Dominio.Entidades
{
    public class Usuario
    {
        public List<Rol> Roles { get; set; } = new List<Rol>();
        private string _nombre;

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
    }
}