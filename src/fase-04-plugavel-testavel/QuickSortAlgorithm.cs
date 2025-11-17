namespace Fase04PlugavelTestavel
{
    public sealed class QuickSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
            int[] copia = (int[])lista.Clone();
            QuickSort(copia, 0, copia.Length - 1);
            return copia;
        }

        private void QuickSort(int[] lista, int inicio, int fim)
        {
            if (inicio < fim)
            {
                int pivo = Particionar(lista, inicio, fim);
                QuickSort(lista, inicio, pivo - 1);
                QuickSort(lista, pivo + 1, fim);
            }
        }

        private int Particionar(int[] lista, int inicio, int fim)
        {
            int pivo = lista[fim];
            int i = inicio - 1;

            for (int j = inicio; j < fim; j++)
            {
                if (lista[j] < pivo)
                {
                    i++;
                    int temp = lista[i];
                    lista[i] = lista[j];
                    lista[j] = temp;
                }
            }

            int temp2 = lista[i + 1];
            lista[i + 1] = lista[fim];
            lista[fim] = temp2;

            return i + 1;
        }
    }
}

