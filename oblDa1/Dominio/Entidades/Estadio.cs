using System;
using System.Collections.Generic;
namespace Dominio.Entidades;

public class Estadio
{
    private string _nombre = string.Empty;
    private string _ciudad = string.Empty;
    private string? _descripcion;
    private int _capacidad;
    public List<Partido> Partidos { get; set; } = new List<Partido>();

    public int Capacidad
    {
        get => _capacidad;
        set
        {
            ValidarCapacidad(value);
            _capacidad = value;
        }
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            ValidarNombre(value);
            _nombre = value;
        }
    }

    public string Ciudad
    {
        get => _ciudad;
        set
        {
            ValidarCiudad(value);
            _ciudad = value;
        }
    }

    public string? Descripcion
    {
        get => _descripcion;
        set
        {
            ValidarDescripcion(value);
            _descripcion = value;
        }
    }

    private void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.");
        }

        if (nombre.Length > 80)
        {
            throw new ArgumentException("El nombre no puede superar los 80 caracteres.");
        }
    }

    private void ValidarCiudad(string ciudad)
    {
        if (string.IsNullOrWhiteSpace(ciudad))
        {
            throw new ArgumentException("La ciudad es obligatoria.");
        }

        if (ciudad.Length > 60)
        {
            throw new ArgumentException("La ciudad no puede superar los 60 caracteres.");
        }
    }

    private void ValidarDescripcion(string? descripcion)
    {
        if (descripcion != null && descripcion.Length > 400)
        {
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        }
    }

    private void ValidarCapacidad(int capacidad)
    {
        if (capacidad < 20000)
        {
            throw new ArgumentException("La capacidad debe ser mayor o igual a 20000.");
        }
    }
}