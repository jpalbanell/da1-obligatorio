namespace IServicios
{
    public interface IMotorSimulacionSelector
    {
        IMotorSimulacion Obtener(string nombre);
        IEnumerable<string> ObtenerNombres();
    }
}
