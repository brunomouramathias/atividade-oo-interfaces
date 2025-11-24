using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Services
{
    // Serviço assíncrono que usa IAsyncRepository
    public sealed class AsyncBookService
    {
        private readonly IAsyncRepository<Book, int> _repository;

        public AsyncBookService(IAsyncRepository<Book, int> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Book> RegisterAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
            
            return await _repository.AddAsync(book);
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            return await _repository.UpdateAsync(book);
        }

        public async Task<bool> RemoveBookAsync(int id)
        {
            return await _repository.RemoveAsync(id);
        }

        public async Task<Book?> FindByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Book>> ListAllAsync()
        {
            return await _repository.ListAllAsync();
        }

        public async Task<int> CountBooksAsync()
        {
            var books = await _repository.ListAllAsync();
            return books.Count;
        }
    }
}

