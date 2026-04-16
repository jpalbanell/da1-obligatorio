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

            int cupo = ObtenerCupoConfederacion(equipo.Confederacion);
            int cantActual = _equipoRepositorio.ObtenerTodos()
                .Count(e => e.Confederacion == equipo.Confederacion);
            if (cantActual >= cupo)
                throw new Exception("Cupo de confederación completo");

            _equipoRepositorio.Agregar(equipo);
        }

        private int ObtenerCupoConfederacion(Confederacion confederacion)
        {
            return confederacion switch
            {
                Confederacion.UEFA => 16,
                Confederacion.CONMEBOL => 7,
                Confederacion.CONCACAF => 7,
                Confederacion.CAF => 9,
                Confederacion.AFC => 8,
                Confederacion.OFC => 1,
                _ => throw new Exception("Confederación inválida")
            };
        }
        
        
    }
}