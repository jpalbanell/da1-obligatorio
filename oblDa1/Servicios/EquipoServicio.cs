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
            if (_equipoRepositorio.ObtenerPorNombre(equipo.Nombre) != null)
                throw new Exception("Ya existe un equipo con ese nombre");
    
            _equipoRepositorio.Agregar(equipo);
        }
        
        
    }
}