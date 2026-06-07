using System.Text;

namespace Servicios.Exportacion
{
    public class ExportadorCSV: Exportador
    {
        private const char SeparadorDeColumnas = ',';
        private const char SeparadorDeFilas = '\n';
        private const char Comilla = '"';

        public override byte[] Exportar(TablaExportable tabla)
        {
            var sb = new StringBuilder();

            sb.Append(string.Join(SeparadorDeColumnas, tabla.Encabezados.Select(Escapar)));
            sb.Append(SeparadorDeFilas);

            foreach (var fila in tabla.Filas)
            {
                sb.Append(string.Join(SeparadorDeColumnas, fila.Select(Escapar)));
                sb.Append(SeparadorDeFilas);
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private string Escapar(string valor)
        {
            bool necesitaComillas =
                valor.Contains(SeparadorDeColumnas) ||
                valor.Contains(Comilla) ||
                valor.Contains(SeparadorDeFilas);

            if (!necesitaComillas)
                return valor;

            string escapado = valor.Replace("\"", "\"\"");
            return $"\"{escapado}\"";
        }
    }
}