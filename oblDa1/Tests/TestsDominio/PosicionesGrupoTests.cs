using Dominio.Entidades;

namespace Tests;

[TestClass]
public class PosicionesGrupoTests
{
    private PosicionesGrupo CrearPosicion()
    {
        var posicion = new PosicionesGrupo(1);
        var equipo = new Equipo { Nombre = "Uruguay" };
        var grupo = new Grupo();
        grupo.Etiqueta = "A";
        posicion.Equipo = equipo;
        posicion.Grupo = grupo;
        return posicion;
    }
    
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
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConGrupoNulo_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.Grupo = null;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConPuntosNegativos_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.Puntos = -1;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConGolesFavorNegativos_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.GolesFavor = -1;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConGolesContraNegativos_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.GolesContra = -1;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConPosicionFinalMenorA1_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.PosicionFinal = 0;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearPosicionesGrupo_ConPosicionFinalMayorA4_DeberiaLanzarExcepcion()
    {
        var posicion = new PosicionesGrupo(1);
        posicion.PosicionFinal = 5;
    }
    
    [TestMethod]
    public void AplicarResultado_Victoria_DeberiaActualizarGolesPuntosYDiferencia()
    {
        var posicion = CrearPosicion();

        posicion.AplicarResultado(2, 0);

        Assert.AreEqual(2, posicion.GolesFavor);
        Assert.AreEqual(0, posicion.GolesContra);
        Assert.AreEqual(2, posicion.DiferenciaGoles);
        Assert.AreEqual(3, posicion.Puntos);
    }

    [TestMethod]
    public void AplicarResultado_Empate_DeberiaActualizarGolesPuntosYDiferencia()
    {
        var posicion = CrearPosicion();

        posicion.AplicarResultado(1, 1);

        Assert.AreEqual(1, posicion.GolesFavor);
        Assert.AreEqual(1, posicion.GolesContra);
        Assert.AreEqual(0, posicion.DiferenciaGoles);
        Assert.AreEqual(1, posicion.Puntos);
    }

    [TestMethod]
    public void AplicarResultado_Derrota_DeberiaActualizarGolesPuntosYDiferencia()
    {
        var posicion = CrearPosicion();

        posicion.AplicarResultado(0, 2);

        Assert.AreEqual(0, posicion.GolesFavor);
        Assert.AreEqual(2, posicion.GolesContra);
        Assert.AreEqual(-2, posicion.DiferenciaGoles);
        Assert.AreEqual(0, posicion.Puntos);
    }
}