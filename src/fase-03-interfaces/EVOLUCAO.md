# Fase 3 — Interfaces

## Implementação

Nesta fase, o código procedural da Fase 2 foi transformado em uma solução orientada a objetos usando interfaces.

### Estrutura

**Interface:** `IAlgoritmoOrdenacao`
- Método: `Ordenar(int[] lista)`

**Implementações:**
- `BubbleSortAlgorithm` - Para listas pequenas (< 10 elementos)
- `InsertionSortAlgorithm` - Para listas médias (10-50 elementos)
- `QuickSortAlgorithm` - Para listas grandes (> 50 elementos)

**Serviço:** `ServicoOrdenacao`
- Recebe a interface no construtor
- Delega a ordenação para a implementação recebida

**Catálogo:** `CatalogoAlgoritmos`
- Mantém registro dos algoritmos disponíveis
- Seleciona algoritmo baseado em política de tamanho
- Permite seleção manual por chave

---

## Como executar

```bash
cd src/fase-03-interfaces
dotnet run
```

---

## Cenários demonstrados

### Cenário 1: Lista pequena
Lista com 5 elementos é ordenada automaticamente com Bubble Sort

### Cenário 2: Lista média
Lista com 20 elementos é ordenada automaticamente com Insertion Sort

### Cenário 3: Lista grande
Lista com 100 elementos é ordenada automaticamente com Quick Sort

### Cenário 4: Seleção manual
Permite escolher manualmente qual algoritmo usar

### Cenário 5: Comparação
Executa os três algoritmos na mesma lista para comparação

---

## O que melhorou em relação à Fase 2

### 1. Eliminação do switch central
Não há mais `switch` para selecionar o algoritmo. A seleção é feita externamente.

### 2. Open/Closed Principle
Novos algoritmos podem ser adicionados sem modificar código existente. Basta criar uma nova classe que implementa `IAlgoritmoOrdenacao`.

### 3. Testes isolados
Cada algoritmo pode ser testado independentemente. É possível criar dublês (stubs/mocks) da interface.

### 4. Política configurável
A lógica de seleção está separada no `CatalogoAlgoritmos`. Mudar a política não afeta o `ServicoOrdenacao`.

### 5. Cliente desacoplado
O `ServicoOrdenacao` não conhece as implementações concretas, apenas a interface.

---

## Decisões de Design

### Inversão de Dependência
O serviço depende da abstração (interface), não das implementações concretas.

### Injeção de Dependência
O algoritmo é injetado no construtor do serviço, permitindo flexibilidade.

### Strategy Pattern
Cada algoritmo encapsula uma estratégia diferente de ordenação.

### Catálogo de Componentes
Centraliza o registro e seleção de algoritmos disponíveis.

---

## Comparação: Procedural vs Interface

| Aspecto | Fase 2 (Procedural) | Fase 3 (Interface) |
|---------|---------------------|---------------------|
| Adicionar algoritmo | Modificar switch | Criar nova classe |
| Testar algoritmo | Através da função central | Classe isolada |
| Trocar política | Alterar lógica interna | Configurar catálogo |
| Acoplamento | Alto (strings mágicas) | Baixo (interface) |
| Extensibilidade | Fechado | Aberto |

---

## Próximos passos possíveis

- Adicionar mais algoritmos (Merge Sort, Heap Sort)
- Implementar políticas mais complexas (verificar se já está ordenado)
- Criar testes unitários com mocks
- Adicionar tratamento de exceções
- Implementar ordenação genérica com `IComparable<T>`

