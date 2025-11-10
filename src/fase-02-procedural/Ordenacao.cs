using System;

namespace Fase02Procedural
{
    public class Ordenacao
    {
        public static int[] OrdenarLista(int[] lista, string modo)
        {
            int[] copia = (int[])lista.Clone();
            
            switch (modo)
            {
                case "bubble":
                    return BubbleSort(copia);
                case "quick":
                    QuickSort(copia, 0, copia.Length - 1);
                    return copia;
                case "insertion":
                    return InsertionSort(copia);
                default:
                    return copia;
            }
        }

        private static int[] BubbleSort(int[] lista)
        {
            int n = lista.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (lista[j] > lista[j + 1])
                    {
                        int temp = lista[j];
                        lista[j] = lista[j + 1];
                        lista[j + 1] = temp;
                    }
                }
            }
            return lista;
        }

        private static void QuickSort(int[] lista, int inicio, int fim)
        {
            if (inicio < fim)
            {
                int pivo = Particionar(lista, inicio, fim);
                QuickSort(lista, inicio, pivo - 1);
                QuickSort(lista, pivo + 1, fim);
            }
        }

        private static int Particionar(int[] lista, int inicio, int fim)
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

        private static int[] InsertionSort(int[] lista)
        {
            int n = lista.Length;
            for (int i = 1; i < n; i++)
            {
                int chave = lista[i];
                int j = i - 1;

                while (j >= 0 && lista[j] > chave)
                {
                    lista[j + 1] = lista[j];
                    j--;
                }
                lista[j + 1] = chave;
            }
            return lista;
        }
    }
}

