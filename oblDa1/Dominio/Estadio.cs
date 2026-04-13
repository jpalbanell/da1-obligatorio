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
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }

            if (value.Length > 80)
            {
                throw new ArgumentException("El nombre no puede superar los 80 caracteres.");
            }

            _nombre = value;
        }
    }
}