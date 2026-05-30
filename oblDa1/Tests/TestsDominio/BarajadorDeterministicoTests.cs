using Dominio;

namespace Tests
{
    [TestClass]
    public class BarajadorDeterministicoTests
    {
        [TestMethod]
        public void Barajar_MismaSemilla_ProduceMismoResultado()
        {
            var lista1 = new List<int> { 1, 2, 3, 4, 5 };
            var lista2 = new List<int> { 1, 2, 3, 4, 5 };

            BarajadorDeterministico.Barajar(lista1, new Random(42));
            BarajadorDeterministico.Barajar(lista2, new Random(42));

            CollectionAssert.AreEqual(lista1, lista2);
            CollectionAssert.AreNotEqual(lista1, new List<int> { 1, 2, 3, 4, 5 });
        }
        
        [TestMethod]
        public void Barajar_ListaVacia_NoLanzaExcepcion()
        {
            var lista = new List<int>();
            BarajadorDeterministico.Barajar(lista, new Random(42));
            Assert.AreEqual(0, lista.Count);
        }
        
        [TestMethod]
        public void Barajar_ListaUnElemento_QuedaIgual()
        {
            var lista = new List<int> { 99 };
            BarajadorDeterministico.Barajar(lista, new Random(42));
            Assert.AreEqual(99, lista[0]);
        }
        
        [TestMethod]
        public void Barajar_ContieneLosMismosElementos()
        {
            var original = new List<int> { 1, 2, 3, 4, 5 };
            var lista = new List<int> { 1, 2, 3, 4, 5 };

            BarajadorDeterministico.Barajar(lista, new Random(42));

            CollectionAssert.AreEquivalent(original, lista);
        }
        
        [TestMethod]
        public void BarajarSubLista_SoloBarajaIndicesIndicados()
        {
            var lista = new List<int> { 1, 2, 3, 4, 5 };
            var indices = new List<int> { 1, 2 };

            BarajadorDeterministico.BarajarSubLista(lista, indices, new Random(42));

            Assert.AreEqual(1, lista[0]);
            Assert.AreEqual(5, lista[4]);
            CollectionAssert.AreEquivalent(new List<int> { 2, 3 }, new List<int> { lista[1], lista[2] });
        }
    }
}