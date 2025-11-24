# Fase 7 — Repository JSON

## Objetivo

Evoluir o Repository da Fase 6 para **persistir em arquivo JSON** usando `System.Text.Json`, mantendo o mesmo contrato `IRepository<T, TId>` mas agora com formato estruturado e legível.

**Ideia-chave:** Os dados persistem em arquivo `.json` com formatação indentada, usando o serializer nativo do .NET (System.Text.Json).

---

## Estrutura Organizada do Projeto

```
fase-07-repository-json/
├── Domain/
│   └── Book.cs                    # Modelo de domínio
├── Repository/
│   ├── IRepository.cs             # Interface genérica
│   └── JsonBookRepository.cs      # Implementação JSON
├── Services/
│   └── BookService.cs             # Serviço de domínio
├── Tests/
│   └── TestesRepositorioJson.cs   # Testes de integração
├── Program.cs                      # Demonstração
├── Fase07RepositoryJson.csproj    # Projeto
├── .gitignore                      # Ignora arquivos JSON
└── REPOSITORY-JSON.md              # Esta documentação
```

---

## Arquitetura

```
BookService (Cliente)
    ↓
IRepository<Book, int> (Contrato - REUTILIZADO)
    ↓
JsonBookRepository (Implementação JSON)
    ↓
books.json (Arquivo em Disco)
```

---

## 1. Formato JSON

### Estrutura do Arquivo

```json
[
  {
    "id": 1,
    "title": "Código Limpo",
    "author": "Robert C. Martin"
  },
  {
    "id": 2,
    "title": "Domain-Driven Design",
    "author": "Eric Evans"
  },
  {
    "id": 10,
    "title": "Livro com \"aspas\" e /barras/",
    "author": "Autor: João & Maria"
  }
]
```

### Características do JSON

1. **Formatação:** Indentação automática (WriteIndented = true)
2. **Naming:** camelCase nas propriedades (PropertyNamingPolicy = CamelCase)
3. **Encoding:** UTF-8 por padrão
4. **Escape:** Automático pelo System.Text.Json (aspas, barras, caracteres especiais)
5. **Ordenação:** Lista ordenada por ID

---

## 2. Implementação JsonBookRepository

### Configuração do Serializer

