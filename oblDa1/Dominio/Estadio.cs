using System;

namespace Dominio;

public class Estadio
{
    private string _nombre = string.Empty;
    private string _ciudad = string.Empty;

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
}