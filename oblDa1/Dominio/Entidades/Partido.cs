namespace Dominio.Entidades
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
        public Partido OrigenLocal { get; set; }
        public Partido OrigenVisitante { get; set; }
        public bool EstaBloqueado { get; set; }
        public bool TieneResultado { get; set; } = false;
        public bool EsPorPerdedor { get; set; } = false;
        public int GolesLocalAnterior { get; set; } = -1;
        public int GolesVisitanteAnterior { get; set; } = -1;

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
            set { _equipoLocal = value; }
        }

        public Equipo EquipoVisitante
        {
            get => _equipoVisitante;
            set
            {
                if (value != null && value == _equipoLocal)
                    throw new ArgumentException("EquipoVisitante no puede ser igual al EquipoLocal");
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
        public Partido() { }

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
        
        private void ValidarVencedor(Equipo equipo)
        {
            if (equipo != null && _equipoLocal != null && _equipoVisitante != null)
            {
                if (equipo != _equipoLocal && equipo != _equipoVisitante)
                    throw new ArgumentException("Vencedor debe ser EquipoLocal o EquipoVisitante");
            }
        }
        
        public Equipo ObtenerPerdedor()
        {
            if (_vencedor == null)
                throw new InvalidOperationException("El partido no tiene vencedor asignado.");
            return _vencedor == _equipoLocal ? _equipoVisitante : _equipoLocal;
        }
        
        public bool PuedeModificarse()
        {
            return !EstaBloqueado;
        }
        
        public bool TieneEquiposCompletos()
        {
            return EquipoLocal != null && EquipoVisitante != null;
        }
        
        public bool EsEmpate()
        {
            return TieneResultado && GolesLocal == GolesVisitante;
        }
        
        public void DeterminarVencedor(Random random = null)
        {
            if (GolesLocal > GolesVisitante)
                Vencedor = EquipoLocal;
            else if (GolesVisitante > GolesLocal)
                Vencedor = EquipoVisitante;
            else if (Fase != FaseTorneo.FaseGrupos && random != null)
                Vencedor = random.Next(2) == 0 ? EquipoLocal : EquipoVisitante;
        }
        
        public void RegistrarResultado(int golesLocal, int golesVisitante, Random random = null)
        {
            if (TieneResultado)
            {
                GolesLocalAnterior = GolesLocal;
                GolesVisitanteAnterior = GolesVisitante;
            }
            GolesLocal = golesLocal;
            GolesVisitante = golesVisitante;
            TieneResultado = true;
            DeterminarVencedor(random);
        }
    }
}