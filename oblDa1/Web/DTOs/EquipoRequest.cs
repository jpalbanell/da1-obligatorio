using Dominio.Entidades;

namespace Web.DTOs
{
    public class EquipoRequest
    {
        public string Nombre { get; set; } = "";
        public string Confederacion { get; set; } = "";
        public int RankingFifa { get; set; }

        public Equipo ToEntity()
        {
            if (string.IsNullOrEmpty(Confederacion) || Confederacion == "")
                throw new Exception("Debe seleccionar una confederación.");
            
            var equipo = new Equipo();
            equipo.Nombre = Nombre;
            equipo.Confederacion = Enum.Parse<Confederacion>(Confederacion);
            equipo.RankingFifa = RankingFifa;
            return equipo;
        }
    }
}