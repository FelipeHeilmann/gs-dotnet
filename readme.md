# Integrantes
- RM 551026 Felipe Heilmann Marques
- RM 552159 - Carlos Eduardo Caramante Ribeiro


# Sistema de Registro e Mitigação de Falhas de Energia

Este é um sistema de linha de comando (CLI) desenvolvido em C# com foco na gestão de falhas de energia, incluindo funcionalidades para registrar falhas, gerar relatórios, acompanhar planos de ação e manter a rastreabilidade das ações do sistema.

## 🎯 Funcionalidades

1. **Registro de Falhas de Energia**
   - Campos: data e hora de início/fim, local, impacto, severidade.
   - Validação de campos obrigatórios.
   - Armazenamento local (em memória ou arquivo).

2. **Geração de Alertas**
   - Se a severidade da falha for alta, um alerta é gerado automaticamente.
   - Pode ser simulado manualmente.

3. **Relatórios de Status**
   - Relatório em TXT contendo:
     - Total de falhas.
     - Número de falhas críticas.
     - Locais mais afetados.
     - Duração média.
   - Filtros opcionais por data, local e severidade.

4. **Plano de Ação para Mitigação**
   - Para cada falha, pode ser associado um plano de ação.
   - Campos: descrição, status (Pendente, Em Andamento, Concluído).
   - Permite cadastrar e atualizar o status do plano.

5. **Atualização de Status do Plano de Ação**
   - Interface CLI para atualizar o status de um plano de ação existente.
   - Lista todos os planos disponíveis com seu status atual.

## ▶️ Como Executar

1. Abra o terminal.
2. Compile o projeto com `dotnet build`.
3. Execute com `dotnet run`.
4. Autentique-se com o login fixo.
5. Utilize o menu interativo para navegar entre as funcionalidades.

## 🛠 Estrutura do Projeto

- `GS.Infra.DAO`: Camada de acesso a dados (em memória).
- `GS.Application`: Regras de negócio, interfaces e modelos.
- `GS.Console`: Entrada principal e interface CLI.

## ✅ Requisitos

- .NET 8 ou superior
- Terminal (cmd, bash, PowerShell, etc.)
