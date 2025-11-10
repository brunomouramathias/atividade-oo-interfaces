# Fase 0 - Aquecimento Conceitual: Contratos de Capacidade

## Caso 1 - Codificação de Mensagem

**Objetivo:** Proteger o conteúdo de uma mensagem antes de transmiti-la ou armazená-la.

**Contrato:** "Transformar o texto original em uma versão criptografada".

**Implementação A:** Codificar o texto em Base64.

**Implementação B:** Aplicar cifra de César com 3 caracteres de deslocamento.

**Política:** Para mensagens de sistema interno ou logs, usar Base64 (rápido e reversível). Para mensagens com dados sensíveis que precisam de proteção básica, usar cifra de César.

**Risco/Observação:** Base64 não é criptografia real, apenas codificação (facilmente reversível). Cifra de César é vulnerável a ataques simples de força bruta. Ambos servem para ofuscação básica, não para segurança crítica.

---

## Caso 2 - Compressão de Arquivos

**Objetivo:** Reduzir o tamanho de arquivos para economizar espaço de armazenamento ou acelerar transmissão.

**Contrato:** "Comprimir dados mantendo integridade para posterior descompressão".

**Implementação A:** Comprimir usando ZIP (para múltiplos arquivos ou pastas).

**Implementação B:** Comprimir usando GZIP (para arquivo único ou stream de dados).

**Política:** Se houver múltiplos arquivos ou estrutura de pastas, usar ZIP. Se for um único arquivo grande ou dados em streaming (como logs ou backups), usar GZIP.

**Risco/Observação:** ZIP adiciona overhead para gerenciar múltiplos arquivos (maior que GZIP para arquivos únicos). GZIP não suporta múltiplos arquivos nativamente sem empacotamento prévio (como tar). Ambos têm custo de CPU para compressão/descompressão.

