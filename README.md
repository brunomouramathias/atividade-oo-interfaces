# Tarefa por Fases — Interfaces em C#

## Composição da Equipe

- Bruno Moura Mathias Fernandes Simão
- Eduardo Mendes
- João Pedro Domingues

## Sumário das Fases

- [Fase 1 - Heurística antes do código (Mapa Mental)](#fase-1---heurística-antes-do-código-mapa-mental)

---

## Fase 1 - Heurística antes do código (Mapa Mental)

### Objetivo
Desenhar um mapa de evolução para um problema trivial, mostrando a transição de código procedural para orientado a objetos e finalmente usando interfaces.

### Como executar
Esta fase é apenas conceitual, não há código para executar.

### Documentação
O mapa mental completo está disponível em: `src/fase-01-heuristica/MAPA-MENTAL.md`

### Decisões de Design

- **Problema escolhido:** Calcular preço com desconto em compras
- **Contratos:** Interface `ICalculadoraDesconto` com método `Calcular(preco)`
- **Aplicação do ISP:** Interface pequena e coesa com apenas um método
- **Ponto de composição:** Cliente (Carrinho) recebe a calculadora no construtor
- **Alternância:** Possível trocar tipo de desconto sem modificar o cliente

### Checklist de Qualidade

- [x] Contrato coeso (único método bem definido)
- [x] Cliente não conhece implementações concretas
- [x] Novos descontos podem ser adicionados sem modificar código existente
- [x] Ausência de switch/if espalhados após uso de interfaces
- [x] Cada classe tem uma única responsabilidade

### Sinais de Alerta Identificados

1. Switch/if por tipo espalhado no código
2. Classe com muitas responsabilidades
3. Implementações que não usam todos os métodos da interface

