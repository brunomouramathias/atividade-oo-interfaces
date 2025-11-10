# Fase 1 - Heurística antes do código (Mapa Mental)

## Problema Escolhido: Calcular Preço com Desconto

**Objetivo:** Calcular o preço final de um produto aplicando diferentes tipos de desconto.

---

## 1. Versão Procedural (com if/switch)

Na abordagem procedural, usamos condicionais para decidir qual tipo de desconto aplicar:

```
função CalcularPrecoFinal(preco, tipoDesconto):
    se tipoDesconto == "CLIENTE_VIP":
        retornar preco * 0.80  // 20% de desconto
    senão se tipoDesconto == "CUPOM10":
        retornar preco * 0.90  // 10% de desconto
    senão se tipoDesconto == "BLACK_FRIDAY":
        retornar preco * 0.50  // 50% de desconto
    senão:
        retornar preco  // sem desconto
```

**Problemas identificados:**
- A cada novo tipo de desconto, precisamos modificar a função
- Lógica de decisão misturada com cálculos
- Difícil de testar cada tipo de desconto isoladamente
- Se tivermos muitos tipos de desconto, a função fica muito grande

---

## 2. OO sem Interface (quem muda o quê)

Na abordagem orientada a objetos, criamos classes para cada tipo de desconto:

```
classe abstrata CalculadoraDesconto:
    método abstrato Calcular(preco)

classe DescontoVip herda CalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.80

classe DescontoCupom herda CalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.90

classe DescontoBlackFriday herda CalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.50

classe SemDesconto herda CalculadoraDesconto:
    método Calcular(preco):
        retornar preco
```

**O que muda:**
- Cada tipo de desconto vira uma classe própria
- Não precisa mais de if/switch
- Para adicionar novo desconto, crio uma nova classe
- Mas ainda existe acoplamento à hierarquia de classes

**Quem muda o quê:**
- Para adicionar desconto: criar nova classe que herda de CalculadoraDesconto
- Cliente escolhe qual calculadora usar na criação do objeto

---

## 3. Com Interface (qual contrato permite alternar)

Agora usamos interface para definir o contrato:

```
interface ICalculadoraDesconto:
    método Calcular(preco)

classe DescontoVip implementa ICalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.80

classe DescontoCupom implementa ICalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.90

classe DescontoBlackFriday implementa ICalculadoraDesconto:
    método Calcular(preco):
        retornar preco * 0.50

classe SemDesconto implementa ICalculadoraDesconto:
    método Calcular(preco):
        retornar preco
```

**Cliente usando o contrato:**
```
classe Carrinho:
    calculadora: ICalculadoraDesconto
    
    construtor(calculadora):
        this.calculadora = calculadora
    
    método ObterTotal(preco):
        retornar calculadora.Calcular(preco)
```

**Contrato ICalculadoraDesconto permite:**
- Trocar tipo de desconto sem modificar o Carrinho
- Testar Carrinho com desconto falso (stub)
- Adicionar novos descontos sem alterar código existente
- Cliente depende só do contrato, não da implementação

---

## 3 Sinais de Alerta Previstos

1. **Switch/if por tipo espalhado no código**
   - Se aparecer vários if/switch verificando tipo de desconto em lugares diferentes
   - Sinal: código repetido e difícil de manter

2. **Classe com muitas responsabilidades**
   - Se uma classe calcular desconto E validar cupom E consultar banco de dados
   - Sinal: violação do princípio da responsabilidade única

3. **Implementações que não usam todos os métodos da interface**
   - Se alguma classe implementar a interface mas não usar algum método
   - Sinal: interface muito gorda, precisa segregar em contratos menores

