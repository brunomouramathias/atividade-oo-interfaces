using System;

namespace Fase04PlugavelTestavel
{
    public sealed class ServicoOrdenacao
    {
        private readonly IAlgoritmoOrdenacao _algoritmo;

        public ServicoOrdenacao(IAlgoritmoOrdenacao algoritmo)
        {
            _algoritmo = algoritmo ?? throw new ArgumentNullException(nameof(algoritmo));
        }

        public int[] ProcessarLista(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? new int[0];
            
            return _algoritmo.Ordenar(lista);
        }
    }
}

