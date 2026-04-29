using Dominio.Entidades;
using Repositorios;

namespace Servicios
{
    public class EquipoServicio : IEquipoServicio
    {
        private IEquipoRepositorio _equipoRepositorio;
        private IAuditoriaServicio _auditoriaServicio;
        private ISesionServicio _sesionServicio;

        public EquipoServicio(IEquipoRepositorio equipoRepositorio, 
            IAuditoriaServicio auditoriaServicio,
            ISesionServicio sesionServicio)
        {
            _equipoRepositorio = equipoRepositorio;
            _auditoriaServicio = auditoriaServicio;
            _sesionServicio = sesionServicio;
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            ValidarNombreUnico(equipo.Nombre);
            ValidarCupoConfederacion(equipo.Confederacion);
            _equipoRepositorio.Agregar(equipo);
            _auditoriaServicio.Registrar($"Alta de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void EditarEquipo(Equipo equipo)
        {
            ValidarNombreUnicoEnEdicion(equipo);
            _equipoRepositorio.Actualizar(equipo);
            _auditoriaServicio.Registrar($"Edición de equipo: {equipo.Nombre}", _sesionServicio.ObtenerUsuarioActual());
        }

        public void EliminarEquipo(string nombre)
        {
            ValidarEquipoExistente(nombre);
            _equipoRepositorio.Eliminar(nombre);
            _auditoriaServicio.Registrar($"Eliminación de equipo: {nombre}", _sesionServicio.ObtenerUsuarioActual());
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
        
        private void ValidarEquipoExistente(string nombre)
        {
            if (_equipoRepositorio.ObtenerPorNombre(nombre) == null)
                throw new Exception("No existe un equipo con ese nombre");
        }
        
        public void CompletarEquiposAutomaticamente(int semillaCompletar)
        {
            var random = new Random(semillaCompletar);
            int contador = 1;
            var resumen = new System.Text.StringBuilder();
            resumen.Append($"Generación automática de equipos con semilla: {semillaCompletar}. ");

            foreach (Confederacion confederacion in Enum.GetValues(typeof(Confederacion)))
            {
                int cupo = ObtenerCupoConfederacion(confederacion);
                int cantActual = _equipoRepositorio.ObtenerTodos()
                    .Count(e => e.Confederacion == confederacion);
                int cantGenerada = 0;

                for (int i = cantActual + 1; i <= cupo; i++)
                {
                    var equipo = new Equipo();
                    equipo.Nombre = $"{confederacion}_{contador:D2}";
                    equipo.Confederacion = confederacion;
                    equipo.RankingFifa = random.Next(300, 2501);
                    _equipoRepositorio.Agregar(equipo);
                    contador++;
                    cantGenerada++;
                }

                resumen.Append($"{confederacion}: {cantGenerada} equipos generados. ");
            }

            resumen.Append("Rangos: 300-2500.");
            _auditoriaServicio.Registrar(resumen.ToString(), _sesionServicio.ObtenerUsuarioActual());
        }
    }
}