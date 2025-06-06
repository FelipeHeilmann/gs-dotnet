# Integrantes
RM 551026 - Felipe Heilmann Marques
RM 552159 - Carlos Eduardo Caramante Ribeiro


# Sistema de Registro e Mitigação de Falhas de Energia

Este é um sistema de linha de comando (CLI) desenvolvido em C# com foco na gestão de falhas de energia, incluindo funcionalidades para registrar falhas, gerar relatórios, acompanhar planos de ação e manter a rastreabilidade das ações do sistema.

## 🎯 Funcionalidades

1. **Login com Autenticação Fixa**
   - Usuário fixo: `rm551026`
   - Senha: `admin1234!`
   - Apenas usuários autenticados podem acessar o sistema.

2. **Registro de Falhas de Energia**
   - Campos: data e hora de início/fim, local, impacto, severidade.
   - Validação de campos obrigatórios.
   - Armazenamento local (em memória ou arquivo).

3. **Geração de Alertas**
   - Se a severidade da falha for alta, um alerta é gerado automaticamente.
   - Pode ser simulado manualmente.

4. **Logs de Eventos**
   - Todas as ações (login, registro de falha, alteração de plano) são registradas com data/hora e usuário.
   - Logs acessíveis para auditoria.

5. **Relatórios de Status**
   - Relatório em TXT contendo:
     - Total de falhas.
     - Número de falhas críticas.
     - Locais mais afetados.
     - Duração média.
   - Filtros opcionais por data, local e severidade.

6. **Plano de Ação para Mitigação**
   - Para cada falha, pode ser associado um plano de ação.
   - Campos: descrição, status (Pendente, Em Andamento, Concluído).
   - Permite cadastrar e atualizar o status do plano.

7. **Atualização de Status do Plano de Ação**
   - Interface CLI para atualizar o status de um plano de ação existente.
   - Lista todos os planos disponíveis com seu status atual.

## ▶️ Como Executar

1. Abra o terminal.
2. Compile o projeto com `dotnet build`.
3. Execute com `dotnet run`.
4. Autentique-se com o login fixo.
5. Utilize o menu interativo para navegar entre as funcionalidades.

## 🛠 Estrutura do Projeto

- `GS.Domain`: Entidades e enums do domínio (FalhaEnergia, PlanoAcao, StatusPlanoAcao).
- `GS.Infra.DAO`: Camada de acesso a dados (em memória).
- `GS.Application.Service`: Regras de negócio.
- `Program.cs`: Entrada principal e interface CLI.

## ✅ Requisitos

- .NET 8 ou superior
- Terminal (cmd, bash, PowerShell, etc.)

## 🛠 Estrutura do Projeto

- `GS.Application`  
  Camada de aplicação responsável pela lógica de negócio.  
  - **Services**: Serviços que implementam regras e operações do sistema, orquestrando chamadas entre domínio e dados.  
  - **Models**: Modelos ou DTOs usados para transferência de dados entre camadas.  
  - **Interfaces DAO**: Contratos que definem a abstração para acesso a dados, permitindo flexibilidade na implementação.

- `GS.Infra`  
  Implementação concreta dos DAOs (Data Access Objects), responsável pelo armazenamento dos dados. Atualmente utiliza uma implementação em memória para persistência temporária durante a execução.

- `Console`  
  Aplicação de interface de linha de comando (CLI), contendo a classe `Program.cs` que é o ponto de entrada do sistema. Gerencia a interação com o usuário, exibição de menus e captura de dados via terminal.

