using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fase06RepositoryCsv.Domain;

namespace Fase06RepositoryCsv.Repository
{
    // Implementação CSV do Repository
    public sealed class CsvBookRepository : IRepository<Book, int>
    {
        private readonly string _path;

        public CsvBookRepository(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path inválido", nameof(path));
            _path = path;
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
            
            var lines = File.ReadAllLines(_path, Encoding.UTF8);
            if (lines.Length == 0)
                return new List<Book>();
            
            var list = new List<Book>();
            var startIndex = 0;
            
            // Pula cabeçalho se existir
            if (lines[0].StartsWith("Id,"))
                startIndex = 1;
            
            for (int i = startIndex; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;
                
                var cols = SplitCsvLine(line);
                if (cols.Count < 3)
                    continue; // Ignora linha quebrada
                
                if (!int.TryParse(cols[0], out var id))
                    continue;
                
                var title = cols[1];
                var author = cols[2];
                list.Add(new Book(id, title, author));
            }
            
            return list;
        }

        private void Save(List<Book> books)
        {
            var sb = new StringBuilder();
            
            // Cabeçalho
            sb.AppendLine("Id,Title,Author");
            
            foreach (var book in books.OrderBy(b => b.Id))
            {
                var id = book.Id.ToString();
                var title = Escape(book.Title);
                var author = Escape(book.Author);
                sb.Append(id).Append(',').Append(title).Append(',').Append(author).AppendLine();
            }
            
            File.WriteAllText(_path, sb.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            if (value == null)
                return string.Empty;
            
            var needsQuotes = value.Contains(',') ||
                            value.Contains('"') ||
                            value.Contains('\n') ||
                            value.Contains('\r');
            
            var escaped = value.Replace("\"", "\"\"");
            return needsQuotes ? $"\"{escaped}\"" : escaped;
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(line))
            {
                result.Add(string.Empty);
                return result;
            }
            
            var current = new StringBuilder();
            var inQuotes = false;
            
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        // Possível aspas escapada
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            current.Append('"');
                            i++; // Consome a segunda aspas
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }
            
            result.Add(current.ToString());
            return result;
        }
    }
}

