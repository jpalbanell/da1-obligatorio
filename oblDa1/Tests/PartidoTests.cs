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
}