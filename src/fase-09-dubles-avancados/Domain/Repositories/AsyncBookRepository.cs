using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Domain.Repositories
{
    // Repositório assíncrono real (simulando I/O)
    public sealed class AsyncBookRepository : IAsyncRepository<Book, int>
    {
        private readonly Dictionary<int, Book> _store = new Dictionary<int, Book>();
        private readonly int _ioDelayMs;

        public AsyncBookRepository(int ioDelayMs = 50)
        {
            _ioDelayMs = ioDelayMs;
        }

        public async Task<Book> AddAsync(Book entity)
        {
            // Simula operação I/O assíncrona
            await Task.Delay(_ioDelayMs);
            _store[entity.Id] = entity;
            return entity;
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            await Task.Delay(_ioDelayMs);
            return _store.TryGetValue(id, out var book) ? book : null;
        }

        public async Task<IReadOnlyList<Book>> ListAllAsync()
        {
            await Task.Delay(_ioDelayMs);
            return _store.Values.OrderBy(b => b.Id).ToList();
        }

        public async Task<bool> UpdateAsync(Book entity)
        {
            await Task.Delay(_ioDelayMs);
            if (!_store.ContainsKey(entity.Id))
                return false;
            
            _store[entity.Id] = entity;
            return true;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await Task.Delay(_ioDelayMs);
            return _store.Remove(id);
        }
    }
}

