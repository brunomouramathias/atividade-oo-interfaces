# Fase 6 — Repository CSV

## Objetivo

Evoluir o Repository da Fase 5 para **persistir em arquivo CSV**, mantendo o mesmo contrato `IRepository<T, TId>` mas agora com os dados sobrevivendo em disco.

**Ideia-chave:** Os dados persistem em arquivo `.csv` com cabeçalho, encoding UTF-8 e escape correto de vírgulas e aspas.

---

## Estrutura Organizada do Projeto

```
fase-06-repository-csv/
├── Domain/
│   └── Book.cs                    # Modelo de domínio
├── Repository/
│   ├── IRepository.cs             # Interface genérica
│   └── CsvBookRepository.cs       # Implementação CSV
├── Services/
│   └── BookService.cs             # Serviço de domínio
├── Tests/
│   └── TestesRepositorioCsv.cs    # Testes de integração
├── Program.cs                      # Demonstração
├── Fase06RepositoryCsv.csproj     # Projeto
├── .gitignore                      # Ignora arquivos CSV
└── REPOSITORY-CSV.md               # Esta documentação
```

---

## Arquitetura

```
BookService (Cliente)
    ↓
IRepository<Book, int> (Contrato - REUTILIZADO)
    ↓
CsvBookRepository (Implementação CSV)
    ↓
books.csv (Arquivo em Disco)
```

---

## 1. Formato CSV

### Estrutura do Arquivo

```csv
Id,Title,Author
1,Código Limpo,Robert C. Martin
2,Domain-Driven Design,Eric Evans
10,"Livro, com vírgula","Autor ""com aspas"""
```

### Regras de Escape

1. **Vírgulas:** Campo com vírgula é colocado entre aspas
2. **Aspas:** Aspas duplas são escapadas como `""`
3. **Quebras de linha:** Campo com `\n` ou `\r` é colocado entre aspas
4. **Encoding:** UTF-8 para suportar acentuação

---

## 2. Implementação CsvBookRepository

### Load() - Carrega do arquivo

```csharp
private List<Book> Load()
{
    if (!File.Exists(_path))
        return new List<Book>();
    
    var lines = File.ReadAllLines(_path, Encoding.UTF8);
    if (lines.Length == 0)
        return new List<Book>();
    
    var list = new List<Book>();
    var startIndex = lines[0].StartsWith("Id,") ? 1 : 0; // Pula cabeçalho
    
    for (int i = startIndex; i < lines.Length; i++)
    {
        var cols = SplitCsvLine(lines[i]);
        if (cols.Count >= 3 && int.TryParse(cols[0], out var id))
        {
            list.Add(new Book(id, cols[1], cols[2]));
        }
    }
    
    return list;
}
```

### Save() - Salva no arquivo

```csharp
private void Save(List<Book> books)
{
    var sb = new StringBuilder();
    sb.AppendLine("Id,Title,Author"); // Cabeçalho
    
    foreach (var book in books.OrderBy(b => b.Id))
    {
        var id = book.Id.ToString();
        var title = Escape(book.Title);
        var author = Escape(book.Author);
        sb.Append(id).Append(',').Append(title).Append(',').Append(author).AppendLine();
    }
    
    File.WriteAllText(_path, sb.ToString(), Encoding.UTF8);
}
```

### Escape() - Escapa caracteres especiais

```csharp
private static string Escape(string value)
{
    if (value == null) return string.Empty;
    
    var needsQuotes = value.Contains(',') ||
                      value.Contains('"') ||
                      value.Contains('\n') ||
                      value.Contains('\r');
    
    var escaped = value.Replace("\"", "\"\""); // " → ""
    return needsQuotes ? $"\"{escaped}\"" : escaped;
}
```

### SplitCsvLine() - Parse linha CSV

```csharp
private static List<string> SplitCsvLine(string line)
{
    var result = new List<string>();
    var current = new StringBuilder();
    var inQuotes = false;
    
    for (int i = 0; i < line.Length; i++)
    {
        var c = line[i];
        
        if (inQuotes)
        {
            if (c == '"')
            {
                // Aspas escapada?
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++; // Consome segunda aspas
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
```

---

## 3. Política de ID

**Decisão adotada:** O ID vem do domínio (fornecido externamente)

- Se ID já existe: **sobrescreve** (comportamento do `Add`)
- Para inserir novo: usar ID único
- Para atualizar: usar `Update` (verifica existência)

---

## Como executar

```bash
cd src/fase-06-repository-csv
dotnet run
```

