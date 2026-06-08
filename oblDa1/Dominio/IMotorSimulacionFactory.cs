namespace Dominio
{
    public interface IMotorSimulacionFactory
    {
        IMotorSimulacion Obtener(string nombre);
        IEnumerable<string> ObtenerNombres();
    }
}
