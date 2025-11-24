using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Tests.Dubles
{
    // FAKE: Implementação funcional simplificada
    // Funciona como a real, mas mais simples (ex: em memória ao invés de BD)
    public sealed class FakeRepository : IAsyncRepository<Book, int>
    {
        private readonly Dictionary<int, Book> _store = new Dictionary<int, Book>();
        
        // Simula latência de rede/disco
        private readonly int _delayMs;

        public FakeRepository(int delayMs = 0)
        {
            _delayMs = delayMs;
        }

        public async Task<Book> AddAsync(Book entity)
        {
            if (_delayMs > 0)
                await Task.Delay(_delayMs);
            
            _store[entity.Id] = entity;
            return entity;
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            if (_delayMs > 0)
                await Task.Delay(_delayMs);
            
            return _store.TryGetValue(id, out var book) ? book : null;
        }

        public async Task<IReadOnlyList<Book>> ListAllAsync()
        {
            if (_delayMs > 0)
                await Task.Delay(_delayMs);
            
            return _store.Values.OrderBy(b => b.Id).ToList();
        }

        public async Task<bool> UpdateAsync(Book entity)
        {
            if (_delayMs > 0)
                await Task.Delay(_delayMs);
            
            if (!_store.ContainsKey(entity.Id))
                return false;
            
            _store[entity.Id] = entity;
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            if (_delayMs > 0)
                await Task.Delay(_delayMs);
            
            return _store.Remove(id);
        }
    }
}