```csharp
_options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

### Load() - Deserializa do arquivo

```csharp
private List<Book> Load()
{
    if (!File.Exists(_path))
        return new List<Book>();
    
    try
    {
        var json = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(json))
            return new List<Book>();
        
        var books = JsonSerializer.Deserialize<List<Book>>(json, _options);
        return books ?? new List<Book>();
    }
    catch
    {
        return new List<Book>();
    }
}
```

### Save() - Serializa no arquivo

```csharp
private void Save(List<Book> books)
{
    var sortedBooks = books.OrderBy(b => b.Id).ToList();
    var json = JsonSerializer.Serialize(sortedBooks, _options);
    File.WriteAllText(_path, json);
}
```

---

## Como executar

```bash
cd src/fase-07-repository-json
dotnet run
```

O programa executa:
1. **10 testes automatizados** com arquivos temporários
2. **6 cenários de uso** com persistência real
3. Mostra o conteúdo do arquivo JSON gerado

---

## Testes Implementados (10 testes)

### Testes de Operações Básicas

1. ✅ **ListAll_WhenFileDoesNotExist_ShouldReturnEmpty**
   - Arquivo ausente retorna lista vazia

2. ✅ **Add_Then_ListAll_ShouldPersistInFile**
   - Dados persistem após Add

3. ✅ **GetById_Existing_ShouldReturnBook**
   - Busca por ID existente funciona

4. ✅ **GetById_Missing_ShouldReturnNull**
   - Busca por ID ausente retorna null

5. ✅ **Update_Existing_ShouldPersistChanges**
   - Update persiste mudanças

6. ✅ **Update_Missing_ShouldReturnFalse**
   - Update de inexistente retorna false

7. ✅ **Remove_Existing_ShouldDeleteFromFile**
   - Remove apaga do arquivo

8. ✅ **Remove_Missing_ShouldReturnFalse**
   - Remove de inexistente retorna false

### Testes de Casos Especiais

9. ✅ **Add_WithSpecialCharacters_ShouldHandleCorrectly**
   - Caracteres especiais (aspas, barras, símbolos) tratados corretamente

10. ✅ **MultipleOperations_ShouldPersist**
    - Múltiplas operações persistem corretamente

---

## Cenários Demonstrados

### Cenário 1: Cadastro de Livros
Cadastra 3 livros e persiste em JSON

### Cenário 2: Listagem
Lista todos os livros do arquivo

### Cenário 3: Caracteres Especiais
Cadastra livro com aspas, barras e símbolos especiais

### Cenário 4: Atualização
Atualiza título de um livro existente

### Cenário 5: Remoção
Remove um livro e verifica persistência

### Cenário 6: Persistência
Demonstra que dados sobrevivem entre execuções

---

## Diferenças: CSV vs JSON

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

---

## Vantagens da Implementação JSON

### 1. Serialização Automática
- System.Text.Json cuida de tudo
- Sem parse manual
- Menos código, menos bugs

### 2. Formato Estruturado
- JSON é padrão universal
- Fácil integração com APIs
- Suporte nativo em navegadores

### 3. Escape Automático
- Não precisa tratar aspas, vírgulas manualmente
- Serializer cuida de caracteres especiais

### 4. Tipagem Forte
- Deserialização direta para objetos
- IntelliSense e type-safety

### 5. Legibilidade Superior
- Formatação indentada
- Estrutura clara
- Fácil edição manual

---

## Limitações Conhecidas

⚠️ **Esta implementação é didática e tem limitações:**

1. **Sem tratamento de concorrência**
   - Múltiplos processos podem corromper arquivo

2. **Performance em listas grandes**
   - Carrega/salva arquivo inteiro a cada operação

3. **Sem versionamento**
   - Mudança no esquema quebra arquivos antigos

4. **Sem transações**
   - Falha no meio da gravação corrompe arquivo

**Em projetos reais:** Usar bibliotecas especializadas ou banco de dados.

---

## Comparação com Fases Anteriores

| Aspecto | Fase 5 (InMemory) | Fase 6 (CSV) | Fase 7 (JSON) |
|---------|-------------------|--------------|---------------|
| Persistência | ❌ | ✅ | ✅ |
| Legibilidade | N/A | Boa | Excelente |
| Escape | N/A | Manual | Automático |
| Performance | Muito rápida | Lenta (I/O) | Lenta (I/O) |
| Complexidade | Baixa | Média | Baixa |
| Padrão Web | N/A | ❌ | ✅ |

---

## Próximas Evoluções

A arquitetura Repository está pronta para:
- **Fase 8:** Repository com Banco de Dados (SQL)
- **Fase 9:** Repository com NoSQL

**Facilidade:** Basta criar nova implementação de `IRepository<T, TId>`, o cliente não muda!

---

## Princípios Aplicados

### Open/Closed Principle
- Aberto para novas implementações (BD, NoSQL)
- Cliente fechado (não muda ao trocar implementação)

### Dependency Inversion
- Cliente depende de `IRepository<T, TId>`
- Não conhece `JsonBookRepository`

### Single Responsibility
- Repository: persistência
- Service: regras de negócio
- Tests: validação

---

## Por que System.Text.Json?

1. **Nativo do .NET**
   - Sem dependências externas
   - Integrado ao framework

2. **Performance**
   - Otimizado para .NET Core/.NET 5+
   - Alocação mínima de memória

3. **Segurança**
   - Mantido pela Microsoft
   - Atualizações de segurança regulares

4. **Simplicidade**
   - API direta e clara
   - Configurações intuitivas

---

## Organização em Pastas

✅ **Vantagens da estrutura organizada:**

- **Domain/** - Modelos de domínio isolados
- **Repository/** - Lógica de persistência separada
- **Services/** - Regras de negócio organizadas
- **Tests/** - Testes separados do código principal

**Resultado:** Código limpo e profissional, fácil de navegar!

