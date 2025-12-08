using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fase11MiniProjeto.Domain.Algorithms;
using Fase11MiniProjeto.Domain.Entities;
using Fase11MiniProjeto.Domain.Interfaces;

namespace Fase11MiniProjeto.Services
{
    /// <summary>
    /// Serviço de ordenação. Consolida todos os conceitos:
    /// - Interfaces para algoritmos (Strategy Pattern)
    /// - Injeção de dependência
    /// - Repository para persistir resultados
    /// </summary>
    public sealed class OrdenacaoService
    {
        private readonly IRepository<OrdenacaoResultado, int> _repository;
        private readonly Dictionary<string, IAlgoritmoOrdenacao> _algoritmos;

        public OrdenacaoService(IRepository<OrdenacaoResultado, int> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            _algoritmos = new Dictionary<string, IAlgoritmoOrdenacao>
            {
                { "bubble", new BubbleSortAlgorithm() },
                { "quick", new QuickSortAlgorithm() }
            };
        }

        /// <summary>
        /// Ordena uma lista e salva o resultado no repositório.
        /// </summary>
        public OrdenacaoResultado Ordenar(int[] lista, string algoritmo = "quick")
        {
            if (lista == null || lista.Length == 0)
                throw new ArgumentException("Lista não pode ser vazia", nameof(lista));

            var algo = _algoritmos.TryGetValue(algoritmo.ToLowerInvariant(), out var a) 
                ? a 
                : _algoritmos["quick"];

            var sw = Stopwatch.StartNew();
            var ordenada = algo.Ordenar(lista);
            sw.Stop();

            var resultado = new OrdenacaoResultado(
                Id: 0,
                AlgoritmoUsado: algo.Nome,
                ListaOriginal: lista,
                ListaOrdenada: ordenada,
                TempoMs: sw.ElapsedMilliseconds
            );

            return _repository.Add(resultado);
        }

        /// <summary>
        /// Seleciona algoritmo baseado no tamanho da lista.
        /// Política: Bubble para listas pequenas, Quick para grandes.
        /// </summary>
        public OrdenacaoResultado OrdenarAutomatico(int[] lista)
        {
            var algoritmo = lista.Length < 10 ? "bubble" : "quick";
            return Ordenar(lista, algoritmo);
        }

        /// <summary>
        /// Lista histórico de ordenações.
        /// </summary>
        public IReadOnlyList<OrdenacaoResultado> ListarHistorico()
        {
            return _repository.ListAll();
        }

        /// <summary>
        /// Busca resultado por ID.
        /// </summary>
        public OrdenacaoResultado? BuscarPorId(int id)
        {
            return _repository.GetById(id);
        }
    }
}
