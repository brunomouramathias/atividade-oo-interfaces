# Fase 2 — Procedural Mínimo

## Implementação

Foram implementados dois exemplos com abordagem procedural usando `switch`:

### 1. Codificação de Mensagem
**Modos:** `base64`, `cesar`, `invertido`, `padrão`

### 2. Ordenação de Lista
**Modos:** `bubble`, `quick`, `insertion`, `padrão`

---

## 5 Cenários de Teste/Fronteira

### Exemplo 1: Codificação de Mensagem

#### Cenário 1: Texto simples com Base64
**Entrada:** `"Hello"`, modo `"base64"`  
**Saída esperada:** `"SGVsbG8="`  
**Resultado:** Texto codificado em Base64 válido

#### Cenário 2: Texto com caracteres especiais usando Cifra de César
**Entrada:** `"ABC xyz 123"`, modo `"cesar"`  
**Saída esperada:** `"DEF abc 123"` (apenas letras deslocadas, números intactos)  
**Resultado:** Apenas letras são deslocadas, espaços e números permanecem

#### Cenário 3: Texto vazio
**Entrada:** `""`, modo `"invertido"`  
**Saída esperada:** `""`  
**Resultado:** String vazia retorna string vazia

#### Cenário 4: Modo não reconhecido (padrão)
**Entrada:** `"Test"`, modo `"xyz"`  
**Saída esperada:** `"Test"` (retorna o original)  
**Resultado:** Texto não modificado quando modo é inválido

#### Cenário 5: Texto longo (fronteira de performance)
**Entrada:** String com 1000 caracteres, modo `"base64"`  
**Saída esperada:** String codificada válida  
**Resultado:** Funciona, mas sem otimização específica para textos grandes

---

### Exemplo 2: Ordenação de Lista

#### Cenário 1: Lista pequena com Bubble Sort
**Entrada:** `[5, 2, 8, 1]`, modo `"bubble"`  
**Saída esperada:** `[1, 2, 5, 8]`  
**Resultado:** Lista ordenada corretamente

#### Cenário 2: Lista já ordenada com Quick Sort
**Entrada:** `[1, 2, 3, 4, 5]`, modo `"quick"`  
**Saída esperada:** `[1, 2, 3, 4, 5]`  
**Resultado:** Quick Sort funciona, mas não é o mais eficiente para dados já ordenados

#### Cenário 3: Lista com elementos duplicados
**Entrada:** `[3, 1, 2, 1, 3]`, modo `"insertion"`  
**Saída esperada:** `[1, 1, 2, 3, 3]`  
**Resultado:** Duplicatas são preservadas e ordenadas corretamente

#### Cenário 4: Lista vazia
**Entrada:** `[]`, modo `"bubble"`  
**Saída esperada:** `[]`  
**Resultado:** Lista vazia permanece vazia

#### Cenário 5: Lista grande (fronteira de performance)
**Entrada:** 1000 elementos aleatórios, modo `"quick"`  
**Saída esperada:** Lista ordenada  
**Resultado:** Quick Sort é eficiente, mas Bubble Sort seria muito lento

---

## Por que essa abordagem não escala

### Problema 1: Adição de novos modos
Cada novo modo de codificação (AES, ROT13, SHA256) ou algoritmo de ordenação (Merge Sort, Heap Sort) exige:
- Adicionar um novo `case` no `switch`
- Implementar a função correspondente
- Modificar a função central `CodificarMensagem` ou `OrdenarLista`

**Impacto:** Violação do princípio Open/Closed (aberto para extensão, fechado para modificação)

### Problema 2: Lógica de decisão acoplada
A decisão de "qual algoritmo usar" está **misturada** com a execução. Se precisarmos:
- Escolher algoritmo baseado em tamanho da lista (não só modo)
- Aplicar diferentes políticas (performance vs memória)
- Combinar múltiplos modos

**Impacto:** Código fica cada vez mais complexo e difícil de manter

### Problema 3: Testes isolados difíceis
Para testar apenas o algoritmo de Cifra de César, precisamos:
- Chamar a função central `CodificarMensagem`
- Passar o modo correto como string
- Não há como testar o algoritmo isoladamente

**Impacto:** Testes acoplados, difícil criar dublês (stubs/mocks)

### Problema 4: Ramificações espalhadas
Se precisarmos verificar o modo em múltiplos lugares (validação, logging, auditoria):
- O `switch` se repete em vários pontos
- Mudança no nome de um modo exige alterar todos os lugares
- Risco de inconsistência

**Impacto:** Manutenção cara e propensa a erros

### Problema 5: Strings "mágicas"
Os modos são strings literais (`"base64"`, `"cesar"`):
- Sem verificação em tempo de compilação
- Erros de digitação só aparecem em runtime
- IDE não oferece autocompletar

**Impacto:** Erros silenciosos e difíceis de detectar

---

## Conclusão

A abordagem procedural com `switch` é **simples** para poucos casos, mas **não escala** quando:
- O número de variações cresce
- Há necessidade de políticas complexas
- Testes isolados e manutenção se tornam críticos

**Próximo passo (Fase 3):** Aplicar Orientação a Objetos com classes específicas para cada modo, eliminando o `switch` central.

