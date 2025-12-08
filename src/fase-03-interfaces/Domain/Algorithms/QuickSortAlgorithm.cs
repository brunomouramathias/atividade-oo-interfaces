using Fase03Interfaces.Domain.Interfaces;

namespace Fase03Interfaces.Domain.Algorithms
{
    /// <summary>
    /// Implementação do algoritmo Quick Sort.
    /// Complexidade média: O(n log n) - Recomendado para listas grandes.
    /// </summary>
    public sealed class QuickSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? System.Array.Empty<int>();

            int[] copia = (int[])lista.Clone();
            QuickSort(copia, 0, copia.Length - 1);
            return copia;
        }

        private void QuickSort(int[] array, int inicio, int fim)
        {
            if (inicio < fim)
            {
                int pivo = Particionar(array, inicio, fim);
                QuickSort(array, inicio, pivo - 1);
                QuickSort(array, pivo + 1, fim);
            }
        }

        private int Particionar(int[] array, int inicio, int fim)
        {
            int pivo = array[fim];
            int i = inicio - 1;

            for (int j = inicio; j < fim; j++)
            {
                if (array[j] < pivo)
                {
                    i++;
                    (array[i], array[j]) = (array[j], array[i]);
                }
            }

            (array[i + 1], array[fim]) = (array[fim], array[i + 1]);
            return i + 1;
        }
    }
}
