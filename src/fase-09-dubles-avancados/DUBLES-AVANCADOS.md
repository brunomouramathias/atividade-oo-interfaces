# Fase 9 — Dublês Avançados e Testes Assíncronos

## Objetivo

Introduzir **dublês avançados** (Dummy, Stub, Spy, Mock, Fake) e implementar **testes assíncronos** usando `async/await` e `Task<T>`, demonstrando como testar código assíncrono com diferentes tipos de test doubles.

**Ideia-chave:** Diferentes tipos de dublês para diferentes propósitos de teste, com operações assíncronas simulando I/O real.

---

## Estrutura Organizada do Projeto

```
fase-09-dubles-avancados/
├── Domain/
│   ├── Entities/
│   │   └── Book.cs
│   ├── Interfaces/
│   │   └── IAsyncRepository.cs
│   └── Repositories/
│       └── AsyncBookRepository.cs
├── Services/
│   └── AsyncBookService.cs
├── Tests/
│   ├── Dubles/
│   │   ├── DummyRepository.cs
│   │   ├── StubRepository.cs
│   │   ├── SpyRepository.cs
│   │   ├── MockRepository.cs
│   │   └── FakeRepository.cs
│   └── TestesAssincronos.cs
├── Program.cs
└── DUBLES-AVANCADOS.md
```

**Organização:** Interfaces e Repositórios no Domain, Dublês em pasta separada, Testes assíncronos!

---

## Tipos de Dublês (Test Doubles)

### 1. 🎭 DUMMY

**O que é:** Objeto que não faz nada, apenas preenche parâmetros

**Quando usar:** Quando o teste não precisa da funcionalidade real

**Implementação:**
```csharp
public sealed class DummyRepository : IAsyncRepository<Book, int>
{
    public Task<Book> AddAsync(Book entity)
    {
        throw new NotImplementedException("Dummy: não deve ser chamado");
    }
    // ... todos os métodos lançam exceção
}
```

**Exemplo de uso:**
- Passar como parâmetro quando o método não será chamado
- Testes focados em outras partes do sistema

---

### 2. 📋 STUB

**O que é:** Retorna valores fixos pré-configurados

**Quando usar:** Para simular respostas específicas sem lógica complexa

**Implementação:**
```csharp
public sealed class StubRepository : IAsyncRepository<Book, int>
{
    private readonly List<Book> _fixedData;

    public StubRepository(List<Book> fixedData)
    {
        _fixedData = fixedData ?? new List<Book>();
    }

    public Task<IReadOnlyList<Book>> ListAllAsync()
    {
        return Task.FromResult<IReadOnlyList<Book>>(_fixedData);
    }
}
```

**Exemplo de uso:**
- Retornar lista fixa de livros
- Simular resposta de API externa
- Cenários determinísticos

---

### 3. 🕵️ SPY

**O que é:** Registra as chamadas feitas (espiona o comportamento)

**Quando usar:** Para verificar se métodos foram chamados e com quais parâmetros

**Implementação:**
```csharp
public sealed class SpyRepository : IAsyncRepository<Book, int>
{
    public int AddAsyncCallCount { get; private set; }
    public List<Book> AddedBooks { get; } = new List<Book>();
    public List<int> QueriedIds { get; } = new List<int>();

    public Task<Book> AddAsync(Book entity)
    {
        AddAsyncCallCount++;
        AddedBooks.Add(entity);
        return Task.FromResult(entity);
    }
}
```

**Exemplo de uso:**
- Verificar quantas vezes um método foi chamado
- Registrar parâmetros das chamadas
- Verificar ordem de execução

---

### 4. ✅ MOCK

**O que é:** Verifica se expectativas foram atendidas

**Quando usar:** Para garantir que contratos são respeitados

**Implementação:**
```csharp
public sealed class MockRepository : IAsyncRepository<Book, int>
{
    private int _expectedAddCalls;
    private int _actualAddCalls;

    public void ExpectAddAsync(Book book, int times = 1)
    {
        _expectedAddCalls = times;
    }

    public void Verify()
    {
        if (_actualAddCalls != _expectedAddCalls)
            throw new Exception("Expectativa falhou");
    }
}
```

