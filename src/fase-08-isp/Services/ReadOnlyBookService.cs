using System;
using System.Collections.Generic;
using Fase08Isp.Domain.Entities;
using Fase08Isp.Domain.Interfaces;

namespace Fase08Isp.Services
{
    // Serviço que precisa APENAS de leitura (ISP aplicado)
    public sealed class ReadOnlyBookService
    {
        private readonly IReadRepository<Book, int> _readRepository;

        public ReadOnlyBookService(IReadRepository<Book, int> readRepository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        }

        public Book? FindById(int id)
        {
            return _readRepository.GetById(id);
        }

        public IReadOnlyList<Book> ListAll()
        {
            return _readRepository.ListAll();
        }

        public int CountBooks()
        {
            return _readRepository.ListAll().Count;
        }

        public bool BookExists(int id)
        {
            return _readRepository.GetById(id) != null;
        }
    }
}

