namespace Dominio.Entidades
{
    public class Grupo
    {
        private static readonly string[] EtiquetasValidas =
            { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };

        private string _etiqueta;
        
        public int Id { get; set; }

        public string Etiqueta
        {
            get => _etiqueta;
            set
            {
                ValidarEtiqueta(value);
                _etiqueta = value;
            }
        }
        
        private void ValidarEtiqueta(string etiqueta)
        {
            if (string.IsNullOrWhiteSpace(etiqueta))
                throw new ArgumentException("La etiqueta es obligatoria.");
            if (!EtiquetasValidas.Contains(etiqueta))
                throw new ArgumentException("La etiqueta debe ser una letra entre A y L.");
        }
    }
}