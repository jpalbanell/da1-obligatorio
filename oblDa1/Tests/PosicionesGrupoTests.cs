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
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConGrupoValido_AsignaGrupoCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);
        var grupo = new Grupo();
        grupo.Etiqueta = "A";

        posicion.Grupo = grupo;

        Assert.AreEqual(grupo, posicion.Grupo);
    }
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConPuntosValidos_AsignaPuntosCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);

        posicion.Puntos = 3;

        Assert.AreEqual(3, posicion.Puntos);
    }
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConGolesFavorValidos_AsignaGolesFavorCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);

        posicion.GolesFavor = 5;

        Assert.AreEqual(5, posicion.GolesFavor);
    }
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConGolesContraValidos_AsignaGolesContraCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);

        posicion.GolesContra = 2;

        Assert.AreEqual(2, posicion.GolesContra);
    }
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConDiferenciaGolesValida_AsignaDiferenciaGolesCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);

        posicion.DiferenciaGoles = -3;

        Assert.AreEqual(-3, posicion.DiferenciaGoles);
    }
    
    [TestMethod]
    public void CrearPosicionesGrupo_ConPosicionFinalValida_AsignaPosicionFinalCorrectamente()
    {
        var posicion = new PosicionesGrupo(1);

        posicion.PosicionFinal = 2;

        Assert.AreEqual(2, posicion.PosicionFinal);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConEquipoNulo_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.Equipo = null;
    }
}