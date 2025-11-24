# Fase 10 — Code Smells e Refatorações (Diffs Pequenos)

## Objetivo

Identificar **code smells** comuns e aplicar **refatorações** com diffs pequenos, demonstrando melhorias incrementais no código mantendo funcionalidade.

**Ideia-chave:** Melhorar código através de pequenas mudanças seguras e testáveis.

---

## Code Smells Demonstrados

### 1. Long Method (Método Longo)
- **Smell:** Método faz muitas coisas
- **Refatoração:** Extract Method

### 2. Duplicate Code (Código Duplicado)
- **Smell:** Mesma lógica repetida
- **Refatoração:** Extract Method/Class

### 3. Large Class (Classe Grande)
- **Smell:** Classe com muitas responsabilidades
- **Refatoração:** Split Class (SRP)

### 4. Long Parameter List (Lista Longa de Parâmetros)
- **Smell:** Método com muitos parâmetros
- **Refatoração:** Introduce Parameter Object

### 5. Data Clumps (Agrupamentos de Dados)
- **Smell:** Dados sempre aparecem juntos
- **Refatoração:** Extract Class

---

## Estrutura do Projeto

```
fase-10-code-smells/
├── Domain/
│   ├── Entities/
│   │   ├── Product.cs
│   │   └── ProductDetails.cs
│   ├── Interfaces/
│   │   └── IProductRepository.cs
│   └── Repositories/
│       └── ProductRepository.cs
├── Services/
│   ├── BeforeRefactoring/
│   │   └── ProductServiceBefore.cs
│   └── AfterRefactoring/
│       └── ProductServiceAfter.cs
├── Tests/
│   └── RefactoringTests.cs
├── Program.cs
└── CODE-SMELLS.md
```

---

## Exemplo 1: Long Method → Extract Method

**ANTES (Smell):**
```csharp
public decimal CalculateTotal(Order order)
{
    decimal total = 0;
    foreach (var item in order.Items)
    {
        total += item.Price * item.Quantity;
    }
    
    decimal discount = 0;
    if (order.CustomerType == "VIP")
        discount = total * 0.1m;
    else if (order.CustomerType == "Regular")
        discount = total * 0.05m;
    
    decimal tax = (total - discount) * 0.15m;
    
    return total - discount + tax;
}
```

**DEPOIS (Refatorado):**
```csharp
public decimal CalculateTotal(Order order)
{
    decimal subtotal = CalculateSubtotal(order);
    decimal discount = CalculateDiscount(subtotal, order.CustomerType);
    decimal tax = CalculateTax(subtotal - discount);
    
    return subtotal - discount + tax;
}

private decimal CalculateSubtotal(Order order)
{
    return order.Items.Sum(item => item.Price * item.Quantity);
}

private decimal CalculateDiscount(decimal subtotal, string customerType)
{
    return customerType switch
    {
        "VIP" => subtotal * 0.1m,
        "Regular" => subtotal * 0.05m,
        _ => 0
    };
}

private decimal CalculateTax(decimal taxableAmount)
{
    return taxableAmount * 0.15m;
}
```

**Benefícios:**
- ✅ Métodos pequenos e focados
- ✅ Nomes descritivos
- ✅ Fácil de testar individualmente
- ✅ Fácil de entender

---

## Exemplo 2: Long Parameter List → Parameter Object

**ANTES (Smell):**
```csharp
public Product CreateProduct(
    string name, 
    string description, 
    decimal price, 
    int quantity, 
    string category, 
    string supplier, 
    DateTime expiryDate)
{
    // Muitos parâmetros dificulta uso e manutenção
}
```

**DEPOIS (Refatorado):**
```csharp
public sealed record ProductDetails(
    string Name,
    string Description,
    decimal Price,
    int Quantity,
    string Category,
    string Supplier,
    DateTime ExpiryDate
);

public Product CreateProduct(ProductDetails details)
{
    // Apenas 1 parâmetro, mais fácil de usar
}
```

**Benefícios:**
- ✅ Menos parâmetros
- ✅ Dados agrupados logicamente
- ✅ Reutilizável

---

## Exemplo 3: Duplicate Code → Extract Method

**ANTES (Smell):**
```csharp
public class ReportService
{
    public void GenerateSalesReport()
    {
        Console.WriteLine("===================");
        Console.WriteLine("Sales Report");
        Console.WriteLine("===================");
        // lógica
    }
    
    public void GenerateInventoryReport()
    {
        Console.WriteLine("===================");
        Console.WriteLine("Inventory Report");
        Console.WriteLine("===================");
        // lógica
    }
}
```

**DEPOIS (Refatorado):**
```csharp
public class ReportService
{
    public void GenerateSalesReport()
    {
        PrintHeader("Sales Report");
        // lógica
    }
    
    public void GenerateInventoryReport()
    {
        PrintHeader("Inventory Report");
        // lógica
    }
    
    private void PrintHeader(string title)
    {
        Console.WriteLine("===================");
        Console.WriteLine(title);
        Console.WriteLine("===================");
    }
}
```

---

## Boas Práticas de Refatoração

### 1. Refatore em Pequenos Passos
- Uma mudança por vez
- Teste após cada mudança
- Commit frequente

### 2. Mantenha Testes Passando
- Rode testes antes
- Rode testes depois
- Se quebrar, reverta

### 3. Use Ferramentas
- IDE com refactoring automático
- Code analysis
- Git para reverter se necessário

### 4. Priorize Legibilidade
- Nomes descritivos
- Métodos pequenos
- Responsabilidade única

---

## Checklist de Refatoração

Antes de refatorar:
- [ ] Testes existem e passam?
- [ ] Entendo o código?
- [ ] Sei o que quero melhorar?

Durante a refatoração:
- [ ] Mudança pequena?
- [ ] Testes ainda passam?
- [ ] Código ficou mais claro?

Depois da refatoração:
- [ ] Todos os testes passam?
- [ ] Código está melhor?
- [ ] Commit feito?

---

## Conclusão

Refatoração não é reescrita completa - são melhorias incrementais e seguras que tornam o código mais limpo e manutenível.

**Lembre-se:** Código limpo não acontece de uma vez, é um processo contínuo!

