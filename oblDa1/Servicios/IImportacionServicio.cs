using Dominio.Entidades;

namespace Servicios
{
    public interface IImportacionServicio
    {
        ResultadoImportacion ImportarEquipos(string contenidoCsv);
    }
}