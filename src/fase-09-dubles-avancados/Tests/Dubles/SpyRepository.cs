using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Tests.Dubles
{
    // SPY: Registra as chamadas feitas (espiona o comportamento)
    // Usado para verificar se métodos foram chamados e com quais parâmetros
    public sealed class SpyRepository : IAsyncRepository<Book, int>
    {
        private readonly List<Book> _store = new List<Book>();
        
        // Registros de chamadas
        public int AddAsyncCallCount { get; private set; }
        public int GetByIdAsyncCallCount { get; private set; }
        public int ListAllAsyncCallCount { get; private set; }
        public int UpdateAsyncCallCount { get; private set; }
        public int RemoveAsyncCallCount { get; private set; }
        
        public List<Book> AddedBooks { get; } = new List<Book>();
        public List<int> QueriedIds { get; } = new List<int>();
        public List<int> RemovedIds { get; } = new List<int>();

        public Task<Book> AddAsync(Book entity)
        {
            AddAsyncCallCount++;
            AddedBooks.Add(entity);
            _store.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            GetByIdAsyncCallCount++;
            QueriedIds.Add(id);
            return Task.FromResult(_store.FirstOrDefault(b => b.Id == id));
        }

        public Task<IReadOnlyList<Book>> ListAllAsync()
        {
            ListAllAsyncCallCount++;
            return Task.FromResult<IReadOnlyList<Book>>(_store);
        }

        public Task<bool> UpdateAsync(Book entity)
        {
            UpdateAsyncCallCount++;
            var index = _store.FindIndex(b => b.Id == entity.Id);
            if (index >= 0)
            {
                _store[index] = entity;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveAsync(int id)
        {
            RemoveAsyncCallCount++;
            RemovedIds.Add(id);
            var removed = _store.RemoveAll(b => b.Id == id) > 0;
            return Task.FromResult(removed);
        }
    }
}

