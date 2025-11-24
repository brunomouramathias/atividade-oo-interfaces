using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Tests.Dubles
{
    // STUB: Retorna valores fixos pré-configurados
    // Usado para simular respostas específicas
    public sealed class StubRepository : IAsyncRepository<Book, int>
    {
        private readonly List<Book> _fixedData;

        public StubRepository(List<Book> fixedData)
        {
            _fixedData = fixedData ?? new List<Book>();
        }

        public Task<Book> AddAsync(Book entity)
        {
            return Task.FromResult(entity);
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            var book = _fixedData.FirstOrDefault(b => b.Id == id);
            return Task.FromResult(book);
        }

        public Task<IReadOnlyList<Book>> ListAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Book>>(_fixedData);
        }

        public Task<bool> UpdateAsync(Book entity)
        {
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(int id)
        {
            return Task.FromResult(true);
        }
    }
}

