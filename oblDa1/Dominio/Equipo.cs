namespace Dominio
{
    public class Equipo
    {
        private string _nombre;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre es obligatorio.");
                _nombre = value;
            }
        }
    }
}