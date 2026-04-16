using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class EquipoServicio : IEquipoServicio
    {
        private IEquipoRepositorio _equipoRepositorio;

        public EquipoServicio(IEquipoRepositorio equipoRepositorio)
        {
            _equipoRepositorio = equipoRepositorio;
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            _equipoRepositorio.Agregar(equipo);
        }
    }
}