O programa executa:
1. **10 testes automatizados** com arquivos temporários
2. **6 cenários de uso** com persistência real
3. Mostra o conteúdo do arquivo CSV gerado

---

## Testes Implementados (10 testes)

### Testes de Operações Básicas

1. ✅ **ListAll_WhenFileDoesNotExist_ShouldReturnEmpty**
   - Arquivo ausente retorna lista vazia

2. ✅ **Add_Then_ListAll_ShouldPersistInFile**
   - Dados persistem após Add

3. ✅ **GetById_Existing_ShouldReturnBook**
   - Busca por ID existente funciona

4. ✅ **GetById_Missing_ShouldReturnNull**
   - Busca por ID ausente retorna null

5. ✅ **Update_Existing_ShouldPersistChanges**
   - Update persiste mudanças

6. ✅ **Update_Missing_ShouldReturnFalse**
   - Update de inexistente retorna false

7. ✅ **Remove_Existing_ShouldDeleteFromFile**
   - Remove apaga do arquivo

8. ✅ **Remove_Missing_ShouldReturnFalse**
   - Remove de inexistente retorna false

### Testes de Casos Especiais

9. ✅ **Add_WithCommaAndQuotes_ShouldEscapeCorrectly**
   - Vírgulas e aspas são escapadas corretamente

10. ✅ **MultipleOperations_ShouldPersist**
    - Múltiplas operações persistem corretamente

---

## Cenários Demonstrados

### Cenário 1: Cadastro de Livros
Cadastra 3 livros e persiste em CSV

### Cenário 2: Listagem
Lista todos os livros do arquivo

### Cenário 3: Campos Especiais
Cadastra livro com vírgulas e aspas no título/autor

### Cenário 4: Atualização
Atualiza título de um livro existente

### Cenário 5: Remoção
Remove um livro e verifica persistência

### Cenário 6: Persistência
Demonstra que dados sobrevivem entre execuções

---

## Diferenças: InMemory vs CSV

| Aspecto | Fase 5 (InMemory) | Fase 6 (CSV) |
|---------|-------------------|--------------|
| Armazenamento | Dictionary em RAM | Arquivo .csv em disco |
| Persistência | ❌ Perdida ao fechar | ✅ Sobrevive entre execuções |
| Performance | Muito rápida | Mais lenta (I/O) |
| Encoding | N/A | UTF-8 |
| Escape | N/A | Vírgulas e aspas |
| Cabeçalho | N/A | Id,Title,Author |
| Testes | Sem I/O | Com arquivo temporário |

---

## Vantagens da Implementação CSV

### 1. Persistência Real
- Dados não são perdidos ao fechar o programa
- Arquivo pode ser aberto no Excel/LibreOffice

### 2. Portabilidade
- Formato texto simples
- Fácil importar/exportar

### 3. Legibilidade
- Humano pode ler e editar
- Útil para depuração

### 4. Compatibilidade
- Padrão universal
- Integra com outras ferramentas

---

## Limitações Conhecidas

⚠️ **Esta implementação é didática e tem limitações:**

1. **Sem tratamento de concorrência**
   - Múltiplos processos podem corromper arquivo

2. **Performance em listas grandes**
   - Carrega/salva arquivo inteiro a cada operação

3. **Sem versionamento**
   - Mudança no esquema quebra arquivos antigos

4. **Sem transações**
   - Falha no meio da gravação corrompe arquivo

**Em projetos reais:** Usar bibliotecas especializadas ou banco de dados.

---

## Próximas Evoluções

A arquitetura Repository está pronta para:
- **Fase 7:** Repository com JSON
- **Fase 8:** Repository com Banco de Dados

**Facilidade:** Basta criar nova implementação de `IRepository<T, TId>`, o cliente não muda!

---

## Princípios Aplicados

### Open/Closed Principle
- Aberto para novas implementações (JSON, BD)
- Cliente fechado (não muda ao trocar implementação)

### Dependency Inversion
- Cliente depende de `IRepository<T, TId>`
- Não conhece `CsvBookRepository`

### Single Responsibility
- Repository: persistência
- Service: regras de negócio
- Tests: validação

---

## Organização em Pastas

✅ **Vantagens da estrutura organizada:**

- **Domain/** - Modelos de domínio isolados
- **Repository/** - Lógica de persistência separada
- **Services/** - Regras de negócio organizadas
- **Tests/** - Testes separados do código principal

**Resultado:** Código mais limpo e fácil de navegar no GitHub!

