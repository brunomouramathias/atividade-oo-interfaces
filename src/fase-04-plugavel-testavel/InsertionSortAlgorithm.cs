namespace Fase04PlugavelTestavel
{
    public sealed class InsertionSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
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

