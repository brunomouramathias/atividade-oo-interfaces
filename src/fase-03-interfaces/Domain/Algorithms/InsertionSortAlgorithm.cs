using Fase03Interfaces.Domain.Interfaces;

namespace Fase03Interfaces.Domain.Algorithms
{
    /// <summary>
    /// Implementação do algoritmo Insertion Sort.
    /// Complexidade: O(n²) - Eficiente para listas pequenas ou quase ordenadas.
    /// </summary>
    public sealed class InsertionSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? System.Array.Empty<int>();

            int[] copia = (int[])lista.Clone();
            int n = copia.Length;
            
            for (int i = 1; i < n; i++)
            {
                int chave = copia[i];
                int j = i - 1;

                while (j >= 0 && copia[j] > chave)
                {
                    copia[j + 1] = copia[j];
                    j--;
                }
                copia[j + 1] = chave;
            }
            
            return copia;
        }
    }
}
