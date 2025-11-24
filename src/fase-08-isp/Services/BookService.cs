using System;
using System.Collections.Generic;
using Fase08Isp.Domain.Entities;
using Fase08Isp.Domain.Interfaces;

namespace Fase08Isp.Services
{
    // Serviço que precisa de leitura E escrita (usa IRepository completo)
    public sealed class BookService
    {
        private readonly IRepository<Book, int> _repository;

        public BookService(IRepository<Book, int> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Book Register(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
            
            return _repository.Add(book);
        }

        public bool UpdateBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
            
            return _repository.Update(book);
        }

        public bool RemoveBook(int id)
        {
            return _repository.Remove(id);
        }

        public Book? FindById(int id)
        {
            return _repository.GetById(id);
        }

        public IReadOnlyList<Book> ListAll()
        {
            return _repository.ListAll();
        }
    }
}

