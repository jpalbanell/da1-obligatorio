using System;

namespace Dominio;

public class Estadio
{
    private string _nombre = string.Empty;

    public string Nombre
    {
        get => _nombre;
        set
        {
            ValidarNombre(value);
            _nombre = value;
        }
    }

    public string Ciudad { get; set; } = string.Empty;

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
}