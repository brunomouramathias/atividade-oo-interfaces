# Fase 8 — ISP (Interface Segregation Principle)

## Objetivo

Aplicar o **ISP (Interface Segregation Principle)** segregando a interface `IRepository<T, TId>` em interfaces menores e mais coesas, permitindo que clientes dependam apenas dos métodos que realmente precisam.

**Ideia-chave:** Clientes não devem ser forçados a depender de interfaces que não usam.

---

## Estrutura Organizada do Projeto

```
fase-08-isp/
├── Domain/
│   ├── Interfaces/              # Interfaces segregadas
│   │   ├── IReadRepository.cs   # Somente leitura
│   │   ├── IWriteRepository.cs  # Somente escrita
│   │   └── IRepository.cs       # Completa (herda ambas)
│   ├── Entities/                # Modelos de domínio
│   │   └── Book.cs
│   └── Repositories/            # Implementações
│       └── InMemoryBookRepository.cs
├── Services/                    # Serviços de aplicação
│   ├── ReadOnlyBookService.cs   # Usa IReadRepository
│   └── BookService.cs           # Usa IRepository
├── Tests/                       # Testes separados
│   └── TestesIsp.cs
├── Program.cs                   # Demonstração
└── ISP.md                       # Esta documentação
```

**Organização:** Interfaces no Domain, Repositórios no Domain, testes separados!

---

## O Problema sem ISP

Antes, tínhamos uma única interface:

```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);              // Escrita
    T? GetById(TId id);           // Leitura
    IReadOnlyList<T> ListAll();   // Leitura
    bool Update(T entity);        // Escrita
    bool Remove(TId id);          // Escrita
}
```

**Problema:** Um serviço que precisa apenas de **leitura** é forçado a receber uma interface com métodos de **escrita**.

---

## A Solução com ISP

### 1. IReadRepository<T, TId>

Apenas operações de **leitura**:

```csharp
public interface IReadRepository<T, TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
}
```

### 2. IWriteRepository<T, TId>

Apenas operações de **escrita**:

```csharp
public interface IWriteRepository<T, TId>
{
    T Add(T entity);
    bool Update(T entity);
    bool Remove(TId id);
}
```

### 3. IRepository<T, TId>

Interface completa (para compatibilidade):

```csharp
public interface IRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
{
    // Não precisa adicionar métodos, herda de ambas
}
```

---

## Implementação

### InMemoryBookRepository

Implementa `IRepository<Book, int>` (completa):

```csharp
public sealed class InMemoryBookRepository : IRepository<Book, int>
{
    private readonly Dictionary<int, Book> _store = new Dictionary<int, Book>();

    // Implementa IWriteRepository
    public Book Add(Book entity) { ... }
    public bool Update(Book entity) { ... }
    public bool Remove(int id) { ... }

    // Implementa IReadRepository
    public Book? GetById(int id) { ... }
    public IReadOnlyList<Book> ListAll() { ... }
}
```

---

## Serviços Demonstrando ISP

### ReadOnlyBookService

Depende **apenas** de `IReadRepository`:

```csharp
public sealed class ReadOnlyBookService
{
    private readonly IReadRepository<Book, int> _readRepository;

    public ReadOnlyBookService(IReadRepository<Book, int> readRepository)
    {
        _readRepository = readRepository;
    }

    public Book? FindById(int id) => _readRepository.GetById(id);
    public IReadOnlyList<Book> ListAll() => _readRepository.ListAll();
    public int CountBooks() => _readRepository.ListAll().Count;
    public bool BookExists(int id) => _readRepository.GetById(id) != null;
}
```

**Benefício:** Compilador garante que este serviço **não pode modificar dados**.

### BookService

Depende de `IRepository` (completa):

```csharp
public sealed class BookService
{
    private readonly IRepository<Book, int> _repository;

    public BookService(IRepository<Book, int> repository)
    {
        _repository = repository;
    }

    public Book Register(Book book) => _repository.Add(book);
    public bool UpdateBook(Book book) => _repository.Update(book);
    public bool RemoveBook(int id) => _repository.Remove(id);
    public Book? FindById(int id) => _repository.GetById(id);
    public IReadOnlyList<Book> ListAll() => _repository.ListAll();
}
```

**Benefício:** Tem acesso completo quando necessário.

---

## Como executar

```bash
cd src/fase-08-isp
dotnet run
```

---

## Testes Implementados (5 testes)

1. ✅ **ReadOnlyService_ShouldOnlyNeedReadRepository**
   - ReadOnlyService funciona apenas com IReadRepository

