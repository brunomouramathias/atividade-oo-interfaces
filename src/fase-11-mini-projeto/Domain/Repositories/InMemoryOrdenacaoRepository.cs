using System;
using System.Collections.Generic;
using System.Linq;
using Fase11MiniProjeto.Domain.Entities;
using Fase11MiniProjeto.Domain.Interfaces;

namespace Fase11MiniProjeto.Domain.Repositories
{
    /// <summary>
    /// Repositório InMemory para resultados de ordenação.
    /// Implementa interface completa (leitura + escrita).
    /// </summary>
    public sealed class InMemoryOrdenacaoRepository : IRepository<OrdenacaoResultado, int>
    {
        private readonly Dictionary<int, OrdenacaoResultado> _store = new();
        private int _nextId = 1;

        public OrdenacaoResultado Add(OrdenacaoResultado entity)
        {
            var id = _nextId++;
            var novoResultado = entity with { Id = id };
            _store[id] = novoResultado;
            return novoResultado;
        }

        public OrdenacaoResultado? GetById(int id)
        {
            return _store.TryGetValue(id, out var entity) ? entity : null;
        }

        public IReadOnlyList<OrdenacaoResultado> ListAll()
        {
            return _store.Values.ToList();
        }

        public bool Update(OrdenacaoResultado entity)
        {
            if (!_store.ContainsKey(entity.Id))
                return false;
            
            _store[entity.Id] = entity;
            return true;
        }

        public bool Remove(int id)
        {
            return _store.Remove(id);
        }
    }
}
