using System.Collections.Generic;
using Fase03Interfaces.Domain.Interfaces;
using Fase03Interfaces.Domain.Algorithms;

namespace Fase03Interfaces.Services
{
    /// <summary>
    /// Catálogo de algoritmos de ordenação.
    /// Responsável por selecionar o algoritmo apropriado conforme política definida.
    /// </summary>
    public sealed class CatalogoAlgoritmos
    {
        private readonly Dictionary<string, IAlgoritmoOrdenacao> _catalogo;

        public CatalogoAlgoritmos()
        {
            _catalogo = new Dictionary<string, IAlgoritmoOrdenacao>
            {
                { "bubble", new BubbleSortAlgorithm() },
                { "insertion", new InsertionSortAlgorithm() },
                { "quick", new QuickSortAlgorithm() }
            };
        }

        /// <summary>
        /// Seleciona algoritmo baseado no tamanho da lista.
        /// Política: Bubble para pequenas, Insertion para médias, Quick para grandes.
        /// </summary>
        public IAlgoritmoOrdenacao SelecionarPorTamanho(int tamanho)
        {
            if (tamanho < 10)
                return _catalogo["bubble"];
            else if (tamanho < 50)
                return _catalogo["insertion"];
            else
                return _catalogo["quick"];
        }

        /// <summary>
        /// Obtém algoritmo por nome.
        /// </summary>
        public IAlgoritmoOrdenacao ObterPorNome(string nome)
        {
            var chave = nome?.ToLowerInvariant() ?? "quick";
            return _catalogo.TryGetValue(chave, out var algoritmo) 
                ? algoritmo 
                : _catalogo["quick"];
        }
    }
}
