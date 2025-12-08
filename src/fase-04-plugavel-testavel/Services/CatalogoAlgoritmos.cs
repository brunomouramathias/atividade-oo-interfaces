using Fase04PlugavelTestavel.Domain.Interfaces;
using Fase04PlugavelTestavel.Domain.Algorithms;

namespace Fase04PlugavelTestavel.Services
{
    /// <summary>
    /// Catálogo estático para resolução de algoritmos.
    /// Implementa o padrão Service Locator simplificado.
    /// </summary>
    public static class CatalogoAlgoritmos
    {
        /// <summary>
        /// Resolve algoritmo por nome.
        /// </summary>
        public static IAlgoritmoOrdenacao Resolver(string modo)
        {
            return modo?.ToLowerInvariant() switch
            {
                "bubble" => new BubbleSortAlgorithm(),
                "quick" => new QuickSortAlgorithm(),
                "insertion" => new InsertionSortAlgorithm(),
                _ => new QuickSortAlgorithm()
            };
        }

        /// <summary>
        /// Resolve algoritmo por tamanho da lista.
        /// </summary>
        public static IAlgoritmoOrdenacao ResolverPorTamanho(int tamanho)
        {
            if (tamanho < 10)
                return new BubbleSortAlgorithm();
            else if (tamanho < 50)
                return new InsertionSortAlgorithm();
            else
                return new QuickSortAlgorithm();
        }
    }
}
