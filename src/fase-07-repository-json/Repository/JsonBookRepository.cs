using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Fase07RepositoryJson.Domain;
using Fase07RepositoryJson.Domain.Interfaces;

namespace Fase07RepositoryJson.Repository
{
    /// <summary>
    /// Implementação JSON do Repository usando System.Text.Json.
    /// </summary>
    public sealed class JsonBookRepository : IRepository<Book, int>
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options;

        public JsonBookRepository(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path inválido", nameof(path));
            
            _path = path;
            
            // Configuração para JSON legível (indentado)
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public Book Add(Book entity)
        {
            var list = Load();
            // Política: se já existe Id, substitui; caso contrário, adiciona
            list.RemoveAll(b => b.Id == entity.Id);
            list.Add(entity);
            Save(list);
            return entity;
        }

        public Book? GetById(int id)
        {
            return Load().FirstOrDefault(b => b.Id == id);
        }

        public IReadOnlyList<Book> ListAll()
        {
            return Load();
        }

        public bool Update(Book entity)
        {
            var list = Load();
            var index = list.FindIndex(b => b.Id == entity.Id);
            if (index < 0)
                return false;
            
            list[index] = entity;
            Save(list);
            return true;
        }

        public bool Remove(int id)
        {
            var list = Load();
            var removed = list.RemoveAll(b => b.Id == id) > 0;
            if (removed)
            {
                Save(list);
            }
            return removed;
        }

        // ------------ Helpers Privados ------------

        private List<Book> Load()
        {
            if (!File.Exists(_path))
                return new List<Book>();
            
            try
            {
                var json = File.ReadAllText(_path);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<Book>();
                
                var books = JsonSerializer.Deserialize<List<Book>>(json, _options);
                return books ?? new List<Book>();
            }
            catch
            {
                // Se falhar ao deserializar, retorna lista vazia
                return new List<Book>();
            }
        }

        private void Save(List<Book> books)
        {
            // Ordena por ID antes de salvar
            var sortedBooks = books.OrderBy(b => b.Id).ToList();
            var json = JsonSerializer.Serialize(sortedBooks, _options);
            File.WriteAllText(_path, json);
        }
    }
}


