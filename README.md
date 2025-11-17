# Tarefa por Fases — Interfaces em C#

## Composição da Equipe

- Bruno Moura Mathias Fernandes Simão
- Eduardo Mendes
- João Pedro Domingues

## Sumário das Fases

- [Fase 0 - Aquecimento Conceitual: Contratos de Capacidade](#fase-0---aquecimento-conceitual-contratos-de-capacidade)
- [Fase 1 - Heurística antes do código (Mapa Mental)](#fase-1---heurística-antes-do-código-mapa-mental)
- [Fase 2 - Procedural Mínimo](#fase-2---procedural-mínimo)
- [Fase 3 - Interfaces](#fase-3---interfaces)
- [Fase 4 - Interface Plugável e Testável](#fase-4---interface-plugável-e-testável)

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

---

## Fase 3 - Interfaces

### Objetivo
Transformar o código procedural da Fase 2 em uma solução orientada a objetos usando interfaces, eliminando o `switch` e aplicando princípios SOLID.

### Implementações Criadas

**Interface:** `IAlgoritmoOrdenacao`
- Contrato: `Ordenar(int[] lista)`

**Implementações:**
- `BubbleSortAlgorithm` - Para listas pequenas
- `InsertionSortAlgorithm` - Para listas médias
- `QuickSortAlgorithm` - Para listas grandes

**Componentes:**
- `ServicoOrdenacao` - Usa a interface (inversão de dependência)
- `CatalogoAlgoritmos` - Seleciona algoritmo por política

### Como executar

```bash
cd src/fase-03-interfaces
dotnet run
```

### 5 Cenários Demonstrados

1. **Lista pequena:** Seleção automática de Bubble Sort para 5 elementos
2. **Lista média:** Seleção automática de Insertion Sort para 20 elementos
3. **Lista grande:** Seleção automática de Quick Sort para 100 elementos
4. **Seleção manual:** Escolha explícita do algoritmo desejado
5. **Comparação:** Execução dos três algoritmos na mesma lista

### Melhorias em relação à Fase 2

1. **Eliminação do switch:** Não há mais ramificações centralizadas
2. **Open/Closed:** Novos algoritmos sem modificar código existente
3. **Testes isolados:** Cada algoritmo testável independentemente
4. **Política configurável:** Lógica de seleção separada do cliente
5. **Baixo acoplamento:** Cliente depende apenas da interface

### Decisões de Design

- **Inversão de Dependência (DIP):** Serviço depende de abstração, não de concreções
- **Injeção de Dependência:** Algoritmo injetado no construtor
- **Strategy Pattern:** Cada algoritmo como estratégia intercambiável
- **Catálogo de Componentes:** Registro centralizado de implementações

### Checklist de Qualidade

- [x] Interface clara com contrato bem definido
- [x] Mínimo 3 implementações concretas
- [x] Serviço que usa inversão de dependência
- [x] Catálogo com política de seleção
- [x] 5 cenários demonstrados
- [x] Código funcional e executável
- [x] Documentação completa em `EVOLUCAO.md`

---

## Fase 4 - Interface Plugável e Testável

### Objetivo
Consolidar a ideia de interfaces plugáveis introduzindo:
- Contrato explícito para o passo variável
- Ponto único de composição (catálogo)
- Cliente dependendo apenas do contrato
- **Testes com dublês** (fake/stub) sem I/O real

### Implementações Criadas

**Interface:** `IAlgoritmoOrdenacao`
- Contrato: `Ordenar(int[] lista)`

**Implementações (sealed):**
- `BubbleSortAlgorithm` - Para listas pequenas
- `InsertionSortAlgorithm` - Para listas médias
- `QuickSortAlgorithm` - Para listas grandes

**Componentes:**
- `ServicoOrdenacao` - Cliente que usa apenas a interface
- `CatalogoAlgoritmos` - Ponto único de composição com políticas
- `FakeAlgoritmoOrdenacao` - Dublê para testes sem I/O

**Testes:**
- `TestesComDubles` - 5 testes automatizados com fake

### Como executar

```bash
cd src/fase-04-plugavel-testavel
dotnet run
```

### Cenários Demonstrados

1. **Testes Automatizados:** 5 testes com dublê executados automaticamente
2. **Seleção Manual:** Usuário escolhe o algoritmo desejado
3. **Seleção Automática:** Por tamanho da lista (pequena/média/grande)
4. **Plugabilidade:** Demonstração dos três algoritmos na mesma lista

### Diferencial da Fase 4

**Novidade principal:** **Testes com dublês**
- Fake implementa a mesma interface
- Testes rápidos e determinísticos
- Sem I/O real
- Verifica comportamento do cliente isoladamente

### Testes Implementados

1. **DeveUsarAlgoritmoInjetado** - Verifica injeção de dependência
2. **ListaVazia_DeveRetornarListaVazia** - Testa caso de borda
3. **ListaNula_DeveRetornarListaVazia** - Testa validação
4. **AlgoritmoNulo_DeveLancarExcecao** - Testa proteção contra null
5. **DeveDelegarParaAlgoritmo** - Verifica delegação correta

### Benefícios Alcançados

1. **Testabilidade Total**
   - Dublês permitem testar sem implementações reais
   - Testes rápidos (sem I/O)
   - Testes determinísticos (sempre mesmo resultado)

2. **Alternância Verdadeira**
   - Cliente não conhece implementações concretas
   - Composição centralizada no catálogo
   - Política externa ao cliente

3. **Manutenibilidade**
   - Validação no cliente
   - Classes sealed (não podem ser herdadas incorretamente)
   - Responsabilidades bem definidas

### Princípios SOLID Aplicados

- **S** - Single Responsibility: Cada classe tem uma responsabilidade
- **O** - Open/Closed: Aberto para extensão, fechado para modificação
- **L** - Liskov Substitution: Implementações são intercambiáveis
- **I** - Interface Segregation: Interface coesa com único método
- **D** - Dependency Inversion: Cliente depende de abstração

### Checklist de Qualidade

- [x] Interface clara e bem definida
- [x] Mínimo 3 implementações sealed
- [x] Cliente com injeção de dependência
- [x] Validação de parâmetros
- [x] Catálogo centralizado
- [x] Dublê (fake) implementado
- [x] 5 testes automatizados
- [x] Testes executam sem I/O
- [x] Código funcional e executável
- [x] Documentação completa em `COMPOSICAO-E-TESTES.md`

