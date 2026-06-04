namespace Dominio.Entidades
{
    public class Equipo
    {
        private string _nombre;
        public Confederacion Confederacion { get; set; }
        private int _rankingFifa;
        public string? Bandera { get; set; }

        public string Nombre
        {
            get => _nombre;
            set
            {
                ValidarNombre(value);
                _nombre = value;
            }
        }
        
        public int RankingFifa
        {
            get => _rankingFifa;
            set
            {
                ValidarRankingFifa(value);
                _rankingFifa = value;
            }
        }

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
                throw new ArgumentException("El ranking FIFA debe estar entre 300 y 2500.");
        }
    }
}