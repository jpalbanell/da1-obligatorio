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
                ValidarNombre(value);
                _nombre = value;
            }
        }
        
        public Confederacion Confederacion { get; set; }
        
        public int RankingFifa { get; set; }
        
        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");
            if (nombre.Length > 60)
                throw new ArgumentException("El nombre no puede superar los 60 caracteres.");
        } 
        
        private void ValidarRankingFifa(int ranking)
        {
            if (ranking < 300 || ranking > 2500)
                throw new ArgumentException("El ranking FIFA debe ser mayor o igual a 300.");
        }
    }
}