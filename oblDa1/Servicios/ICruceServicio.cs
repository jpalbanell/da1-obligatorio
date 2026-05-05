using Dominio.Entidades;

namespace Servicios
{
    public interface ICruceServicio
    {
        void GenerarCruces(int semillaCrucesFase, Usuario usuario);
    }
}