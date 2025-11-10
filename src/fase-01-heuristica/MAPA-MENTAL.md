# Fase 1 — Heurística antes do código (mapa mental)

## Problema escolhido

Queremos ordenar uma lista de números permitindo que o sistema escolha automaticamente o algoritmo mais adequado (Bubble Sort ou Quick Sort) conforme o tamanho da lista.

---

## Quadro 1 — Procedural (onde surgem if/switch)

**Fluxo:**
- Recebe lista de números
- `if (tamanho < 10)` então aplica Bubble Sort senão aplica Quick Sort
- Executa ordenação
- Retorna lista ordenada

**Código conceitual:**
```
função OrdenarLista(lista):
    se lista.tamanho < 10:
        retornar BubbleSort(lista)
    senão:
        retornar QuickSort(lista)

função BubbleSort(lista):
    para i de 0 até lista.tamanho:
        para j de 0 até lista.tamanho - i - 1:
            se lista[j] > lista[j+1]:
                trocar(lista[j], lista[j+1])
    retornar lista

função QuickSort(lista):
    // lógica de particionamento recursivo
```

**Decisões embutidas no fluxo:**
- Ramificações por tamanho da lista
- Lógica de escolha misturada com lógica de ordenação

**Sinais de dor identificados:**
- Cada novo algoritmo (Merge Sort, Heap Sort) adiciona novos `if/switch`
- Difícil testar cada algoritmo isoladamente
- Cliente precisa conhecer o critério de escolha (tamanho < 10)

---

## Quadro 2 — OO sem interface (quem encapsula o quê; o que ainda fica rígido)

**Encapsulamento:**
- Criamos classes concretas: `BubbleSortAlgorithm`, `QuickSortAlgorithm`
- Cada classe encapsula a lógica do seu algoritmo
- Um "serviço" de ordenação orquestra o fluxo

**Código conceitual:**
```
classe abstrata AlgoritmoOrdenacao:
    método abstrato Ordenar(lista)

classe BubbleSortAlgorithm herda AlgoritmoOrdenacao:
    método Ordenar(lista):
        // implementação bubble sort
        retornar lista ordenada

classe QuickSortAlgorithm herda AlgoritmoOrdenacao:
    método Ordenar(lista):
        // implementação quick sort
        retornar lista ordenada

classe ServicoOrdenacao:
    método OrdenarLista(lista):
        se lista.tamanho < 10:
            algoritmo = novo BubbleSortAlgorithm()
        senão:
            algoritmo = novo QuickSortAlgorithm()
        retornar algoritmo.Ordenar(lista)
```

**Melhoras obtidas:**
- Coesão: cada algoritmo em sua própria classe
- Menos duplicação de código
- Mais fácil adicionar novos algoritmos

**Rigidez remanescente:**
- **Serviço ainda conhece as classes concretas**
- **`if` de escolha ainda existe** no orquestrador
- Trocar o critério de seleção (ex: adicionar verificação de tipo de dados) exige alterar o ServicoOrdenacao

---

## Quadro 3 — Com interface (contrato que permite alternar + ponto de composição)

**Contrato (o que):**
"Ordenar uma coleção em ordem crescente"

**Implementações (como):**
- `BubbleSortAlgorithm` - para listas pequenas
- `QuickSortAlgorithm` - para listas médias/grandes
- Futuras: `MergeSortAlgorithm`, `HeapSortAlgorithm`

**Código conceitual:**
```
interface IAlgoritmoOrdenacao:
    método Ordenar(lista)

classe BubbleSortAlgorithm implementa IAlgoritmoOrdenacao:
    método Ordenar(lista):
        // implementação bubble sort
        retornar lista ordenada

classe QuickSortAlgorithm implementa IAlgoritmoOrdenacao:
    método Ordenar(lista):
        // implementação quick sort
        retornar lista ordenada

classe ServicoOrdenacao:
    algoritmo: IAlgoritmoOrdenacao
    
    construtor(algoritmo):
        this.algoritmo = algoritmo
    
    método OrdenarLista(lista):
        retornar algoritmo.Ordenar(lista)
```

**Ponto de composição (fora do cliente):**
```
// Catálogo de algoritmos por chave
catalogo = {
    "pequena": BubbleSortAlgorithm(),
    "grande": QuickSortAlgorithm()
}

// Política configurada
função SelecionarAlgoritmo(lista):
    se lista.tamanho < 10:
        retornar catalogo["pequena"]
    senão:
        retornar catalogo["grande"]

// Uso
lista = [5, 2, 8, 1, 9]
algoritmo = SelecionarAlgoritmo(lista)
servico = ServicoOrdenacao(algoritmo)
listaOrdenada = servico.OrdenarLista(lista)
```

**Efeito alcançado:**
- **Cliente não muda** quando alternamos implementações ou política
- Testes ficam mais simples (podemos usar dublês/stubs)
- Nova política (ex: verificar se está quase ordenado) não altera o ServicoOrdenacao
- Novos algoritmos não modificam código existente

---

## 3 sinais de alerta previstos

### 1. Cliente muda ao trocar implementação
**Cheiro:** Se precisar alterar o ServicoOrdenacao cada vez que adicionar um novo algoritmo ou mudar a política de seleção.

**Sinal de acoplamento ao "como":** O cliente conhece detalhes de implementação que não deveria conhecer.

### 2. Ramificações espalhadas
**Cheiro:** `if/switch` por tipo de algoritmo ou tamanho de lista aparecem em vários pontos do código (validação, logging, seleção).

**Problema:** Cada mudança de critério exige alteração em múltiplos lugares, aumentando chance de bugs.

### 3. Testes lentos e frágeis
**Cheiro:** Testes precisam executar algoritmos reais, não conseguem simular falhas ou casos específicos facilmente.

**Sem dublês:** Difícil testar comportamento do serviço sem executar a ordenação completa; impossível simular erro de memória ou timeout.

---

## Onde encontrar/testar

**Artefato do mapa:**
- Este documento em `src/fase-01-heuristica/MAPA-MENTAL.md`

**README atualizado:**
- Composição da equipe (nomes e RAs)
- Link para Fase 1
- Instruções de visualização do mapa mental

