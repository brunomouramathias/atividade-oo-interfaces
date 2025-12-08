# Relatório do Projeto - Interfaces em C#

## O que é este projeto?

Este projeto foi desenvolvido como atividade prática de **Programação Orientada a Objetos (POO)** focando no conceito de **Interfaces**.

O tema escolhido foi **Ordenação de Coleções**, onde implementamos dois algoritmos principais:
- **Bubble Sort** - para listas pequenas
- **Quick Sort** - para listas grandes

## Por que usamos Interfaces?

Interface é um **contrato** que define o que uma classe deve fazer, mas não como fazer. Isso permite:

1. **Trocar implementações sem mudar o código cliente**
   - Se tiver uma interface `IAlgoritmoOrdenacao`, posso usar Bubble Sort ou Quick Sort sem alterar quem chama

2. **Facilitar os testes**
   - Posso criar um "dublê" (fake) que simula o comportamento para testar sem executar o algoritmo real

3. **Desacoplar o código**
   - O serviço não precisa saber qual algoritmo específico está usando

## Estrutura do Projeto

### Pastas Domain/
Contém as **regras de negócio**:
- `Interfaces/` - Contratos (ex: `IAlgoritmoOrdenacao`, `IRepository`)
- `Algorithms/` - Implementações dos algoritmos
- `Entities/` - Entidades de dados (ex: `Book`)
- `Repositories/` - Implementações de acesso a dados

### Pastas Services/
Contém a **lógica de aplicação** que orquestra as entidades do domínio.

### Pastas Tests/
Contém os **testes** separados do código principal (seguindo boas práticas).

## O que cada fase demonstra?

| Fase | Conceito |
|------|----------|
| 02 | Código procedural com switch |
| 03 | Primeira versão com interfaces |
| 04 | Testes com dublês (fake) |
| 05 | Padrão Repository (InMemory) |
| 06 | Repository com CSV |
| 07 | Repository com JSON |
| 08 | ISP - Interface Segregation Principle |
| 09 | Dublês avançados (Stub, Spy, Mock) |
| 10 | Code Smells e Refatoração |
| 11 | Mini-projeto consolidando tudo |

## Perguntas que o professor pode fazer

### 1. O que é uma Interface?
É um contrato que define métodos que uma classe deve implementar, sem definir como.

### 2. Por que não usar classes abstratas?
Interfaces permitem herança múltipla de contratos. Uma classe pode implementar várias interfaces, mas só pode herdar de uma classe.

### 3. O que é Inversão de Dependência?
É depender de abstrações (interfaces) em vez de implementações concretas. O serviço não cria o algoritmo, recebe via construtor.

### 4. O que é Repository Pattern?
É uma abstração para acesso a dados. O código não sabe se está usando banco, arquivo ou memória.

### 5. O que é ISP?
Interface Segregation Principle: interfaces menores e específicas. Em vez de uma interface grande, temos `IReadRepository` e `IWriteRepository`.

### 6. Para que servem os dublês em testes?
Para isolar o código que queremos testar, simulando dependências sem executá-las de verdade.

### 7. Qual a diferença entre Fake, Stub, Spy e Mock?
- **Fake**: Implementação funcional simplificada (ex: repositório em memória)
- **Stub**: Retorna valores fixos
- **Spy**: Registra chamadas feitas
- **Mock**: Verifica se expectativas foram atendidas

## Como executar?

```bash
# Executar uma fase específica
cd src/fase-11-mini-projeto
dotnet run

# Executar testes
dotnet run --project tests/Fase04PlugavelTestavel.Tests/Fase04PlugavelTestavel.Tests.csproj
```

## Conclusão

Este projeto demonstra como interfaces permitem criar código:
- **Flexível** - trocar implementações facilmente
- **Testável** - usar dublês para isolar testes
- **Desacoplado** - módulos independentes
- **Organizado** - separação clara de responsabilidades