**Exemplo de uso:**
- Verificar número exato de chamadas
- Garantir que método foi chamado com parâmetros específicos
- Testes de contrato

---

### 5. 🔧 FAKE

**O que é:** Implementação funcional simplificada

**Quando usar:** Para simular comportamento real de forma mais leve

**Implementação:**
```csharp
public sealed class FakeRepository : IAsyncRepository<Book, int>
{
    private readonly Dictionary<int, Book> _store = new Dictionary<int, Book>();
    private readonly int _delayMs;

    public async Task<Book> AddAsync(Book entity)
    {
        if (_delayMs > 0)
            await Task.Delay(_delayMs); // Simula latência
        
        _store[entity.Id] = entity;
        return entity;
    }
}
```

**Exemplo de uso:**
- Substituir banco de dados por dicionário em memória
- Simular latência de rede
- Testes de integração rápidos

---

## Interface Assíncrona

```csharp
public interface IAsyncRepository<T, TId>
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(TId id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<bool> UpdateAsync(T entity);
    Task<bool> RemoveAsync(TId id);
}
```

**Características:**
- Todos os métodos retornam `Task<T>`
- Permite operações assíncronas com `async/await`
- Simula operações I/O (banco de dados, APIs, arquivos)

---

## Como executar

```bash
cd src/fase-09-dubles-avancados
dotnet run
```

O programa executa:
1. **8 testes assíncronos** com diferentes dublês
2. **8 cenários demonstrativos** dos tipos de dublês
3. Comparação de performance entre Fake e Real

---

## Testes Implementados (8 testes)

### Testes com Dublês

1. ✅ **Stub_RetornaValoresFixos**
   - Stub retorna dados pré-configurados

2. ✅ **Spy_RegistraChamadas**
   - Spy registra todas as chamadas e parâmetros

3. ✅ **Mock_VerificaExpectativas**
   - Mock verifica se expectativas foram atendidas

4. ✅ **Fake_SimulaComportamentoReal**
   - Fake funciona como repositório real

### Testes Assíncronos

5. ✅ **Fake_ComLatencia**
   - Simula latência de I/O

6. ✅ **OperacoesParalelas_Funcionam**
   - Múltiplas operações em paralelo

7. ✅ **Task_WhenAll_ExecutaEmParalelo**
   - Task.WhenAll permite execução paralela

8. ✅ **Spy_VerificaOrdemDeChamadas**
   - Spy registra ordem correta

---

## Cenários Demonstrados

### Cenário 1: Tipos de Dublês
Explica cada tipo e quando usar

### Cenário 2: Stub em Ação
Retorna lista fixa de livros

### Cenário 3: Spy em Ação
Registra todas as chamadas feitas

### Cenário 4: Mock em Ação
Verifica expectativas de chamadas

### Cenário 5: Fake vs Repositório Real
Compara performance (Fake mais rápido)

### Cenário 6: Operações Assíncronas em Paralelo
Demonstra Task.WhenAll

### Cenário 7: Benefícios dos Testes Assíncronos
Lista vantagens

### Cenário 8: Comparação de Dublês
Tabela comparativa

---

## Comparação de Dublês

| Dublê | Quando Usar | Característica | Complexidade |
|-------|-------------|----------------|--------------|
| Dummy | Não é chamado | Lança exceção | Mínima |
| Stub | Dados fixos | Respostas simples | Baixa |
| Spy | Verificar uso | Registra chamadas | Média |
| Mock | Expectativas | Verifica contratos | Média |
| Fake | Funcionalidade | Implementação real | Alta |

---

## Operações Assíncronas

### Sequencial vs Paralelo

**Sequencial:**
```csharp
var book1 = await service.FindByIdAsync(1); // 30ms
var book2 = await service.FindByIdAsync(2); // 30ms
var book3 = await service.FindByIdAsync(3); // 30ms
// Total: ~90ms
```

