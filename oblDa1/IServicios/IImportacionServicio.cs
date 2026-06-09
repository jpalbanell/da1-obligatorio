using Dominio.Entidades;

namespace IServicios
{
    public interface IImportacionServicio
    {
        ResultadoImportacion ImportarEquipos(string contenidoCsv);
    }
}
