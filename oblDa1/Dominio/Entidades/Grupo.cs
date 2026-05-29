namespace Dominio.Entidades
{
    public class Grupo
    {
        public static readonly string[] EtiquetasValidas =
            { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };

        private string _etiqueta;
        public int Id { get; set; }
        public List<Partido> ListaPartidos { get; private set; } = new List<Partido>();
        public List<PosicionesGrupo> ListaPosiciones { get; private set; } = new List<PosicionesGrupo>();
        

        public string Etiqueta
        {
            get => _etiqueta;
            set
            {
                ValidarEtiqueta(value);
                _etiqueta = value;
            }
        }
        
        public bool PuedeAgregarEquipo(Equipo equipo)
        {
            if (ListaPosiciones.Count >= 4)
                return false;

            var cantidadMismaConfederacion = ListaPosiciones
                .Select(p => p.Equipo)
                .Count(e => e.Confederacion == equipo.Confederacion);

            if (equipo.Confederacion == Confederacion.UEFA)
                return cantidadMismaConfederacion < 2;

            return cantidadMismaConfederacion == 0;
        }
        
        private void ValidarEtiqueta(string etiqueta)
        {
            if (string.IsNullOrWhiteSpace(etiqueta))
                throw new ArgumentException("La etiqueta es obligatoria.");
            if (!EtiquetasValidas.Contains(etiqueta))
                throw new ArgumentException("La etiqueta debe ser una letra entre A y L.");
        }
        
        public void AgregarEquipo(Equipo equipo)
        {
            if (!PuedeAgregarEquipo(equipo))
                throw new InvalidOperationException("No se puede agregar el equipo al grupo.");
    
            var posicion = new PosicionesGrupo();
            posicion.Equipo = equipo;
            posicion.Grupo = this;
            ListaPosiciones.Add(posicion);
        }
        
        public List<Equipo> ObtenerEquipos()
        {
            return ListaPosiciones.Select(p => p.Equipo).ToList();
        }
        
        public PosicionesGrupo ObtenerPosicionDeEquipo(string nombreEquipo)
        {
            return ListaPosiciones.FirstOrDefault(p => p.Equipo.Nombre == nombreEquipo);
        }
        
        public void ActualizarPosiciones(Partido partido)
        {
            if (partido == null) return;

            var posLocal = ObtenerPosicionDeEquipo(partido.EquipoLocal.Nombre);
            var posVisitante = ObtenerPosicionDeEquipo(partido.EquipoVisitante.Nombre);

            if (posLocal == null || posVisitante == null) return;

            if (partido.GolesLocalAnterior >= 0)
            {
                posLocal.RevertirResultado(partido.GolesLocalAnterior, partido.GolesVisitanteAnterior);
                posVisitante.RevertirResultado(partido.GolesVisitanteAnterior, partido.GolesLocalAnterior);
            }

            posLocal.AplicarResultado(partido.GolesLocal, partido.GolesVisitante);
            posVisitante.AplicarResultado(partido.GolesVisitante, partido.GolesLocal);
        }
    }
}