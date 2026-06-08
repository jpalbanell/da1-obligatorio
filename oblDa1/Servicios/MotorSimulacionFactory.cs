using Dominio;

namespace Servicios
{
    public class MotorSimulacionFactory : IMotorSimulacionFactory
    {
        private readonly Dictionary<string, IMotorSimulacion> _motores;

        public MotorSimulacionFactory(IEnumerable<IMotorSimulacion> motores)
        {
            _motores = motores.ToDictionary(m => m.Nombre);
        }

        public IMotorSimulacion Obtener(string nombre)
        {
            if (!_motores.TryGetValue(nombre, out var motor))
                throw new ArgumentException($"Motor de simulación '{nombre}' no encontrado.");
            return motor;
        }

        public IEnumerable<string> ObtenerNombres() => _motores.Keys;
    }
}
