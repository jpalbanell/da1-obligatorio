using ClosedXML.Excel;

namespace Servicios.Exportacion
{
    public class ExportadorXLSX : Exportador
    {
        private const int FilaEncabezados = 1;
        private const int PrimeraColumna = 1;

        public override byte[] Exportar(TablaExportable tabla)
        {
            using var libro = new XLWorkbook();
            var hoja = libro.AddWorksheet("Datos");

            for (int columna = 0; columna < tabla.Encabezados.Count; columna++)
            {
                hoja.Cell(FilaEncabezados, PrimeraColumna + columna).Value = tabla.Encabezados[columna];
            }

            for (int fila = 0; fila < tabla.Filas.Count; fila++)
            {
                var valores = tabla.Filas[fila];
                for (int columna = 0; columna < valores.Count; columna++)
                {
                    hoja.Cell(FilaEncabezados + 1 + fila, PrimeraColumna + columna).Value = valores[columna];
                }
            }

            using var stream = new MemoryStream();
            libro.SaveAs(stream);
            return stream.ToArray();
        }
    }
}