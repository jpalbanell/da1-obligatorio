using System;
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

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEstadio_ConNombreVacio_DeberiaLanzarExcepcion()
    {
        var estadio = new Estadio();
        estadio.Nombre = "";
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEstadio_ConNombreMayorA80Caracteres_DeberiaLanzarExcepcion()
    {
        var estadio = new Estadio();
        string nombreLargo = new string('A', 81);
        estadio.Nombre = nombreLargo;
    }

    [TestMethod]
    public void CrearEstadio_ConCiudadValida_DeberiaAsignarCiudad()
    {
        var estadio = new Estadio();
        estadio.Ciudad = "Montevideo";
        Assert.AreEqual("Montevideo", estadio.Ciudad);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEstadio_ConCiudadVacia_DeberiaLanzarExcepcion()
    {
        var estadio = new Estadio();
        estadio.Ciudad = "";
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearEstadio_ConCiudadMayorA60Caracteres_DeberiaLanzarExcepcion()
    {
        var estadio = new Estadio();
        string ciudadLarga = new string('A', 61);
        estadio.Ciudad = ciudadLarga;
    }

    [TestMethod]
    public void CrearEstadio_ConDescripcionNula_DeberiaAsignarDescripcion()
    {
        var estadio = new Estadio();
        estadio.Descripcion = null;
        Assert.IsNull(estadio.Descripcion);
    }
}