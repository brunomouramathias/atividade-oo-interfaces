using System;
using System.Collections.Generic;
using Fase06RepositoryCsv.Domain;
using Fase06RepositoryCsv.Domain.Interfaces;

namespace Fase06RepositoryCsv.Services
{
    // Serviço de domínio que fala só com o Repository
    public static class BookService
    {
        public static Book Register(IRepository<Book, int> repo, Book book)
        {
            // Validações de domínio
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
            
            return repo.Add(book);
        }

        public static IReadOnlyList<Book> ListAll(IRepository<Book, int> repo)
        {
            return repo.ListAll();
        }

        public static Book? FindById(IRepository<Book, int> repo, int id)
        {
            return repo.GetById(id);
        }

        public static bool UpdateBook(IRepository<Book, int> repo, Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Título não pode ser vazio", nameof(book));
            
            if (string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("Autor não pode ser vazio", nameof(book));
            
            return repo.Update(book);
        }

        public static bool RemoveBook(IRepository<Book, int> repo, int id)
        {
            return repo.Remove(id);
        }
    }
}
