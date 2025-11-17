# Fase 4 — Interface Plugável e Testável

## Objetivo

Evoluir a solução da Fase 3 introduzindo:
- **Contrato explícito** (interface) para o passo variável
- **Ponto único de composição** (catálogo) que converte política em implementação
- **Cliente** dependendo apenas do contrato (não conhece classes concretas)
- **Testes com dublês** (fake/stub) sem I/O real

---

## Estrutura da Solução

### 1. Contrato (Interface)

```csharp
public interface IAlgoritmoOrdenacao
{
    int[] Ordenar(int[] lista);
}
```

**O que faz:** Define o comportamento esperado sem revelar "como"  
**Por que existe:** Permite alternância entre implementações sem alterar o cliente

---

### 2. Implementações Concretas

#### BubbleSortAlgorithm

```csharp
public sealed class BubbleSortAlgorithm : IAlgoritmoOrdenacao
{
    public int[] Ordenar(int[] lista)
    {
        // Implementação Bubble Sort
        // Eficiente para listas pequenas
    }
}
```

#### QuickSortAlgorithm

```csharp
public sealed class QuickSortAlgorithm : IAlgoritmoOrdenacao
{
    public int[] Ordenar(int[] lista)
    {
        // Implementação Quick Sort
        // Eficiente para listas grandes
    }
}
```

#### InsertionSortAlgorithm

```csharp
public sealed class InsertionSortAlgorithm : IAlgoritmoOrdenacao
{
    public int[] Ordenar(int[] lista)
    {
        // Implementação Insertion Sort
        // Eficiente para listas médias
    }
}
```

**Características:**
- Classes `sealed` (não podem ser herdadas)
- Cada uma encapsula um algoritmo específico
- Todas seguem o mesmo contrato

---

### 3. Cliente (dependente do contrato)

```csharp
public sealed class ServicoOrdenacao
{
    private readonly IAlgoritmoOrdenacao _algoritmo;

    public ServicoOrdenacao(IAlgoritmoOrdenacao algoritmo)
    {
        _algoritmo = algoritmo ?? throw new ArgumentNullException(nameof(algoritmo));
    }

    public int[] ProcessarLista(int[] lista)
    {
        if (lista == null || lista.Length == 0)
            return lista ?? new int[0];
        
        return _algoritmo.Ordenar(lista);
    }
}
```

**Princípios aplicados:**
- **Injeção de Dependência:** Algoritmo recebido via construtor
- **Inversão de Dependência (DIP):** Depende de abstração, não de concreção
- **Validação:** Verifica parâmetros e lança exceção se necessário

---

### 4. Ponto de Composição (Catálogo)

```csharp
public static class CatalogoAlgoritmos
{
    public static IAlgoritmoOrdenacao Resolver(string modo)
    {
        return modo?.ToLowerInvariant() switch
        {
            "bubble" => new BubbleSortAlgorithm(),
            "quick" => new QuickSortAlgorithm(),
            "insertion" => new InsertionSortAlgorithm(),
            _ => new QuickSortAlgorithm() // padrão seguro
        };
    }

    public static IAlgoritmoOrdenacao ResolverPorTamanho(int tamanho)
    {
        if (tamanho < 10)
            return new BubbleSortAlgorithm();
        else if (tamanho < 50)
            return new InsertionSortAlgorithm();
        else
            return new QuickSortAlgorithm();
    }
}
```

**Responsabilidades:**
- Centraliza a criação de implementações
- Aplica política de seleção
- Único lugar que conhece as classes concretas

---

### 5. Teste com Dublê (Fake)

```csharp
public sealed class FakeAlgoritmoOrdenacao : IAlgoritmoOrdenacao
{
    public int[] UltimaListaRecebida { get; private set; }
    public int QuantidadeDeChamadas { get; private set; }

    public int[] Ordenar(int[] lista)
    {
        QuantidadeDeChamadas++;
        UltimaListaRecebida = (int[])lista.Clone();
        
        // Retorna uma ordenação previsível para testes
        return new int[] { 1, 2, 3 };
    }
}
```

**Por que usar dublês:**
- Sem I/O real (rápido e determinístico)
- Permite verificar comportamento do cliente
- Isola o que está sendo testado

---

## Exemplos de Uso

### Uso 1: Composição Manual

```csharp
int[] lista = { 42, 13, 7, 25, 18 };
IAlgoritmoOrdenacao algoritmo = new QuickSortAlgorithm();
ServicoOrdenacao servico = new ServicoOrdenacao(algoritmo);
int[] resultado = servico.ProcessarLista(lista);
```

### Uso 2: Composição via Catálogo

```csharp
int[] lista = { 5, 2, 8, 1 };
IAlgoritmoOrdenacao algoritmo = CatalogoAlgoritmos.ResolverPorTamanho(lista.Length);
ServicoOrdenacao servico = new ServicoOrdenacao(algoritmo);
int[] resultado = servico.ProcessarLista(lista);
```

### Uso 3: Teste com Dublê

```csharp
// Arrange
var fake = new FakeAlgoritmoOrdenacao();
var servico = new ServicoOrdenacao(fake);
int[] listaEntrada = { 5, 2, 8, 1 };

// Act
var resultado = servico.ProcessarLista(listaEntrada);

// Assert
// Verifica se o algoritmo foi chamado corretamente
// Verifica se a lista foi passada para o algoritmo
// Verifica o resultado retornado
```

---

## Como executar

```bash
cd src/fase-04-plugavel-testavel
dotnet run
```

O programa executa automaticamente:
1. **Testes com dublês** - 5 testes automatizados
2. **Cenário interativo** - Seleção manual do algoritmo
3. **Seleção automática** - Por tamanho da lista
4. **Demonstração de plugabilidade** - Todos os algoritmos

---

## Benefícios da Abordagem

### 1. Testabilidade
- Testes rápidos sem I/O
- Dublês permitem simular cenários específicos
- Fácil isolar comportamento

### 2. Flexibilidade
- Trocar implementação sem alterar cliente
- Adicionar novos algoritmos facilmente
- Política configurável

### 3. Manutenibilidade
- Cada classe tem responsabilidade única
- Baixo acoplamento
- Alto coesão

### 4. Princípios SOLID

#### S - Single Responsibility
Cada algoritmo em sua própria classe

#### O - Open/Closed
Aberto para extensão (novos algoritmos), fechado para modificação

#### L - Liskov Substitution
Qualquer `IAlgoritmoOrdenacao` pode substituir outra

#### I - Interface Segregation
Interface coesa com único método necessário

#### D - Dependency Inversion
Cliente depende de abstração, não de concreção

---

## Comparação: Fase 3 vs Fase 4

| Aspecto | Fase 3 | Fase 4 |
|---------|---------|---------|
| Interface | ✓ | ✓ |
| Implementações | ✓ | ✓ (sealed) |
| Cliente desacoplado | ✓ | ✓ (com validação) |
| Catálogo | ✓ | ✓ (com políticas) |
| Testes com dublês | ✗ | ✓ |
| Testes automatizados | ✗ | ✓ |
| Documentação completa | Parcial | Completa |

---

## Lições Aprendidas

1. **Interfaces permitem alternância verdadeira** - Cliente não conhece implementações

2. **Composição centralizada** - Um único ponto decide qual implementação usar

3. **Dublês tornam testes práticos** - Rápidos, determinísticos, sem I/O

4. **Validação no cliente** - Protege contra usos incorretos

5. **Política externa** - Lógica de seleção separada do cliente

