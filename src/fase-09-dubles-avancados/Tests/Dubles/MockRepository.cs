using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Tests.Dubles
{
    // MOCK: Verifica se chamadas esperadas foram feitas
    // Lança exceção se expectativas não forem atendidas
    public sealed class MockRepository : IAsyncRepository<Book, int>
    {
        private int _expectedAddCalls;
        private int _actualAddCalls;
        private Book? _expectedBook;

        public void ExpectAddAsync(Book book, int times = 1)
        {
            _expectedBook = book;
            _expectedAddCalls = times;
        }

        public void Verify()
        {
            if (_actualAddCalls != _expectedAddCalls)
            {
                throw new Exception($"Expectativa falhou: esperava {_expectedAddCalls} chamadas a AddAsync, mas recebeu {_actualAddCalls}");
            }
        }

        public Task<Book> AddAsync(Book entity)
        {
            _actualAddCalls++;
            
            if (_expectedBook != null && entity.Id != _expectedBook.Id)
            {
                throw new Exception($"Mock: esperava livro com ID {_expectedBook.Id}, recebeu {entity.Id}");
            }
            
            return Task.FromResult(entity);
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            return Task.FromResult<Book?>(null);
        }

        public Task<IReadOnlyList<Book>> ListAllAsync()
        {
            return Task.FromResult<IReadOnlyList<Book>>(new List<Book>());
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

