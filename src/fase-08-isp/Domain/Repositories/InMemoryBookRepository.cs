using System;
using System.Collections.Generic;
using System.Linq;
using Fase08Isp.Domain.Entities;
using Fase08Isp.Domain.Interfaces;

namespace Fase08Isp.Domain.Repositories
{
    // Implementação completa: IRepository<Book, int>
    public sealed class InMemoryBookRepository : IRepository<Book, int>
    {
        private readonly Dictionary<int, Book> _store = new Dictionary<int, Book>();

        // Implementação de IWriteRepository
        public Book Add(Book entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            _store[entity.Id] = entity;
            return entity;
        }

        public bool Update(Book entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            if (!_store.ContainsKey(entity.Id))
                return false;
            
            _store[entity.Id] = entity;
            return true;
        }

        public bool Remove(int id)
        {
            return _store.Remove(id);
        }

        // Implementação de IReadRepository
        public Book? GetById(int id)
        {
            return _store.TryGetValue(id, out var book) ? book : null;
        }

        public IReadOnlyList<Book> ListAll()
        {
            return _store.Values.OrderBy(b => b.Id).ToList();
        }
    }
}

