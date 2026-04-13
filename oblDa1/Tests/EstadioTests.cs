using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests;

[TestClass]
public class EstadioTests
{
    [TestMethod]
    public void CrearEstadio_ConNombreValido_DeberiaAsignarNombre()
    {
        var estadio = new Estadio();

        estadio.Nombre = "Centenario";

        Assert.AreEqual("Centenario", estadio.Nombre);
    }
}