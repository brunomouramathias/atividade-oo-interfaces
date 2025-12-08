# Atividade por Fases — Interfaces em C#

Projeto desenvolvido para o estudo de Interfaces e Padrões de Projeto em C#.

## Equipe

- Bruno Moura Mathias Fernandes Simão
- Eduardo Mendes
- João Pedro Domingues

## Tema Escolhido

**Ordenação de Coleções**

Implementamos dois algoritmos de ordenação (Bubble Sort e Quick Sort) e aplicamos os conceitos de interfaces para permitir a troca de algoritmos sem modificar o código cliente.

**Política de seleção:**
- Listas pequenas (< 10 elementos): Bubble Sort
- Listas maiores: Quick Sort

## Estrutura do Projeto

```
src/
├── fase-02-procedural/         # Versão com switch
├── fase-03-interfaces/         # Primeira versão com interfaces
│   ├── Domain/Interfaces/      # Contrato IAlgoritmoOrdenacao
│   ├── Domain/Algorithms/      # BubbleSort, QuickSort, InsertionSort
│   └── Services/               # ServicoOrdenacao, CatalogoAlgoritmos
├── fase-04-plugavel-testavel/  # Testes com dublês
├── fase-05-repository-inmemory/  # Padrão Repository
├── fase-06-repository-csv/     # Persistência em CSV
├── fase-07-repository-json/    # Persistência em JSON
├── fase-08-isp/                # ISP (interfaces segregadas)
├── fase-09-dubles-avancados/   # Testes assíncronos
├── fase-10-code-smells/        # Refatorações
└── fase-11-mini-projeto/       # Consolidação final

tests/
├── Fase04PlugavelTestavel.Tests/
└── Fase06RepositoryCsv.Tests/
```

## Como Executar

```bash
# Executar o mini-projeto (fase 11)
dotnet run --project src/fase-11-mini-projeto/Fase11MiniProjeto.csproj

# Executar os testes
dotnet run --project tests/Fase04PlugavelTestavel.Tests/Fase04PlugavelTestavel.Tests.csproj
```

## Requisitos

- .NET 8.0 SDK

## Conceitos Aplicados

- **Interfaces**: Definem contratos sem implementação
- **Inversão de Dependência**: Cliente depende de abstração
- **Strategy Pattern**: Algoritmos intercambiáveis
- **Repository Pattern**: Abstração de acesso a dados
- **ISP**: Interfaces segregadas (leitura/escrita)
- **Testes com Dublês**: Fake, Stub, Spy, Mock
