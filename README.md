# Tarefa por Fases — Interfaces em C#

## Composição da Equipe

- Bruno Moura Mathias Fernandes Simão
- Eduardo Mendes
- João Pedro Domingues

## Sumário das Fases

- [Fase 0 - Aquecimento Conceitual: Contratos de Capacidade](#fase-0---aquecimento-conceitual-contratos-de-capacidade)
- [Fase 1 - Heurística antes do código (Mapa Mental)](#fase-1---heurística-antes-do-código-mapa-mental)

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

### Problema Escolhido
Ordenação de listas de números, alternando automaticamente entre Bubble Sort (listas pequenas) e Quick Sort (listas maiores).

### Como executar
Esta fase é conceitual (sem código executável). O mapa mental completo está em: `src/fase-01-heuristica/MAPA-MENTAL.md`

### Estrutura do Mapa

**Quadro 1 - Procedural:**
- Função com `if/switch` para escolher algoritmo por tamanho
- Decisões embutidas no fluxo principal
- Sinais de dor: novos algoritmos exigem mais `if`

**Quadro 2 - OO sem interface:**
- Classes `BubbleSortAlgorithm` e `QuickSortAlgorithm`
- Orquestrador ainda conhece classes concretas
- Melhora: coesão; Rigidez: `if` ainda existe no orquestrador

**Quadro 3 - Com interface:**
- Interface `IAlgoritmoOrdenacao`
- Ponto de composição: catálogo por chave + política externa
- Cliente não muda ao trocar algoritmos

### Decisões de Design

- **Contrato:** Interface `IAlgoritmoOrdenacao` com método `Ordenar(lista)`
- **Aplicação do ISP:** Interface coesa com um único método bem definido
- **Ponto de composição:** Política de seleção fora do cliente (catálogo + função de seleção)
- **Alternância:** Trocar algoritmo ou política sem modificar o ServicoOrdenacao

### 3 Sinais de Alerta Identificados

1. **Cliente muda ao trocar implementação** - acoplamento ao "como"
2. **Ramificações espalhadas** - `if/switch` por tipo em vários pontos
3. **Testes lentos e frágeis** - sem dublês, difícil simular cenários

### Checklist de Qualidade

- [x] Mapa mostra evolução procedural → OO → interface
- [x] Identifica onde surgem `if/switch` no procedural
- [x] Explica o que melhora e o que fica rígido no OO sem interface
- [x] Define contrato claro e ponto de composição externo
- [x] Lista 3 sinais de alerta com explicação

