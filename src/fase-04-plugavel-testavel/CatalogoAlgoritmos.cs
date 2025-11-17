namespace Fase04PlugavelTestavel
{
    public static class CatalogoAlgoritmos
    {
        public static IAlgoritmoOrdenacao Resolver(string modo)
        {
            return modo?.ToLowerInvariant() switch
            {
                "bubble" => new BubbleSortAlgorithm(),
                "quick" => new QuickSortAlgorithm(),
                "insertion" => new InsertionSortAlgorithm(),
                _ => new QuickSortAlgorithm() // padrão seguro
            };
        }

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

