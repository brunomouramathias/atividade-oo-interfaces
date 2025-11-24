using System;
using System.IO;
using Fase07RepositoryJson.Domain;
using Fase07RepositoryJson.Repository;

namespace Fase07RepositoryJson.Tests
{
    // Testes de integração para JSON Repository
    public static class TestesRepositorioJson
    {
        public static void ExecutarTodos()
        {
            Console.WriteLine("=== TESTES DO REPOSITORY JSON ===\n");

            Teste1_ListAll_WhenFileDoesNotExist_ShouldReturnEmpty();
            Teste2_Add_Then_ListAll_ShouldPersistInFile();
            Teste3_Add_WithSpecialCharacters_ShouldHandleCorrectly();
            Teste4_GetById_Existing_ShouldReturnBook();
            Teste5_GetById_Missing_ShouldReturnNull();
            Teste6_Update_Existing_ShouldPersistChanges();
            Teste7_Update_Missing_ShouldReturnFalse();
            Teste8_Remove_Existing_ShouldDeleteFromFile();
            Teste9_Remove_Missing_ShouldReturnFalse();
            Teste10_MultipleOperations_ShouldPersist();

            Console.WriteLine("\n=== TODOS OS TESTES PASSARAM! ===\n");
        }

        private static string CreateTempPath()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
            return path;
        }

        private static JsonBookRepository CreateRepo(string path)
        {
            return new JsonBookRepository(path);
        }

        private static void Teste1_ListAll_WhenFileDoesNotExist_ShouldReturnEmpty()
        {
            Console.WriteLine("Teste 1: ListAll_WhenFileDoesNotExist_ShouldReturnEmpty");
            
            var path = CreateTempPath();
            var repo = CreateRepo(path);
            var all = repo.ListAll();
            
            Verificar(all.Count == 0, "Lista deveria estar vazia");
            
            Console.WriteLine("  ✓ Passou\n");
        }

        private static void Teste2_Add_Then_ListAll_ShouldPersistInFile()
        {
            Console.WriteLine("Teste 2: Add_Then_ListAll_ShouldPersistInFile");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                repo.Add(new Book(1, "Código Limpo", "Robert C. Martin"));
                
                var all = repo.ListAll();
                Verificar(all.Count == 1, "Deveria ter 1 livro");
                Verificar(all[0].Id == 1, "ID deveria ser 1");
                Verificar(all[0].Title == "Código Limpo", "Título incorreto");
                
                // Verificar que arquivo JSON foi criado
                Verificar(File.Exists(path), "Arquivo JSON deveria existir");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste3_Add_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            Console.WriteLine("Teste 3: Add_WithSpecialCharacters_ShouldHandleCorrectly");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                repo.Add(new Book(1, "Livro com \"aspas\" e /barras/", "Autor com: símbolos, especiais!"));
                
                var all = repo.ListAll();
                Verificar(all.Count == 1, "Deveria ter 1 livro");
                Verificar(all[0].Title.Contains("\""), "Título deveria conter aspas");
                Verificar(all[0].Author.Contains(":"), "Autor deveria conter dois pontos");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste4_GetById_Existing_ShouldReturnBook()
        {
            Console.WriteLine("Teste 4: GetById_Existing_ShouldReturnBook");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                repo.Add(new Book(1, "Livro A", "Autor"));
                
                var found = repo.GetById(1);
                Verificar(found != null, "Deveria encontrar o livro");
                Verificar(found.Title == "Livro A", "Título incorreto");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste5_GetById_Missing_ShouldReturnNull()
        {
            Console.WriteLine("Teste 5: GetById_Missing_ShouldReturnNull");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                var found = repo.GetById(99);
                
                Verificar(found == null, "Não deveria encontrar o livro");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste6_Update_Existing_ShouldPersistChanges()
        {
            Console.WriteLine("Teste 6: Update_Existing_ShouldPersistChanges");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                repo.Add(new Book(1, "Livro A", "Autor"));
                
                var updated = repo.Update(new Book(1, "Livro A (Revisto)", "Autor"));
                Verificar(updated, "Update deveria retornar true");
                Verificar(repo.GetById(1).Title == "Livro A (Revisto)", "Título deveria estar atualizado");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste7_Update_Missing_ShouldReturnFalse()
        {
            Console.WriteLine("Teste 7: Update_Missing_ShouldReturnFalse");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                var updated = repo.Update(new Book(1, "Livro A", "Autor"));
                
                Verificar(!updated, "Update deveria retornar false");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste8_Remove_Existing_ShouldDeleteFromFile()
        {
            Console.WriteLine("Teste 8: Remove_Existing_ShouldDeleteFromFile");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                repo.Add(new Book(1, "Livro A", "Autor"));
                
                var removed = repo.Remove(1);
                Verificar(removed, "Remove deveria retornar true");
                Verificar(repo.ListAll().Count == 0, "Lista deveria estar vazia");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste9_Remove_Missing_ShouldReturnFalse()
        {
            Console.WriteLine("Teste 9: Remove_Missing_ShouldReturnFalse");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                var removed = repo.Remove(99);
                
                Verificar(!removed, "Remove deveria retornar false");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        private static void Teste10_MultipleOperations_ShouldPersist()
        {
            Console.WriteLine("Teste 10: MultipleOperations_ShouldPersist");
            
            var path = CreateTempPath();
            try
            {
                var repo = CreateRepo(path);
                
                // Adicionar múltiplos
                repo.Add(new Book(1, "Livro 1", "Autor 1"));
                repo.Add(new Book(2, "Livro 2", "Autor 2"));
                repo.Add(new Book(3, "Livro 3", "Autor 3"));
                
                Verificar(repo.ListAll().Count == 3, "Deveria ter 3 livros");
                
                // Atualizar
                repo.Update(new Book(2, "Livro 2 (Atualizado)", "Autor 2"));
                Verificar(repo.GetById(2).Title == "Livro 2 (Atualizado)", "Livro 2 deveria estar atualizado");
                
                // Remover
                repo.Remove(1);
                Verificar(repo.ListAll().Count == 2, "Deveria ter 2 livros");
                Verificar(repo.GetById(1) == null, "Livro 1 não deveria existir");
                
                Console.WriteLine("  ✓ Passou\n");
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
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

