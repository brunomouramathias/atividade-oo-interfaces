using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Interfaces;

namespace Fase09DublesAvancados.Tests.Dubles
{
    // DUMMY: Objeto que não faz nada, apenas preenche parâmetros
    // Usado quando o teste não precisa da funcionalidade real
    public sealed class DummyRepository : IAsyncRepository<Book, int>
    {
        public Task<Book> AddAsync(Book entity)
        {
            throw new NotImplementedException("Dummy: não deve ser chamado");
        }

        public Task<Book?> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Dummy: não deve ser chamado");
        }

        public Task<IReadOnlyList<Book>> ListAllAsync()
        {
            throw new NotImplementedException("Dummy: não deve ser chamado");
        }

        public Task<bool> UpdateAsync(Book entity)
        {
            throw new NotImplementedException("Dummy: não deve ser chamado");
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException("Dummy: não deve ser chamado");
        }
    }
}

