using ClosedXML.Excel;
using Servicios.Exportacion;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ExportadorXLSXTests
    {
        [TestMethod]
        public void Exportar_TablaConEncabezadosYUnaFila_GeneraXlsxConLasCeldas()
        {
            var tabla = new TablaExportable
            {
                Encabezados = new List<string> { "Nombre", "Confederacion" },
                Filas = new List<List<string>>
                {
                    new List<string> { "Uruguay", "CONMEBOL" }
                }
            };
            var exportador = new ExportadorXLSX();

            byte[] resultado = exportador.Exportar(tabla);

            using var stream = new MemoryStream(resultado);
            using var libro = new XLWorkbook(stream);
            var hoja = libro.Worksheet(1);
            Assert.AreEqual("Nombre", hoja.Cell(1, 1).GetString());
            Assert.AreEqual("Confederacion", hoja.Cell(1, 2).GetString());
            Assert.AreEqual("Uruguay", hoja.Cell(2, 1).GetString());
            Assert.AreEqual("CONMEBOL", hoja.Cell(2, 2).GetString());
        }
    }
}