2. ✅ **FullService_ShouldNeedFullRepository**
   - BookService tem acesso completo (leitura + escrita)

3. ✅ **ReadOnlyService_CannotModifyData**
   - ReadOnlyService não pode modificar (garantido em compilação)

4. ✅ **ISP_AllowsDifferentImplementations**
   - Mesma implementação serve para diferentes interfaces

5. ✅ **ReadOnlyService_CountsCorrectly**
   - Operações de leitura funcionam corretamente

---

## Cenários Demonstrados

### Cenário 1: Serviço Completo
BookService usa IRepository (leitura + escrita)

### Cenário 2: Serviço Somente Leitura
ReadOnlyService usa IReadRepository (apenas leitura)

### Cenário 3: Benefícios do ISP
- Interfaces menores e coesas
- Clientes dependem apenas do necessário
- Compilador garante restrições

### Cenário 4: Estrutura de Interfaces
Visualização da hierarquia de interfaces

### Cenário 5: Demonstração de Segregação
Mesmo repositório, interfaces diferentes

---

## Benefícios do ISP

### 1. Menor Acoplamento
- Clientes dependem apenas do que precisam
- Reduz dependências desnecessárias

### 2. Segurança em Compilação
- ReadOnlyService **não pode** chamar métodos de escrita
- Erro em tempo de compilação, não em runtime

### 3. Interfaces Coesas
- Cada interface tem responsabilidade bem definida
- IReadRepository: apenas leitura
- IWriteRepository: apenas escrita

### 4. Facilita Testes
- Pode mockar apenas IReadRepository para serviços de leitura
- Mocks menores e mais simples

### 5. Flexibilidade
- Mesma implementação pode ser usada de formas diferentes
- Permite implementações especializadas (ex: ReadOnlyCachedRepository)

---

## Violações do ISP (o que evitamos)

❌ **Forçar clientes a depender de métodos não usados**
```csharp
// ANTES (violação do ISP)
public class ReportService
{
    private readonly IRepository<Book, int> _repo; // Tem Add, Update, Remove
    // Mas ReportService só precisa de GetById e ListAll!
}
```

✅ **Com ISP aplicado**
```csharp
// DEPOIS (ISP aplicado)
public class ReportService
{
    private readonly IReadRepository<Book, int> _repo; // Só GetById e ListAll
}
```

---

## Comparação: Antes vs Depois

| Aspecto | Sem ISP | Com ISP |
|---------|---------|---------|
| Interface única | IRepository (5 métodos) | 3 interfaces segregadas |
| ReadOnlyService | Depende de 5 métodos | Depende de 2 métodos |
| Segurança | Runtime | Compile-time |
| Acoplamento | Alto | Baixo |
| Coesão | Média | Alta |
| Testabilidade | Mocks grandes | Mocks pequenos |

---

## Princípios SOLID Aplicados

### S - Single Responsibility
- Cada interface tem uma responsabilidade

### O - Open/Closed
- Aberto para novas implementações

### L - Liskov Substitution
- IRepository pode substituir IReadRepository

### **I - Interface Segregation** ⭐
- **Interfaces segregadas por responsabilidade**
- **Clientes dependem apenas do necessário**

### D - Dependency Inversion
- Serviços dependem de abstrações

---

## Hierarquia de Interfaces

```
IReadRepository<T, TId>
  ├── GetById(TId id)
  └── ListAll()

IWriteRepository<T, TId>
  ├── Add(T entity)
  ├── Update(T entity)
  └── Remove(TId id)

IRepository<T, TId>
  └── (herda IReadRepository + IWriteRepository)
```

---

## Preparação para Próximas Fases

A arquitetura com ISP está pronta para:
- Fase 9: Dublês Avançados e Testes Assíncronos
- Implementações especializadas (ex: CachedReadRepository)
- Diferentes back-ends (SQL, NoSQL, Cache)

**Facilidade:** Interfaces segregadas facilitam mocks e testes!

---

## Checklist de Qualidade

- [x] IReadRepository com apenas operações de leitura
- [x] IWriteRepository com apenas operações de escrita
- [x] IRepository herdando de ambas
- [x] ReadOnlyBookService usando IReadRepository
- [x] BookService usando IRepository completo
- [x] InMemoryBookRepository implementando IRepository
- [x] 5 testes demonstrando ISP
- [x] Testes em arquivo separado (Tests/TestesIsp.cs)
- [x] Código organizado (Domain/Interfaces, Domain/Entities, Domain/Repositories)
- [x] Repositório dentro do Domain (conforme solicitado)
- [x] Documentação completa em ISP.md

