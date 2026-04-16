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
            ValidarNombreUnico(equipo.Nombre);
            ValidarCupoConfederacion(equipo.Confederacion);
            _equipoRepositorio.Agregar(equipo);
        }

        public void EditarEquipo(Equipo equipo)
        {
            ValidarNombreUnicoEnEdicion(equipo);
            _equipoRepositorio.Actualizar(equipo);
        }

        public void EliminarEquipo(string nombre)
        {
            _equipoRepositorio.Eliminar(nombre);
        }
        
        public List<Equipo> ObtenerTodos()
        {
            return _equipoRepositorio.ObtenerTodos();
        }
        
        public Equipo ObtenerPorNombre(string nombre)
        {
            return _equipoRepositorio.ObtenerPorNombre(nombre);
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
        
        private void ValidarNombreUnico(string nombre)
        {
            if (_equipoRepositorio.ObtenerPorNombre(nombre) != null)
                throw new Exception("Ya existe un equipo con ese nombre");
        }

        private void ValidarNombreUnicoEnEdicion(Equipo equipo)
        {
            var equipoExistente = _equipoRepositorio.ObtenerPorNombre(equipo.Nombre);
            if (equipoExistente != null && equipoExistente != equipo)
                throw new Exception("Ya existe un equipo con ese nombre");
        }

        private void ValidarCupoConfederacion(Confederacion confederacion)
        {
            int cupo = ObtenerCupoConfederacion(confederacion);
            int cantActual = _equipoRepositorio.ObtenerTodos()
                .Count(e => e.Confederacion == confederacion);
            if (cantActual >= cupo)
                throw new Exception("Cupo de confederación completo");
        }
        
        
    }
}