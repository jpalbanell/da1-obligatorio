using System.Text;
using Servicios.Exportacion;

namespace Tests.TestsServicios
{
    [TestClass]
    public class ExportadorCSVTest
    {
        [TestMethod]
        public void Exportar_TablaConEncabezadosYUnaFila_GeneraCsvCorrecto()
        {
            var tabla = new TablaExportable
            {
                Encabezados = new List<string> { "Nombre", "Confederacion" },
                Filas = new List<List<string>>
                {
                    new List<string> { "Uruguay", "CONMEBOL" }
                }
            };
            var exportador = new ExportadorCSV();

            byte[] resultado = exportador.Exportar(tabla);

            string csv = Encoding.UTF8.GetString(resultado);
            Assert.AreEqual("Nombre,Confederacion\nUruguay,CONMEBOL\n", csv);
        }
        
        [TestMethod]
        public void Exportar_ValorConComa_LoEnciertaEntreComillas()
        {
            var tabla = new TablaExportable
            {
                Encabezados = new List<string> { "Estadio", "Ciudad" },
                Filas = new List<List<string>>
                {
                    new List<string> { "Arena, SP", "Sao Paulo" }
                }
            };
            var exportador = new ExportadorCSV();

            byte[] resultado = exportador.Exportar(tabla);

            string csv = Encoding.UTF8.GetString(resultado);
            Assert.AreEqual("Estadio,Ciudad\n\"Arena, SP\",Sao Paulo\n", csv);
        }
    }
}