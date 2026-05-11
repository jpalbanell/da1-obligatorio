using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Dominio
{
    public static class UtilTexto
    {
        public static string Normalizar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            var resultado = texto.ToLowerInvariant();

            var formaDescompuesta = resultado.Normalize(NormalizationForm.FormD);
            var sinTildes = new StringBuilder();
            foreach (var c in formaDescompuesta)
            {
                var categoria = CharUnicodeInfo.GetUnicodeCategory(c);
                if (categoria != UnicodeCategory.NonSpacingMark)
                    sinTildes.Append(c);
            }
            resultado = sinTildes.ToString().Normalize(NormalizationForm.FormC);

            var conEspacios = new StringBuilder();
            foreach (var c in resultado)
            {
                conEspacios.Append(char.IsLetterOrDigit(c) ? c : ' ');
            }
            resultado = conEspacios.ToString();

            resultado = Regex.Replace(resultado, @"\s+", " ").Trim();

            return resultado;
        }
    }
}