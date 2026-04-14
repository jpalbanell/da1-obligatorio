namespace Dominio
{
    public class Partido
    {
        private string _codigo;
        private DateTime _fecha;
        private Equipo _equipoLocal;
        private Equipo _equipoVisitante;
        private Estadio _estadio;
        private Grupo _grupo;
        private int _golesLocal;
        private int _golesVisitante;
        private Equipo _vencedor;

        public int Id { get; set; }

        public string Codigo
        {
            get => _codigo;
            set
            {
                ValidarNoNuloOVacio(value, "Codigo");
                _codigo = value;
            }
        }

        public DateTime Fecha
        {
            get => _fecha;
            set
            {
                ValidarFecha(value);
                _fecha = value;
            }
        }

        public FaseTorneo Fase { get; set; }

        public Equipo EquipoLocal
        {
            get => _equipoLocal;
            set
            {
                ValidarNoNulo(value, "EquipoLocal");
                _equipoLocal = value;
            }
        }

        public Equipo EquipoVisitante
        {
            get => _equipoVisitante;
            set
            {
                ValidarNoNulo(value, "EquipoVisitante");
                ValidarEquipoDistintoDeLocal(value);
                _equipoVisitante = value;
            }
        }

        public Estadio Estadio
        {
            get => _estadio;
            set
            {
                ValidarNoNulo(value, "Estadio");
                _estadio = value;
            }
        }

        public Grupo Grupo
        {
            get => _grupo;
            set
            {
                ValidarNoNulo(value, "Grupo");
                _grupo = value;
            }
        }

        public int GolesLocal
        {
            get => _golesLocal;
            set
            {
                ValidarNoNegativo(value, "GolesLocal");
                _golesLocal = value;
            }
        }

        public int GolesVisitante
        {
            get => _golesVisitante;
            set
            {
                ValidarNoNegativo(value, "GolesVisitante");
                _golesVisitante = value;
            }
        }

        public Equipo Vencedor
        {
            get => _vencedor;
            set
            {
                ValidarVencedor(value);
                _vencedor = value;
            }
        }

        public Cruce Cruce { get; set; }

        public Partido(int id)
        {
            Id = id;
        }

        private void ValidarNoNuloOVacio(string valor, string campo)
        {
            if (string.IsNullOrEmpty(valor))
                throw new ArgumentException($"{campo} no puede ser nulo o vacío");
        }

        private void ValidarNoNulo(object valor, string campo)
        {
            if (valor == null)
                throw new ArgumentException($"{campo} no puede ser nulo");
        }

        private void ValidarFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("Fecha no puede ser vacía");
        }

        private void ValidarNoNegativo(int valor, string campo)
        {
            if (valor < 0)
                throw new ArgumentException($"{campo} no puede ser negativo");
        }

        private void ValidarEquipoDistintoDeLocal(Equipo equipo)
        {
            if (equipo == _equipoLocal)
                throw new ArgumentException("EquipoVisitante no puede ser igual al EquipoLocal");
        }

        private void ValidarVencedor(Equipo equipo)
        {
            if (equipo != null && equipo != _equipoLocal && equipo != _equipoVisitante)
                throw new ArgumentException("Vencedor debe ser EquipoLocal o EquipoVisitante");
        }
    }
}