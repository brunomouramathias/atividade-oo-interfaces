# Fase 1 — Heurística antes do código (mapa mental)

## Exemplo 1: Codificação de Mensagem

### Problema escolhido

Queremos proteger o conteúdo de uma mensagem antes de transmiti-la ou armazená-la, escolhendo automaticamente o método de codificação adequado (Base64 ou Cifra de César) conforme o contexto de uso.

---

### Quadro 1 — Procedural (onde surgem if/switch)

**Fluxo:**
- Recebe mensagem de texto
- `if (contexto == "log_interno")` então aplica Base64 senão aplica Cifra de César
- Executa codificação
- Retorna mensagem codificada

**Código conceitual:**
```
função CodificarMensagem(texto, contexto):
    se contexto == "log_interno":
        retornar CodificarBase64(texto)
    senão se contexto == "dados_sensiveis":
        retornar CodificarCesar(texto, 3)
    senão:
        retornar texto

função CodificarBase64(texto):
    // converte bytes para Base64
    retornar textoBase64

função CodificarCesar(texto, deslocamento):
    resultado = ""
    para cada caractere em texto:
        resultado += (caractere + deslocamento)
    retornar resultado
```

**Decisões embutidas no fluxo:**
- Ramificações por contexto de uso
- Lógica de escolha misturada com lógica de codificação

**Sinais de dor identificados:**
- Cada novo método (ROT13, AES, SHA) adiciona novos `if/switch`
- Difícil testar cada codificador isoladamente
- Cliente precisa conhecer os contextos válidos

---

### Quadro 2 — OO sem interface (quem encapsula o quê; o que ainda fica rígido)

**Encapsulamento:**
- Criamos classes concretas: `CodificadorBase64`, `CodificadorCesar`
- Cada classe encapsula a lógica do seu algoritmo
- Um "serviço" de codificação orquestra o fluxo

**Código conceitual:**
```
classe abstrata Codificador:
    método abstrato Codificar(texto)
    método abstrato Decodificar(texto)

classe CodificadorBase64 herda Codificador:
    método Codificar(texto):
        // implementação Base64
        retornar textoBase64
    
    método Decodificar(texto):
        // implementação reversa Base64
        retornar textoOriginal

classe CodificadorCesar herda Codificador:
    deslocamento = 3
    
    método Codificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere + deslocamento)
        retornar resultado
    
    método Decodificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere - deslocamento)
        retornar resultado

classe ServicoCodificacao:
    método ProcessarMensagem(texto, contexto):
        se contexto == "log_interno":
            codificador = novo CodificadorBase64()
        senão:
            codificador = novo CodificadorCesar()
        retornar codificador.Codificar(texto)
```

**Melhoras obtidas:**
- Coesão: cada codificador em sua própria classe
- Menos duplicação de código
- Mais fácil adicionar novos codificadores

**Rigidez remanescente:**
- **Serviço ainda conhece as classes concretas**
- **`if` de escolha ainda existe** no orquestrador
- Trocar o critério de seleção exige alterar o ServicoCodificacao

---

### Quadro 3 — Com interface (contrato que permite alternar + ponto de composição)

**Contrato (o que):**
"Transformar o texto original em uma versão codificada e permitir decodificação"

**Implementações (como):**
- `CodificadorBase64` - para logs internos e dados não sensíveis
- `CodificadorCesar` - para proteção básica de dados sensíveis
- Futuras: `CodificadorROT13`, `CodificadorAES`

**Código conceitual:**
```
interface ICodificador:
    método Codificar(texto)
    método Decodificar(texto)

classe CodificadorBase64 implementa ICodificador:
    método Codificar(texto):
        // implementação Base64
        retornar textoBase64
    
    método Decodificar(texto):
        // implementação reversa Base64
        retornar textoOriginal

classe CodificadorCesar implementa ICodificador:
    deslocamento = 3
    
    método Codificar(texto):
        // implementação Cifra de César
        retornar textoCodificado
    
    método Decodificar(texto):
        // implementação reversa
        retornar textoOriginal

classe GerenciadorMensagens:
    codificador: ICodificador
    
    construtor(codificador):
        this.codificador = codificador
    
    método EnviarMensagem(texto):
        retornar codificador.Codificar(texto)
    
    método ReceberMensagem(textoCodificado):
        retornar codificador.Decodificar(textoCodificado)
```

**Ponto de composição (fora do cliente):**
```
// Catálogo de codificadores por chave
catalogo = {
    "log_interno": CodificadorBase64(),
    "dados_sensiveis": CodificadorCesar()
}

// Política configurada
função SelecionarCodificador(contexto):
    retornar catalogo[contexto]

// Uso
texto = "Mensagem secreta"
codificador = SelecionarCodificador("dados_sensiveis")
gerenciador = GerenciadorMensagens(codificador)
mensagemProtegida = gerenciador.EnviarMensagem(texto)
```

**Efeito alcançado:**
- **Cliente não muda** quando alternamos implementações ou política
- Testes ficam mais simples (podemos usar dublês/stubs)
- Nova política (ex: verificar tamanho da mensagem) não altera o GerenciadorMensagens
- Novos codificadores não modificam código existente

---

### 3 sinais de alerta previstos (Codificação)

#### 1. Cliente muda ao trocar implementação
**Cheiro:** Se precisar alterar o GerenciadorMensagens cada vez que adicionar um novo codificador ou mudar a política de seleção.

**Sinal de acoplamento ao "como":** O cliente conhece detalhes de implementação que não deveria conhecer.

#### 2. Ramificações espalhadas
**Cheiro:** `if/switch` por tipo de codificador ou contexto aparecem em vários pontos do código (validação, logging, seleção).

**Problema:** Cada mudança de critério exige alteração em múltiplos lugares, aumentando chance de bugs.

#### 3. Testes lentos e frágeis
**Cheiro:** Testes precisam executar codificadores reais, não conseguem simular falhas ou casos específicos facilmente.

**Sem dublês:** Difícil testar comportamento do gerenciador sem executar a codificação completa; impossível simular erro de formato inválido.

---

---

## Exemplo 2: Ordenação de Coleções

### Problema escolhido

Queremos ordenar uma lista de números permitindo que o sistema escolha automaticamente o algoritmo mais adequado (Bubble Sort ou Quick Sort) conforme o tamanho da lista.

---

### Quadro 1 — Procedural (onde surgem if/switch)

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

### Quadro 2 — OO sem interface (quem encapsula o quê; o que ainda fica rígido)

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

### Quadro 3 — Com interface (contrato que permite alternar + ponto de composição)

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

### 3 sinais de alerta previstos (Ordenação)

#### 1. Cliente muda ao trocar implementação
**Cheiro:** Se precisar alterar o ServicoOrdenacao cada vez que adicionar um novo algoritmo ou mudar a política de seleção.

**Sinal de acoplamento ao "como":** O cliente conhece detalhes de implementação que não deveria conhecer.

#### 2. Ramificações espalhadas
**Cheiro:** `if/switch` por tipo de algoritmo ou tamanho de lista aparecem em vários pontos do código (validação, logging, seleção).

**Problema:** Cada mudança de critério exige alteração em múltiplos lugares, aumentando chance de bugs.

#### 3. Testes lentos e frágeis
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