**Paralelo:**
```csharp
var task1 = service.FindByIdAsync(1);
var task2 = service.FindByIdAsync(2);
var task3 = service.FindByIdAsync(3);
await Task.WhenAll(task1, task2, task3);
// Total: ~30ms (executa em paralelo!)
```

---

## Benefícios dos Testes Assíncronos

### 1. Simula I/O Real
- Banco de dados
- APIs externas
- Operações de arquivo
- Requisições HTTP

### 2. Testa Código Assíncrono
- `async/await` funciona corretamente
- Tratamento de exceções assíncronas
- Cancelamento com `CancellationToken`

### 3. Testes Paralelos
- Execução mais rápida
- Task.WhenAll para múltiplas operações
- Melhor aproveitamento de recursos

### 4. Dublês com Latência
- Fake simula delays reais
- Testes mais realistas
- Identifica problemas de performance

### 5. Spy para Verificação
- Registra ordem de chamadas
- Conta quantidade de operações
- Verifica parâmetros passados

---

## Diferenças: Síncrono vs Assíncrono

| Aspecto | Fase 4 (Síncrono) | Fase 9 (Assíncrono) |
|---------|-------------------|---------------------|
| Interface | IRepository | IAsyncRepository |
| Retorno | T | Task&lt;T&gt; |
| Métodos | GetById() | GetByIdAsync() |
| Performance | Bloqueante | Não-bloqueante |
| I/O | Síncrono | Assíncrono |
| Paralelo | Difícil | Fácil (Task.WhenAll) |
| Testes | FakeAlgorithmoOrdenacao | 5 tipos de dublês |

---

## Quando Usar Cada Dublê?

### Use DUMMY quando:
- Método não será chamado no teste
- Precisa preencher parâmetro obrigatório

### Use STUB quando:
- Precisa de dados fixos/pré-configurados
- Teste não precisa de lógica real
- Quer cenários determinísticos

### Use SPY quando:
- Precisa verificar SE método foi chamado
- Quer saber QUANTAS vezes foi chamado
- Precisa verificar ORDEM de chamadas
- Quer inspecionar PARÂMETROS passados

### Use MOCK quando:
- Tem expectativas rígidas sobre chamadas
- Precisa verificar contrato
- Quer garantir número exato de chamadas

### Use FAKE quando:
- Precisa de comportamento funcional
- Quer substituir banco de dados por memória
- Precisa simular latência
- Quer testes de integração rápidos

---

## Limitações e Considerações

### ⚠️ Implementação Didática

Estas implementações são para aprendizado e têm limitações:

1. **Sem tratamento de exceções assíncronas complexas**
2. **Sem suporte a CancellationToken**
3. **Sem retry policies**
4. **Simulação de latência simplificada**

**Em projetos reais:** Use frameworks de mock (Moq, NSubstitute) e bibliotecas de teste (xUnit, NUnit).

---

## Próximas Evoluções

A arquitetura está pronta para:
- Fase 10: Testes com frameworks (xUnit)
- Fase 11: Integração com banco de dados real
- Fase 12: APIs REST assíncronas

---

## Princípios Aplicados

### Dependency Inversion
- Serviços dependem de IAsyncRepository
- Não conhecem implementações concretas

### Interface Segregation
- Interface assíncrona coesa
- Métodos bem definidos

### Single Responsibility
- Cada dublê tem um propósito específico
- Testes focados em comportamentos únicos

### Open/Closed
- Aberto para novos dublês
- Fechado para modificação nos existentes

---

## Conclusão

Esta fase demonstra:

✅ **5 tipos de dublês** com propósitos distintos
✅ **Testes assíncronos** com async/await
✅ **Operações paralelas** com Task.WhenAll
✅ **Simulação de latência** para testes realistas
✅ **Spy para verificação** de comportamento
✅ **Mock para contratos** rígidos

**Resultado:** Testes mais robustos, rápidos e realistas!

