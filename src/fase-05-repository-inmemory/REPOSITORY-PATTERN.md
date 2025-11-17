# Fase 5 — Repository InMemory

## Objetivo

Introduzir o padrão **Repository** como ponto único de acesso a dados, usando um domínio simples (Book) com implementação InMemory baseada em coleção.

**Ideia-chave:** O cliente fala só com o Repository, nunca com coleções diretamente.

---

## Arquitetura

```
Cliente (BookService)
    ↓
IRepository<T, TId> (Contrato)
    ↓
InMemoryRepository<T, TId> (Implementação)
    ↓
Dictionary<TId, T> (Coleção InMemory)
```

---

## 1. Contrato do Repository

```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
```

**Características:**
- **Genérico:** Funciona com qualquer tipo `T` e identificador `TId`
- **Coeso:** Expõe só operações de acesso a dados
- **Sem regra de negócio:** Apenas CRUD básico
- **IReadOnlyList:** Não expõe coleções mutáveis

---

## 2. Implementação InMemory

```csharp
public sealed class InMemoryRepository<T, TId> : IRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new Dictionary<TId, T>();
    private readonly Func<T, TId> _getId;

    public InMemoryRepository(Func<T, TId> getId)
    {
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }

    public T Add(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return entity;
    }

    public T? GetById(TId id)
    {
        return _store.TryGetValue(id, out var entity) ? entity : default;
    }

    public IReadOnlyList<T> ListAll()
    {
        return _store.Values.ToList();
    }

    public bool Update(T entity)
    {
        var id = _getId(entity);
        if (!_store.ContainsKey(id))
            return false;
        
        _store[id] = entity;
        return true;
    }

    public bool Remove(TId id)
    {
        return _store.Remove(id);
    }
}
```

**Decisões de design:**
- **Dictionary:** Armazenamento em memória por chave-valor
- **Func<T, TId>:** Extrator de ID configurável
- **Sealed:** Não pode ser herdada
- **Sem I/O:** Totalmente em memória

---

## 3. Modelo de Domínio

```csharp
public sealed record Book(int Id, string Title, string Author);
```

**Características:**
- **Record:** Imutável por padrão
- **Sealed:** Não pode ser herdado
- **Simples:** Apenas dados essenciais

---

## 4. Serviço de Domínio

```csharp
public static class BookService
{
    public static Book Register(IRepository<Book, int> repo, Book book)
    {
        // Validações de domínio
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Título não pode ser vazio");
        
        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Autor não pode ser vazio");
        
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
        // Validações antes de atualizar
        return repo.Update(book);
    }

    public static bool RemoveBook(IRepository<Book, int> repo, int id)
    {
        return repo.Remove(id);
    }
}
```

**Responsabilidades:**
- **Validações de domínio:** Regras de negócio
- **Orquestração:** Coordena operações com o Repository
- **Não acessa coleções:** Fala só com o Repository

---

## 5. Composição (Ponto Único)

```csharp
// Criação do Repository para Book
IRepository<Book, int> repo = new InMemoryRepository<Book, int>(book => book.Id);

// Uso através do serviço
BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
var all = BookService.ListAll(repo);
```

**Características:**
- Repository criado em um único ponto
- Política de ID definida: `book => book.Id`
- Cliente usa apenas a interface

---

## Como executar

```bash
cd src/fase-05-repository-inmemory
dotnet run
```

O programa executa:
1. **8 testes automatizados** do Repository
2. **6 cenários de uso** com o BookService

---

## Testes Implementados

### 1. Add_Then_ListAll_ShouldReturnOneItem
Verifica que após adicionar um item, a listagem retorna exatamente esse item.

### 2. GetById_Existing_ShouldReturnEntity
Verifica que GetById retorna a entidade quando o ID existe.

### 3. GetById_Missing_ShouldReturnNull
Verifica que GetById retorna null quando o ID não existe.

### 4. Update_Existing_ShouldReturnTrue
Verifica que Update retorna true e atualiza quando o ID existe.

### 5. Update_Missing_ShouldReturnFalse
Verifica que Update retorna false quando o ID não existe.

### 6. Remove_Existing_ShouldReturnTrue
Verifica que Remove retorna true e remove quando o ID existe.

### 7. Remove_Missing_ShouldReturnFalse
Verifica que Remove retorna false quando o ID não existe.

### 8. MultipleOperations_ShouldWork
Verifica que múltiplas operações funcionam corretamente em sequência.

---

## Cenários de Uso Demonstrados

### Cenário 1: Cadastro de Livros
Cadastra 3 livros no repositório.

### Cenário 2: Listagem
Lista todos os livros cadastrados.

### Cenário 3: Busca por ID
Busca um livro existente e tenta buscar um inexistente.

### Cenário 4: Atualização
Atualiza o título de um livro existente.

### Cenário 5: Remoção
Remove um livro e verifica a contagem.

### Cenário 6: Operações em Lote
Cadastra vários livros de uma vez e lista todos.

---

## Benefícios do Padrão Repository

### 1. Abstração de Persistência
- Cliente não sabe como os dados são armazenados
- Fácil trocar InMemory por CSV, JSON, BD

### 2. Testabilidade
- Repository pode ser facilmente mockado
- Testes rápidos sem I/O real

### 3. Centralização
- Único ponto de acesso a dados
- Lógica de persistência isolada

### 4. Flexibilidade
- Trocar implementação sem alterar cliente
- Adicionar cache, logging, etc.

---

## Política de ID

**Decisão adotada:** O ID vem de fora (domínio fornece)

```csharp
new InMemoryRepository<Book, int>(book => book.Id)
```

**Alternativas:**
- Repository gera ID sequencial
- Repository gera GUID
- ID composto (múltiplos campos)

---

## Pitfalls Evitados

### ❌ Não fazer:
- Colocar regra de negócio no Repository
- Expor `List<T>` mutável (usar `IReadOnlyList<T>`)
- Cliente acessar coleções diretamente
- Misturar Repository com lógica de apresentação

### ✅ Fazer:
- Repository focado em acesso a dados
- Retornar coleções imutáveis
- Cliente falar só com o Repository
- Separar domínio, repository e apresentação

---

## Próximas Evoluções

A arquitetura está preparada para:
- **Fase 6:** Repository com persistência em CSV
- **Fase 7:** Repository com persistência em JSON
- **Fase 8:** Repository com banco de dados

**Facilidade:** Apenas trocar a implementação do `IRepository`, o cliente não muda!

---

## Princípios Aplicados

### Dependency Inversion Principle (DIP)
Cliente depende de `IRepository<T, TId>`, não de `InMemoryRepository<T, TId>`

### Single Responsibility Principle (SRP)
- Repository: acesso a dados
- Service: regras de negócio
- Model: estrutura de dados

### Interface Segregation Principle (ISP)
Interface coesa com apenas as operações essenciais

### Open/Closed Principle (OCP)
Aberto para novas implementações (CSV, JSON), fechado para modificações no cliente

