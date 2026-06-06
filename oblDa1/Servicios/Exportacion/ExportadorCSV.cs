using System.Text;

namespace Servicios.Exportacion
{
    public class ExportadorCSV
    {
        private const char SeparadorDeColumnas = ',';
        private const char SeparadorDeFilas = '\n';

        public byte[] Exportar(TablaExportable tabla)
        {
            var sb = new StringBuilder();

            sb.Append(string.Join(SeparadorDeColumnas, tabla.Encabezados));
            sb.Append(SeparadorDeFilas);

            foreach (var fila in tabla.Filas)
            {
                sb.Append(string.Join(SeparadorDeColumnas, fila));
                sb.Append(SeparadorDeFilas);
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}