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
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConEquipoValido_AsignaEquipoCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);
        var equipo = new Equipo();
        equipo.Nombre = "Uruguay";

        posicion.Equipo = equipo;

        Assert.AreEqual(equipo, posicion.Equipo);
    }
}