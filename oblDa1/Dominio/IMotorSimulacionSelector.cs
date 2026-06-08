namespace Dominio
{
    public interface IMotorSimulacionSelector
    {
        IMotorSimulacion Obtener(string nombre);
        IEnumerable<string> ObtenerNombres();
    }
}
