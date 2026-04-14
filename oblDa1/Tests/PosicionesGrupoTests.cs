using Dominio.Entidades;

namespace Tests;

[TestClass]
public class PosicionesGrupoTests
{
    [TestMethod]
    public void CrearPosicionesGrupo_ConIdValido_AsignaIdCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);
        Assert.AreEqual(1, posicion.Id);
    }
}