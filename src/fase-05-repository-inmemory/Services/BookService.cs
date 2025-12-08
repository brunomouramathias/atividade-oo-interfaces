using System;
using System.Collections.Generic;
using Fase05RepositoryInMemory.Domain.Entities;
using Fase05RepositoryInMemory.Domain.Interfaces;

namespace Fase05RepositoryInMemory.Services
{
    /// <summary>
    /// Serviço de domínio para gerenciamento de livros.
    /// Contém regras de negócio e validações.
    /// </summary>
    public sealed class BookService
    {
        private readonly IRepository<Book, int> _repository;

        public BookService(IRepository<Book, int> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Book Register(Book book)
        {
            ValidateBook(book);
            return _repository.Add(book);
        }

        public IReadOnlyList<Book> ListAll()
        {
            return _repository.ListAll();
        }

        public Book? FindById(int id)
        {
            return _repository.GetById(id);
        }

        public bool UpdateBook(Book book)
        {
            ValidateBook(book);
            return _repository.Update(book);
        }

        public bool RemoveBook(int id)
        {
            return _repository.Remove(id);
        }

        private static void ValidateBook(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
        }
    }
}
