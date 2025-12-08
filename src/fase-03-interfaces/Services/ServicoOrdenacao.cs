using System;
using Fase03Interfaces.Domain.Interfaces;

namespace Fase03Interfaces.Services
{
    /// <summary>
    /// Serviço de ordenação que utiliza inversão de dependência.
    /// Recebe a implementação via construtor (injeção de dependência).
    /// </summary>
    public sealed class ServicoOrdenacao
    {
        private readonly IAlgoritmoOrdenacao _algoritmo;

        public ServicoOrdenacao(IAlgoritmoOrdenacao algoritmo)
        {
            _algoritmo = algoritmo ?? throw new ArgumentNullException(nameof(algoritmo));
        }

        /// <summary>
        /// Processa e ordena uma lista de inteiros.
        /// </summary>
        public int[] OrdenarLista(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? Array.Empty<int>();
            
            return _algoritmo.Ordenar(lista);
        }
    }
}
