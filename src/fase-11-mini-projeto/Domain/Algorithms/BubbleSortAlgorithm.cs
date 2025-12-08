using Fase11MiniProjeto.Domain.Interfaces;

namespace Fase11MiniProjeto.Domain.Algorithms
{
    /// <summary>
    /// Implementação do Bubble Sort.
    /// O(n²) - Recomendado para listas pequenas.
    /// </summary>
    public sealed class BubbleSortAlgorithm : IAlgoritmoOrdenacao
    {
        public string Nome => "Bubble Sort";

        public int[] Ordenar(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? System.Array.Empty<int>();

            int[] copia = (int[])lista.Clone();
            int n = copia.Length;
            
            for (int i = 0; i < n - 1; i++)
            {
                bool trocou = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (copia[j] > copia[j + 1])
                    {
                        (copia[j], copia[j + 1]) = (copia[j + 1], copia[j]);
                        trocou = true;
                    }
                }
                if (!trocou) break;
            }
            
            return copia;
        }
    }
}
