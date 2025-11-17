namespace Fase04PlugavelTestavel
{
    public sealed class BubbleSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
            int[] copia = (int[])lista.Clone();
            int n = copia.Length;
            
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (copia[j] > copia[j + 1])
                    {
                        int temp = copia[j];
                        copia[j] = copia[j + 1];
                        copia[j + 1] = temp;
                    }
                }
            }
            
            return copia;
        }
    }
}

