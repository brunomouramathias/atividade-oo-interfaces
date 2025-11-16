using System.Collections.Generic;

namespace Fase03Interfaces
{
    public class CatalogoAlgoritmos
    {
        private readonly Dictionary<string, IAlgoritmoOrdenacao> _catalogo;

        public CatalogoAlgoritmos()
        {
            _catalogo = new Dictionary<string, IAlgoritmoOrdenacao>
            {
                { "pequena", new BubbleSortAlgorithm() },
                { "media", new InsertionSortAlgorithm() },
                { "grande", new QuickSortAlgorithm() }
            };
        }

        public IAlgoritmoOrdenacao SelecionarPorTamanho(int tamanho)
        {
            if (tamanho < 10)
                return _catalogo["pequena"];
            else if (tamanho < 50)
                return _catalogo["media"];
            else
                return _catalogo["grande"];
        }

        public IAlgoritmoOrdenacao ObterAlgoritmo(string chave)
        {
            if (_catalogo.ContainsKey(chave))
                return _catalogo[chave];
            
            return _catalogo["grande"];
        }
    }
}

