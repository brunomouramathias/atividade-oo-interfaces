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
- [Fase 5 - Repository InMemory](#fase-5---repository-inmemory)
- [Fase 6 - Repository CSV](#fase-6---repository-csv)
- [Fase 7 - Repository JSON](#fase-7---repository-json)
- [Fase 8 - ISP (Interface Segregation Principle)](#fase-8---isp-interface-segregation-principle)
- [Fase 9 - Dublês Avançados e Testes Assíncronos](#fase-9---dublês-avançados-e-testes-assíncronos)

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

---

## Fase 5 - Repository InMemory

### Objetivo
Introduzir o padrão **Repository** como ponto único de acesso a dados, usando um domínio simples (Book) com implementação InMemory baseada em coleção Dictionary.

**Ideia-chave:** O cliente fala só com o Repository, nunca com coleções diretamente.

### Implementações Criadas

**Contrato:** `IRepository<T, TId>`
- Operações: `Add`, `GetById`, `ListAll`, `Update`, `Remove`
- Genérico para qualquer tipo e identificador

**Implementação:** `InMemoryRepository<T, TId>`
- Armazenamento: `Dictionary<TId, T>`
- Sem I/O: Totalmente em memória
- Configurável: Recebe `Func<T, TId>` para extrair ID

**Domínio:** `Book`
- Record imutável com Id, Title, Author
- Simples e focado

**Serviço:** `BookService`
- Validações de domínio
- Orquestra operações com Repository
- Não acessa coleções diretamente

### Como executar

```bash
cd src/fase-05-repository-inmemory
dotnet run
```

### Testes e Cenários

**8 Testes Automatizados:**
1. Add_Then_ListAll_ShouldReturnOneItem
2. GetById_Existing_ShouldReturnEntity
3. GetById_Missing_ShouldReturnNull
4. Update_Existing_ShouldReturnTrue
5. Update_Missing_ShouldReturnFalse
6. Remove_Existing_ShouldReturnTrue
7. Remove_Missing_ShouldReturnFalse
8. MultipleOperations_ShouldWork

**6 Cenários de Uso:**
1. Cadastro de livros
2. Listagem completa
3. Busca por ID (existente e inexistente)
4. Atualização de dados
5. Remoção de item
6. Operações em lote

### Benefícios do Padrão Repository

1. **Abstração de Persistência**
   - Cliente não sabe como dados são armazenados
   - Fácil trocar InMemory por CSV, JSON, BD

2. **Testabilidade**
   - Repository facilmente mockável
   - Testes rápidos sem I/O real

3. **Centralização**
   - Único ponto de acesso a dados
   - Lógica de persistência isolada

4. **Flexibilidade**
   - Trocar implementação sem alterar cliente
   - Adicionar cache, logging transparentemente

### Decisões de Design

**Política de ID:** O ID vem do domínio (fornecido externamente)
```csharp
new InMemoryRepository<Book, int>(book => book.Id)
```

**Retorno de coleções:** `IReadOnlyList<T>` para não expor coleções mutáveis

**Separação de responsabilidades:**
- Repository: acesso a dados
- Service: regras de negócio
- Model: estrutura de dados

### Arquitetura

```
BookService (Cliente)
    ↓
IRepository<Book, int> (Contrato)
    ↓
InMemoryRepository<Book, int> (Implementação)
    ↓
Dictionary<int, Book> (Armazenamento)
```

### Princípios SOLID Aplicados

- **S** - Repository focado em acesso a dados
- **O** - Aberto para novas implementações (CSV, JSON)
- **L** - Implementações intercambiáveis
- **I** - Interface coesa com operações essenciais
- **D** - Cliente depende de abstração

### Preparação para Próximas Fases

A arquitetura está pronta para:
- Fase 6: Repository com CSV
- Fase 7: Repository com JSON  
- Fase 8: Repository com Banco de Dados

**Facilidade:** Apenas trocar implementação, cliente não muda!

### Checklist de Qualidade

- [x] Contrato genérico `IRepository<T, TId>`
- [x] Implementação InMemory com Dictionary
- [x] Domínio simples (Book)
- [x] Serviço que usa apenas o Repository
- [x] 8 testes automatizados cobrindo operações
- [x] Testes de fronteira (ID ausente, etc.)
- [x] Sem I/O (totalmente em memória)
- [x] Cliente não acessa coleções diretamente
- [x] IReadOnlyList para proteção
- [x] Código funcional e executável
- [x] Documentação completa em `REPOSITORY-PATTERN.md`

---

## Fase 6 - Repository CSV

### Objetivo
Evoluir o Repository para **persistir em arquivo CSV**, mantendo o mesmo contrato `IRepository<T, TId>` mas agora com dados sobrevivendo em disco.

**Ideia-chave:** Dados persistem em arquivo `.csv` com cabeçalho, encoding UTF-8 e escape correto.

### Estrutura Organizada em Pastas

```
fase-06-repository-csv/
├── Domain/              # Modelos de domínio
│   └── Book.cs
├── Repository/          # Lógica de persistência
│   ├── IRepository.cs
│   └── CsvBookRepository.cs
├── Services/            # Regras de negócio
│   └── BookService.cs
├── Tests/               # Testes de integração
│   └── TestesRepositorioCsv.cs
├── Program.cs           # Demonstração
└── REPOSITORY-CSV.md    # Documentação
```

**Organização:** Código estruturado em pastas para melhor navegação no GitHub!

### Implementações Criadas

**Contrato:** `IRepository<T, TId>` (reutilizado da Fase 5)

**Implementação CSV:** `CsvBookRepository`
- Armazenamento: Arquivo `.csv` em disco
- Encoding: UTF-8
- Cabeçalho: `Id,Title,Author`
- Escape: Vírgulas e aspas corretamente tratadas

**Formato CSV:**
```csv
Id,Title,Author
1,Código Limpo,Robert C. Martin
10,"Livro, com vírgula","Autor ""com aspas"""
```

### Como executar

```bash
cd src/fase-06-repository-csv
dotnet run
```

### Testes e Cenários

**10 Testes de Integração:**
1. ListAll_WhenFileDoesNotExist_ShouldReturnEmpty
2. Add_Then_ListAll_ShouldPersistInFile
3. Add_WithCommaAndQuotes_ShouldEscapeCorrectly
4. GetById_Existing_ShouldReturnBook
5. GetById_Missing_ShouldReturnNull
6. Update_Existing_ShouldPersistChanges
7. Update_Missing_ShouldReturnFalse
8. Remove_Existing_ShouldDeleteFromFile
9. Remove_Missing_ShouldReturnFalse
10. MultipleOperations_ShouldPersist

**6 Cenários de Uso:**
1. Cadastro de livros
2. Listagem de arquivo CSV
3. Campos com vírgulas e aspas (escape)
4. Atualização persistida
5. Remoção do arquivo
6. Demonstração de persistência entre execuções

### Formato CSV - Detalhes Técnicos

**Regras de Escape:**
- **Vírgulas:** Campo com `,` é colocado entre `"aspas"`
- **Aspas:** `"` é escapado como `""`
- **Quebras de linha:** Campo com `\n` entre aspas
- **Encoding:** UTF-8 para acentuação

**Política de ID:** Fornecido pelo domínio (externo)

### Métodos Principais

**Load()** - Carrega do arquivo
- Verifica existência do arquivo
- Parse com tratamento de escape
- Pula cabeçalho se existir

**Save()** - Salva no arquivo
- Gera cabeçalho
- Ordena por ID
- Escape de campos especiais

**Escape()** - Trata caracteres especiais
- Detecta necessidade de aspas
- Escapa aspas duplas

**SplitCsvLine()** - Parse de linha CSV
- Respeita campos entre aspas
- Trata aspas escapadas

### Diferenças: InMemory vs CSV

| Aspecto | Fase 5 (InMemory) | Fase 6 (CSV) |
|---------|-------------------|--------------|
| Armazenamento | Dictionary (RAM) | Arquivo .csv (disco) |
| Persistência | ❌ Perdida ao fechar | ✅ Sobrevive |
| Performance | Muito rápida | Mais lenta (I/O) |
| Legibilidade | N/A | Humano pode ler |
| Portabilidade | N/A | Excel/LibreOffice |
| Encoding | N/A | UTF-8 |
| Testes | Sem I/O | Arquivo temporário |

### Vantagens do CSV

1. **Persistência Real**
   - Dados não são perdidos
   - Sobrevivem entre execuções

2. **Portabilidade**
   - Formato universal
   - Integra com ferramentas

3. **Legibilidade**
   - Humano pode ler/editar
   - Útil para depuração

4. **Compatibilidade**
   - Abre no Excel
   - Importa em BDs

### Limitações Conhecidas

⚠️ **Implementação didática com limitações:**

- Sem tratamento de concorrência
- Performance em listas grandes (carrega/salva tudo)
- Sem versionamento de esquema
- Sem transações

**Em produção:** Usar bibliotecas especializadas ou banco de dados.

### Preparação para Próximas Fases

A arquitetura está pronta para:
- Fase 7: Repository com JSON
- Fase 8: Repository com Banco de Dados

**Facilidade:** Apenas trocar implementação, cliente não muda!

### Checklist de Qualidade

- [x] Contrato `IRepository<T, TId>` reutilizado
- [x] Implementação `CsvBookRepository` completa
- [x] Formato CSV com cabeçalho
- [x] Encoding UTF-8
- [x] Escape de vírgulas e aspas
- [x] 10 testes de integração
- [x] Testes com arquivo temporário
- [x] Casos especiais (vírgulas, aspas)
- [x] Persistência real demonstrada
- [x] **Código organizado em pastas** (Domain, Repository, Services, Tests)
- [x] .gitignore para arquivos CSV
- [x] Documentação completa em `REPOSITORY-CSV.md`

---

## Fase 7 - Repository JSON

### Objetivo
Evoluir o Repository da Fase 6 para **persistir em arquivo JSON** usando `System.Text.Json`, mantendo o mesmo contrato `IRepository<T, TId>` mas agora com formato estruturado e legível.

**Ideia-chave:** Dados persistem em arquivo `.json` com formatação indentada, usando o serializer nativo do .NET.

### Estrutura Organizada em Pastas

```
fase-07-repository-json/
├── Domain/              # Modelos de domínio
│   └── Book.cs
├── Repository/          # Lógica de persistência
│   ├── IRepository.cs
│   └── JsonBookRepository.cs
├── Services/            # Regras de negócio
│   └── BookService.cs
├── Tests/               # Testes de integração
│   └── TestesRepositorioJson.cs
├── Program.cs           # Demonstração
└── REPOSITORY-JSON.md   # Documentação
```

**Organização:** Mesma estrutura profissional da Fase 6!

### Implementações Criadas

**Contrato:** `IRepository<T, TId>` (reutilizado da Fase 5 e 6)

**Implementação JSON:** `JsonBookRepository`
- Armazenamento: Arquivo `.json` em disco
- Serialização: System.Text.Json (nativo do .NET)
- Formatação: Indentação automática (WriteIndented)
- Naming: camelCase (PropertyNamingPolicy)
- Escape: Automático (aspas, barras, caracteres especiais)

**Formato JSON:**
```json
[
  {
    "id": 1,
    "title": "Código Limpo",
    "author": "Robert C. Martin"
  },
  {
    "id": 10,
    "title": "Livro com \"aspas\" e /barras/",
    "author": "Autor: João & Maria"
  }
]
```

### Como executar

```bash
cd src/fase-07-repository-json
dotnet run
```

### Testes e Cenários

**10 Testes de Integração:**
1. ListAll_WhenFileDoesNotExist_ShouldReturnEmpty
2. Add_Then_ListAll_ShouldPersistInFile
3. Add_WithSpecialCharacters_ShouldHandleCorrectly
4. GetById_Existing_ShouldReturnBook
5. GetById_Missing_ShouldReturnNull
6. Update_Existing_ShouldPersistChanges
7. Update_Missing_ShouldReturnFalse
8. Remove_Existing_ShouldDeleteFromFile
9. Remove_Missing_ShouldReturnFalse
10. MultipleOperations_ShouldPersist

**6 Cenários de Uso:**
1. Cadastro de livros
2. Listagem de arquivo JSON
3. Caracteres especiais (aspas, barras, símbolos)
4. Atualização persistida
5. Remoção do arquivo
6. Demonstração de persistência entre execuções

### Formato JSON - Detalhes Técnicos

**Configuração do Serializer:**
```csharp
_options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

**Vantagens:**
- Escape automático de caracteres especiais
- Formatação indentada para legibilidade
- Sem parse manual necessário
- Tipagem forte na deserialização

### Métodos Principais

**Load()** - Deserializa do arquivo
- Verifica existência do arquivo
- Deserializa com JsonSerializer.Deserialize<List<Book>>()
- Tratamento de erros (retorna lista vazia)

**Save()** - Serializa no arquivo
- Ordena por ID
- Serializa com JsonSerializer.Serialize()
- Formatação automática

### Diferenças: CSV vs JSON

| Aspecto | Fase 6 (CSV) | Fase 7 (JSON) |
|---------|--------------|---------------|
| Formato | Texto com vírgulas | Estruturado JSON |
| Legibilidade | Boa (Excel) | Excelente (estruturado) |
| Escape Manual | Sim (vírgulas, aspas) | Não (automático) |
| Biblioteca | Manual (SplitCsvLine) | System.Text.Json |
| Complexidade | Média (parse manual) | Baixa (serialização automática) |
| Tipagem | Texto → parse | Automática |
| Cabeçalho | Necessário | Não necessário |
| Indentação | N/A | Sim (WriteIndented) |
| Padrão Web | ❌ | ✅ |

### Vantagens do JSON

1. **Serialização Automática**
   - System.Text.Json cuida de tudo
   - Sem parse manual
   - Menos código, menos bugs

2. **Formato Estruturado**
   - JSON é padrão universal
   - Fácil integração com APIs
   - Suporte nativo em navegadores

3. **Escape Automático**
   - Não precisa tratar aspas, vírgulas manualmente
   - Serializer cuida de caracteres especiais

4. **Tipagem Forte**
   - Deserialização direta para objetos
   - IntelliSense e type-safety

5. **Legibilidade Superior**
   - Formatação indentada
   - Estrutura clara
   - Fácil edição manual

### Limitações Conhecidas

⚠️ **Implementação didática com limitações:**

- Sem tratamento de concorrência
- Performance em listas grandes (carrega/salva tudo)
- Sem versionamento de esquema
- Sem transações

**Em produção:** Usar bibliotecas especializadas ou banco de dados.

### Por que System.Text.Json?

1. **Nativo do .NET** - Sem dependências externas
2. **Performance** - Otimizado para .NET Core/.NET 5+
3. **Segurança** - Mantido pela Microsoft
4. **Simplicidade** - API direta e clara

### Preparação para Próximas Fases

A arquitetura está pronta para:
- Fase 8: Repository com Banco de Dados
- Fase 9: Repository com NoSQL

**Facilidade:** Apenas trocar implementação, cliente não muda!

### Checklist de Qualidade

- [x] Contrato `IRepository<T, TId>` reutilizado
- [x] Implementação `JsonBookRepository` completa
- [x] System.Text.Json para serialização
- [x] Formatação indentada (WriteIndented)
- [x] CamelCase nas propriedades
- [x] Escape automático de caracteres especiais
- [x] 10 testes de integração
- [x] Testes com arquivo temporário
- [x] Casos especiais (aspas, símbolos)
- [x] Persistência real demonstrada
- [x] **Código organizado em pastas** (Domain, Repository, Services, Tests)
- [x] .gitignore para arquivos JSON
- [x] Documentação completa em `REPOSITORY-JSON.md`

---

## Fase 8 - ISP (Interface Segregation Principle)

### Objetivo
Aplicar o **ISP (Interface Segregation Principle)** segregando a interface `IRepository<T, TId>` em interfaces menores e mais coesas, permitindo que clientes dependam apenas dos métodos que realmente precisam.

**Ideia-chave:** Clientes não devem ser forçados a depender de interfaces que não usam.

### Estrutura Organizada em Pastas

```
fase-08-isp/
├── Domain/
│   ├── Interfaces/          # Interfaces segregadas
│   │   ├── IReadRepository.cs
│   │   ├── IWriteRepository.cs
│   │   └── IRepository.cs
│   ├── Entities/
│   │   └── Book.cs
│   └── Repositories/
│       └── InMemoryBookRepository.cs
├── Services/
│   ├── ReadOnlyBookService.cs
│   └── BookService.cs
├── Tests/
│   └── TestesIsp.cs
├── Program.cs
└── ISP.md
```

**Organização:** Interfaces segregadas no Domain, Repositórios no Domain, Testes separados!

### O Problema sem ISP

Antes tínhamos uma única interface com 5 métodos:

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

**Problema:** Um serviço que precisa apenas de leitura é forçado a receber interface com métodos de escrita.

### A Solução com ISP

**1. IReadRepository<T, TId>** - Apenas leitura
```csharp
public interface IReadRepository<T, TId>
{
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
}
```

**2. IWriteRepository<T, TId>** - Apenas escrita
```csharp
public interface IWriteRepository<T, TId>
{
    T Add(T entity);
    bool Update(T entity);
    bool Remove(TId id);
}
```

**3. IRepository<T, TId>** - Interface completa
```csharp
public interface IRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
{
    // Herda de ambas
}
```

### Como executar

```bash
cd src/fase-08-isp
dotnet run
```

### Testes: 5/5 passaram ✅

### Benefícios do ISP

- Clientes dependem apenas do necessário
- Segurança em compile-time
- Interfaces menores e coesas
- Facilita testes e mocks

### Checklist de Qualidade

- [x] Interfaces segregadas (IReadRepository, IWriteRepository)
- [x] Interface completa herda de ambas
- [x] ReadOnlyService usa apenas IReadRepository
- [x] 5 testes demonstrando ISP
- [x] **Código organizado em pastas**
- [x] Documentação completa em `ISP.md`

---

## Fase 9 - Dublês Avançados e Testes Assíncronos

### Objetivo
Introduzir **5 tipos de dublês avançados** (Dummy, Stub, Spy, Mock, Fake) e implementar **testes assíncronos** usando `async/await`, demonstrando como testar código assíncrono.

**Ideia-chave:** Diferentes tipos de dublês para diferentes propósitos, com operações assíncronas simulando I/O real.

### Estrutura Organizada em Pastas

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

### Tipos de Dublês

| Dublê | Quando Usar | Característica |
|-------|-------------|----------------|
| 🎭 Dummy | Não é chamado | Lança exceção |
| 📋 Stub | Dados fixos | Respostas simples |
| 🕵️ Spy | Verificar uso | Registra chamadas |
| ✅ Mock | Expectativas | Verifica contratos |
| 🔧 Fake | Funcionalidade | Implementação real |

### Interface Assíncrona

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

### Como executar

```bash
cd src/fase-09-dubles-avancados
dotnet run
```

### Testes: 8/8 assíncronos passaram ✅

### Operações Paralelas

**Sequencial:** ~90ms (3 × 30ms)  
**Paralelo:** ~30ms (Task.WhenAll)

```csharp
var task1 = service.FindByIdAsync(1);
var task2 = service.FindByIdAsync(2);
var task3 = service.FindByIdAsync(3);
await Task.WhenAll(task1, task2, task3);
```

### Benefícios

- Simula I/O real (BD, APIs, arquivos)
- Testa código assíncrono (async/await)
- Testes paralelos (mais rápidos)
- 5 tipos de dublês especializados
- Spy registra ordem e quantidade

### Checklist de Qualidade

- [x] Interface assíncrona (IAsyncRepository)
- [x] 5 tipos de dublês implementados
- [x] 8 testes assíncronos (todos passando)
- [x] Task.WhenAll para paralelismo
- [x] Fake com simulação de latência
- [x] Spy registra chamadas e ordem
- [x] **Código organizado em pastas**
- [x] Documentação completa em `DUBLES-AVANCADOS.md`

