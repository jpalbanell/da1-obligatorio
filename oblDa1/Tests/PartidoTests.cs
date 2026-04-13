using Dominio;
namespace Tests;
[TestClass]
public class PartidoTests
{
    [TestMethod]
    public void CrearPartido_ConIdValido_AsignaIdCorrectamente()
    {
        // Arrange
        int idEsperado = 1;

        // Act
        var partido = new Partido(idEsperado);

        // Assert
        Assert.AreEqual(idEsperado, partido.Id);
    }
    
    [TestMethod]
    public void CrearPartido_ConCodigoValido_AsignaCodigoCorrectamente()
    {
        // Arrange
        int id = 1;
        string codigoEsperado = "P001";

        // Act
        var partido = new Partido(id, codigoEsperado);

        // Assert
        Assert.AreEqual(codigoEsperado, partido.Codigo);
    }
}