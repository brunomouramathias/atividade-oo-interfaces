# Fase 1 - Heurística antes do código (Mapa Mental)

## Problema Escolhido: Codificação de Mensagem

**Objetivo:** Proteger o conteúdo de uma mensagem antes de transmiti-la ou armazená-la.

---

## 1. Versão Procedural (com if/switch)

Na abordagem procedural, usamos condicionais para decidir qual tipo de codificação aplicar:

```
função CodificarMensagem(texto, tipoCodificacao):
    se tipoCodificacao == "BASE64":
        retornar ConverterParaBase64(texto)
    senão se tipoCodificacao == "CESAR":
        retornar AplicarCifraDeCesar(texto, 3)
    senão se tipoCodificacao == "NENHUM":
        retornar texto
    senão:
        retornar texto

função ConverterParaBase64(texto):
    // lógica de conversão para Base64
    
função AplicarCifraDeCesar(texto, deslocamento):
    // desloca cada caractere em 3 posições
```

**Problemas identificados:**
- A cada novo método de codificação, precisamos modificar a função
- Lógica de decisão misturada com algoritmos de codificação
- Difícil de testar cada tipo de codificação isoladamente
- Se tivermos muitos métodos, a função fica muito grande e confusa

---

## 2. OO sem Interface (quem muda o quê)

Na abordagem orientada a objetos, criamos classes para cada tipo de codificação:

```
classe abstrata Codificador:
    método abstrato Codificar(texto)
    método abstrato Decodificar(texto)

classe CodificadorBase64 herda Codificador:
    método Codificar(texto):
        retornar ConverterParaBase64(texto)
    
    método Decodificar(texto):
        retornar ConverterDeBase64(texto)

classe CodificadorCesar herda Codificador:
    deslocamento = 3
    
    método Codificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere + deslocamento)
        retornar resultado
    
    método Decodificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere - deslocamento)
        retornar resultado

classe SemCodificacao herda Codificador:
    método Codificar(texto):
        retornar texto
    
    método Decodificar(texto):
        retornar texto
```

**O que muda:**
- Cada método de codificação vira uma classe própria
- Não precisa mais de if/switch
- Para adicionar nova codificação, crio uma nova classe
- Mas ainda existe acoplamento à hierarquia de classes

**Quem muda o quê:**
- Para adicionar codificação: criar nova classe que herda de Codificador
- Cliente escolhe qual codificador usar na criação do objeto

---

## 3. Com Interface (qual contrato permite alternar)

Agora usamos interface para definir o contrato:

```
interface ICodificador:
    método Codificar(texto)
    método Decodificar(texto)

classe CodificadorBase64 implementa ICodificador:
    método Codificar(texto):
        retornar ConverterParaBase64(texto)
    
    método Decodificar(texto):
        retornar ConverterDeBase64(texto)

classe CodificadorCesar implementa ICodificador:
    deslocamento = 3
    
    método Codificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere + deslocamento)
        retornar resultado
    
    método Decodificar(texto):
        resultado = ""
        para cada caractere em texto:
            resultado += (caractere - deslocamento)
        retornar resultado

classe SemCodificacao implementa ICodificador:
    método Codificar(texto):
        retornar texto
    
    método Decodificar(texto):
        retornar texto
```

**Cliente usando o contrato:**
```
classe GerenciadorMensagens:
    codificador: ICodificador
    
    construtor(codificador):
        this.codificador = codificador
    
    método EnviarMensagem(textoOriginal):
        textoProtegido = codificador.Codificar(textoOriginal)
        // envia a mensagem protegida
        
    método ReceberMensagem(textoProtegido):
        textoOriginal = codificador.Decodificar(textoProtegido)
        retornar textoOriginal
```

**Contrato ICodificador permite:**
- Trocar método de codificação sem modificar o GerenciadorMensagens
- Testar GerenciadorMensagens com codificador falso (stub)
- Adicionar novos métodos de codificação sem alterar código existente
- Cliente depende só do contrato, não da implementação

---

## 3 Sinais de Alerta Previstos

1. **Switch/if por tipo espalhado no código**
   - Se aparecer vários if/switch verificando tipo de codificação em lugares diferentes
   - Sinal: código repetido e difícil de manter, cada nova codificação exige mudança em vários pontos

2. **Classe com muitas responsabilidades**
   - Se uma classe codificar mensagem E validar formato E salvar em arquivo E enviar por rede
   - Sinal: violação do princípio da responsabilidade única (SRP)

3. **Implementações que não usam todos os métodos da interface**
   - Se alguma classe implementar ICodificador mas lançar exceção em Decodificar
   - Sinal: interface muito gorda, talvez precise segregar em IEncoder e IDecoder separados

