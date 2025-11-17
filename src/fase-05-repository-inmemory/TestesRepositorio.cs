using System;

namespace Fase05RepositoryInMemory
{
    // Testes do Repository sem I/O
    public static class TestesRepositorio
    {
        public static void ExecutarTodos()
        {
            Console.WriteLine("=== TESTES DO REPOSITORY ===\n");

            Teste1_Add_Then_ListAll_ShouldReturnOneItem();
            Teste2_GetById_Existing_ShouldReturnEntity();
            Teste3_GetById_Missing_ShouldReturnNull();
            Teste4_Update_Existing_ShouldReturnTrue();
            Teste5_Update_Missing_ShouldReturnFalse();
            Teste6_Remove_Existing_ShouldReturnTrue();
            Teste7_Remove_Missing_ShouldReturnFalse();
            Teste8_MultipleOperations_ShouldWork();

            Console.WriteLine("\n=== TODOS OS TESTES PASSARAM! ===\n");
        }

        private static InMemoryRepository<Book, int> CriarRepo()
        {
            return new InMemoryRepository<Book, int>(b => b.Id);
        }

        private static void Teste1_Add_Then_ListAll_ShouldReturnOneItem()
        {
            Console.WriteLine("Teste 1: Add_Then_ListAll_ShouldReturnOneItem");
            
            var repo = CriarRepo();
            repo.Add(new Book(1, "Livro A", "Autor"));
            var all = repo.ListAll();
            
            Verificar(all.Count == 1, "Deveria retornar 1 item");
            Verificar(all[0].Id == 1, "ID deveria ser 1");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste2_GetById_Existing_ShouldReturnEntity()
        {
            Console.WriteLine("Teste 2: GetById_Existing_ShouldReturnEntity");
            
            var repo = CriarRepo();
            repo.Add(new Book(1, "Livro A", "Autor"));
            var found = repo.GetById(1);
            
            Verificar(found != null, "Deveria encontrar o livro");
            Verificar(found.Title == "Livro A", "Título deveria ser 'Livro A'");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste3_GetById_Missing_ShouldReturnNull()
        {
            Console.WriteLine("Teste 3: GetById_Missing_ShouldReturnNull");
            
            var repo = CriarRepo();
            var found = repo.GetById(99);
            
            Verificar(found == null, "Não deveria encontrar o livro");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste4_Update_Existing_ShouldReturnTrue()
        {
            Console.WriteLine("Teste 4: Update_Existing_ShouldReturnTrue");
            
            var repo = CriarRepo();
            repo.Add(new Book(1, "Livro A", "Autor"));
            var updated = repo.Update(new Book(1, "Livro A (Revisto)", "Autor"));
            
            Verificar(updated, "Update deveria retornar true");
            Verificar(repo.GetById(1).Title == "Livro A (Revisto)", "Título deveria estar atualizado");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste5_Update_Missing_ShouldReturnFalse()
        {
            Console.WriteLine("Teste 5: Update_Missing_ShouldReturnFalse");
            
            var repo = CriarRepo();
            var updated = repo.Update(new Book(1, "Livro A", "Autor"));
            
            Verificar(!updated, "Update deveria retornar false");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste6_Remove_Existing_ShouldReturnTrue()
        {
            Console.WriteLine("Teste 6: Remove_Existing_ShouldReturnTrue");
            
            var repo = CriarRepo();
            repo.Add(new Book(1, "Livro A", "Autor"));
            var removed = repo.Remove(1);
            
            Verificar(removed, "Remove deveria retornar true");
            Verificar(repo.ListAll().Count == 0, "Lista deveria estar vazia");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste7_Remove_Missing_ShouldReturnFalse()
        {
            Console.WriteLine("Teste 7: Remove_Missing_ShouldReturnFalse");
            
            var repo = CriarRepo();
            var removed = repo.Remove(99);
            
            Verificar(!removed, "Remove deveria retornar false");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste8_MultipleOperations_ShouldWork()
        {
            Console.WriteLine("Teste 8: MultipleOperations_ShouldWork");
            
            var repo = CriarRepo();
            
            // Adicionar múltiplos itens
            repo.Add(new Book(1, "Livro 1", "Autor 1"));
            repo.Add(new Book(2, "Livro 2", "Autor 2"));
            repo.Add(new Book(3, "Livro 3", "Autor 3"));
            
            Verificar(repo.ListAll().Count == 3, "Deveria ter 3 livros");
            
            // Atualizar um
            repo.Update(new Book(2, "Livro 2 (Atualizado)", "Autor 2"));
            Verificar(repo.GetById(2).Title == "Livro 2 (Atualizado)", "Livro 2 deveria estar atualizado");
            
            // Remover um
            repo.Remove(1);
            Verificar(repo.ListAll().Count == 2, "Deveria ter 2 livros");
            Verificar(repo.GetById(1) == null, "Livro 1 não deveria existir mais");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Verificar(bool condicao, string mensagem)
        {
            if (!condicao)
            {
                throw new Exception($"Falha na verificação: {mensagem}");
            }
        }
    }
}

