namespace Dominio
{
    public static class BarajadorDeterministico
    {
        public static void Barajar<T>(List<T> lista, Random random)
        {
            for (int i = lista.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (lista[i], lista[j]) = (lista[j], lista[i]);
            }
        }
        
        public static void BarajarSubLista<T>(List<T> lista, List<int> indices, Random random)
        {
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                int indiceI = indices[i];
                int indiceJ = indices[j];
                (lista[indiceI], lista[indiceJ]) = (lista[indiceJ], lista[indiceI]);
            }
        }
    }
}