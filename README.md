# Tarefa por Fases — Interfaces em C#

## Composição da Equipe

- Bruno Moura Mathias Fernandes Simão
- Eduardo Mendes
- João Pedro Domingues

## Sumário das Fases

- [Fase 0 - Aquecimento Conceitual: Contratos de Capacidade](#fase-0---aquecimento-conceitual-contratos-de-capacidade)
- [Fase 1 - Heurística antes do código (Mapa Mental)](#fase-1---heurística-antes-do-código-mapa-mental)
- [Fase 2 - Procedural Mínimo](#fase-2---procedural-mínimo)

---

## Fase 0 - Aquecimento Conceitual: Contratos de Capacidade

### Objetivo
Treinar o olhar de design: identificar um objetivo fixo e peças alternáveis que realizam esse objetivo por caminhos diferentes. Nomear o contrato (o que fazer) e duas implementações (como fazer), além de propor uma política simples para escolher entre as peças.

### Como executar
Esta fase é apenas conceitual, sem código. O documento completo está em: `src/fase-00-aquecimento/CONTRATOS-DE-CAPACIDADE.md`

### Casos Apresentados

**Caso 1: Codificação de Mensagem**
- Objetivo: Proteger conteúdo de mensagem
- Contrato: "Transformar texto em versão criptografada"
- Implementações: Base64 vs Cifra de César
- Política: Base64 para logs internos; Cifra de César para dados sensíveis básicos

**Caso 2: Ordenação de Coleções**
- Objetivo: Organizar lista de números ou objetos
- Contrato: "Ordenar a coleção em ordem crescente"
- Implementações: Bubble Sort vs Quick Sort
- Política: Listas pequenas → Bubble Sort; listas médias/grandes → Quick Sort

### Checklist de Qualidade

- [x] Cada caso tem objetivo, contrato, duas implementações e política bem definida
- [x] O contrato não revela "como" (é descritivo, não técnico)
- [x] As implementações são alternáveis (não são variações cosméticas)
- [x] A política é concreta e aplicável (não ambígua)
- [x] Há risco/observação por caso

---

## Fase 1 - Heurística antes do código (Mapa Mental)

### Objetivo
Desenhar um mapa de evolução para um problema trivial, mostrando a transição de código procedural para orientado a objetos e finalmente usando interfaces.

### Problemas Escolhidos
1. **Codificação de Mensagem** - Alternar entre Base64 e Cifra de César conforme contexto
2. **Ordenação de listas** - Alternar entre Bubble Sort e Quick Sort conforme tamanho

### Como executar
Esta fase é conceitual (sem código executável). O mapa mental completo está em: `src/fase-01-heuristica/MAPA-MENTAL.md`

### Estrutura do Mapa (2 exemplos completos)

**Exemplo 1: Codificação de Mensagem**
- Quadro 1: Função com `if` por contexto (log_interno vs dados_sensiveis)
- Quadro 2: Classes `CodificadorBase64` e `CodificadorCesar` 
- Quadro 3: Interface `ICodificador` com catálogo + GerenciadorMensagens

**Exemplo 2: Ordenação de Coleções**
- Quadro 1: Função com `if` por tamanho de lista
- Quadro 2: Classes `BubbleSortAlgorithm` e `QuickSortAlgorithm`
- Quadro 3: Interface `IAlgoritmoOrdenacao` com catálogo + ServicoOrdenacao

### Decisões de Design

**Contratos definidos:**
- `ICodificador` - Codificar(texto) e Decodificar(texto)
- `IAlgoritmoOrdenacao` - Ordenar(lista)

**Aplicação do ISP:** Interfaces coesas com métodos bem definidos e relacionados

**Ponto de composição:** Catálogo por chave + política de seleção externa ao cliente

**Alternância:** Trocar implementações ou políticas sem modificar clientes (GerenciadorMensagens, ServicoOrdenacao)

### 3 Sinais de Alerta Identificados (em cada exemplo)

1. **Cliente muda ao trocar implementação** - acoplamento ao "como"
2. **Ramificações espalhadas** - `if/switch` por tipo em vários pontos
3. **Testes lentos e frágeis** - sem dublês, difícil simular cenários

### Checklist de Qualidade

- [x] Mapa mostra evolução procedural → OO → interface
- [x] Identifica onde surgem `if/switch` no procedural
- [x] Explica o que melhora e o que fica rígido no OO sem interface
- [x] Define contrato claro e ponto de composição externo
- [x] Lista 3 sinais de alerta com explicação

---

## Fase 2 - Procedural Mínimo

### Objetivo
Implementar a ideia de modos (mínimo 3 + padrão) para um objetivo simples, demonstrando a abordagem procedural com `switch`.

### Implementações Criadas

**Exemplo 1: Codificação de Mensagem**
- Modos: `base64`, `cesar`, `invertido`, `padrão`
- Arquivo: `Codificacao.cs`

**Exemplo 2: Ordenação de Lista**
- Modos: `bubble`, `quick`, `insertion`, `padrão`
- Arquivo: `Ordenacao.cs`

### Como executar

```bash
cd src/fase-02-procedural
dotnet run
```

### 5 Cenários de Teste por Exemplo

**Codificação:**
1. Texto simples com Base64
2. Texto com caracteres especiais (Cifra de César)
3. Texto vazio
4. Modo não reconhecido (padrão)
5. Texto longo (fronteira de performance)

**Ordenação:**
1. Lista pequena (Bubble Sort)
2. Lista já ordenada (Quick Sort)
3. Lista com duplicatas (Insertion Sort)
4. Lista vazia
5. Lista grande (fronteira de performance)

### Por que não escala

1. **Adição de novos modos:** Cada novo modo exige modificar o `switch` central
2. **Lógica acoplada:** Decisão de "qual usar" está misturada com a execução
3. **Testes difíceis:** Não há como testar algoritmos isoladamente
4. **Ramificações espalhadas:** `switch` se repete em vários pontos
5. **Strings mágicas:** Modos como literais sem verificação em compilação

### Decisões de Design

- **Abordagem:** Procedural com `switch expression`
- **Modos:** Strings literais para identificação
- **Algoritmos:** Funções privadas para cada variação
- **Retorno:** Modo padrão quando string não reconhecida

### Checklist de Qualidade

- [x] Implementa mínimo 3 modos + padrão
- [x] 5 cenários de teste descritos por exemplo
- [x] Explicação clara do por que não escala
- [x] Código funcional e executável
- [x] Documentação completa em `ANALISE.md`

