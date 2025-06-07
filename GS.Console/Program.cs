using GS.Application.Enums;
using GS.Application.Service;
using GS.Infra.DAO;

var dao = new FalhaEnergiaDAO();
var falhaEnergiaService = new FalhaEnergiaService(dao);

var usuarioService = new UsuarioService();
bool autenticado = false;

while (!autenticado)
{
    Console.Clear();
    Console.WriteLine("=== LOGIN ===");
    Console.Write("Usuário: ");
    var usuario = Console.ReadLine();

    Console.Write("Senha: ");
    var senha = Console.ReadLine();

    if (usuarioService.Autenticar(usuario, senha))
    {
        Console.WriteLine("Login bem-sucedido!");
        autenticado = true;
    }
    else
    {
        Console.WriteLine("\n❌ Usuário ou senha inválidos. Pressione qualquer tecla para tentar novamente.");
        Console.ReadKey();
    }
}

while (true)
{
    Console.Clear();
    Console.WriteLine("=== REGISTRO DE FALHAS DE ENERGIA ===");
    Console.WriteLine("1. Registrar nova falha");
    Console.WriteLine("2. Listar falhas registradas");
    Console.WriteLine("3. Gerar relatório de falhas");
    Console.WriteLine("4. Registrar plano de ação para falha");
    Console.WriteLine("5. Atualizar status de plano de ação");
    Console.WriteLine("0. Sair");
    Console.Write("Escolha uma opção: ");
    var opcao = Console.ReadLine();

    try
    {
        switch (opcao)
        {
            case "1":
                RegistrarFalha(falhaEnergiaService);
                break;

            case "2":
                ListarFalhas(falhaEnergiaService);
                break;

            case "3":
                Console.Write("Caminho do arquivo para salvar o relatório: ");
                var caminhoArquivo = Console.ReadLine();
                falhaEnergiaService.GerarRelatorio(caminhoArquivo);
                Console.WriteLine("✅ Relatório gerado com sucesso!");
                break;

            case "4":
                RegistrarPlanoAcao(falhaEnergiaService, dao);
                break;

            case "5":
                AtualizarStatusPlanoAcao(falhaEnergiaService);
                break;

            case "0":
                return;

            default:
                Console.WriteLine("⚠️ Opção inválida. Tente novamente.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro: {ex.Message}");
    }

    Console.WriteLine("\nPressione qualquer tecla para continuar...");
    Console.ReadKey();
}

static void RegistrarFalha(FalhaEnergiaService servico)
{
    try
    {
        Console.Write("Data e hora de início (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime inicio))
        {
            Console.WriteLine("⚠️ Data de início inválida.");
            return;
        }

        Console.Write("Data e hora de fim (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime fim))
        {
            Console.WriteLine("⚠️ Data de fim inválida.");
            return;
        }

        Console.Write("Local da falha: ");
        var local = Console.ReadLine();

        Console.Write("Descrição do impacto: ");
        var impacto = Console.ReadLine();

        Console.Write("Severidade (1 a 5): ");
        if (!int.TryParse(Console.ReadLine(), out int severidade) || severidade < 1 || severidade > 5)
        {
            Console.WriteLine("⚠️ Severidade deve estar entre 1 e 5.");
            return;
        }

        servico.RegistrarFalha(inicio, fim, local, impacto, severidade);
        Console.WriteLine("✅ Falha registrada com sucesso!");

        if (severidade == 5)
        {
            Console.WriteLine("\n⚠️ Severidade alta detectada (5).");
            Console.Write("Informe o diretório onde deseja salvar o relatório: ");
            var diretorio = Console.ReadLine();

            Console.Write("Informe o nome do arquivo (ex: relatorio.txt): ");
            var nomeArquivo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(diretorio) || string.IsNullOrWhiteSpace(nomeArquivo))
            {
                Console.WriteLine("⚠️ Diretório ou nome do arquivo inválido. Relatório não gerado.");
                return;
            }

            var caminhoCompleto = Path.Combine(diretorio, nomeArquivo);

            try
            {
                servico.GerarRelatorio(caminhoCompleto);
                Console.WriteLine($"✅ Relatório gerado automaticamente em: {caminhoCompleto}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar relatório: {ex.Message}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao registrar falha: {ex.Message}");
    }
}

static void ListarFalhas(FalhaEnergiaService falhaEnergiaService)
{
    var falhas = falhaEnergiaService.ObterTodasFalhas();
    Console.WriteLine("\n--- FALHAS REGISTRADAS ---");

    if (!falhas.Any())
    {
        Console.WriteLine("⚠️ Nenhuma falha registrada.");
        return;
    }

    foreach (var falha in falhas)
    {
        Console.WriteLine($"\nID: {falha.Id}");
        Console.WriteLine($"Início: {falha.DataHoraInicio}");
        Console.WriteLine($"Fim: {falha.DataHoraFim}");
        Console.WriteLine($"Duração: {falha.ObterDuracao().TotalMinutes} minutos");
        Console.WriteLine($"Local: {falha.Local}");
        Console.WriteLine($"Impacto: {falha.Impacto}");
        Console.WriteLine($"Severidade: {falha.Severidade}");
        Console.WriteLine("Planos de Ação:");
        if (falha.PlanosAcao.Count == 0)
        {
            Console.WriteLine("  Nenhum plano de ação registrado.");
        }
        else
        {
            foreach (var plano in falha.PlanosAcao)
            {
                Console.WriteLine($"- Ação: {plano.Acao}");
                Console.WriteLine($"  Responsável: {plano.Responsavel}");
                Console.WriteLine($"  Prazo: {plano.Prazo:yyyy-MM-dd}");
                Console.WriteLine($"  Status: {plano.Status}");
            }
        }
        Console.WriteLine("----------------------------------------");
    }
}

static void RegistrarPlanoAcao(FalhaEnergiaService servico, FalhaEnergiaDAO dao)
{
    var falhas = dao.ObterTodas();
    if (falhas.Count == 0)
    {
        Console.WriteLine("⚠️ Nenhuma falha disponível para associar um plano.");
        return;
    }

    Console.WriteLine("Falhas disponíveis:");
    foreach (var falha in falhas)
    {
        Console.WriteLine($"{falha.Id} - {falha.Local} ({falha.DataHoraInicio} a {falha.DataHoraFim})");
    }

    Console.Write("Informe o ID da falha: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid falhaId))
    {
        Console.WriteLine("⚠️ ID inválido.");
        return;
    }

    Console.Write("Descrição do plano de ação: ");
    var acao = Console.ReadLine();

    Console.Write("Responsável: ");
    var responsavel = Console.ReadLine();

    Console.Write("Prazo para execução (yyyy-MM-dd): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime prazo))
    {
        Console.WriteLine("⚠️ Data de prazo inválida.");
        return;
    }

    servico.AdicionarPlanoAcao(falhaId, acao, responsavel, prazo);
    Console.WriteLine("✅ Plano de ação registrado com sucesso!");
}

static void AtualizarStatusPlanoAcao(FalhaEnergiaService falhaEnergiaService)
{
    var falhas = falhaEnergiaService.ObterTodasFalhas();

    if (!falhas.Any())
    {
        Console.WriteLine("⚠️ Nenhuma falha registrada.");
        return;
    }

    Console.WriteLine("Falhas registradas:");
    foreach (var falha in falhas)
    {
        Console.WriteLine($"ID: {falha.Id}, Local: {falha.Local}, Início: {falha.DataHoraInicio}, Fim: {falha.DataHoraFim}");
        if (falha.PlanosAcao.Count == 0)
        {
            Console.WriteLine("  Nenhum plano de ação registrado.");
        }
        else
        {
            foreach (var plano in falha.PlanosAcao)
            {
                Console.WriteLine($"  Plano ID: {plano.Id}, Ação: {plano.Acao}, Responsável: {plano.Responsavel}, Prazo: {plano.Prazo:yyyy-MM-dd}, Status: {plano.Status}");
            }
        }
    }

    Console.Write("Digite o ID da falha que deseja atualizar: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid falhaId))
    {
        Console.WriteLine("⚠️ ID da falha inválido.");
        return;
    }

    Console.Write("Digite o ID do plano de ação que deseja atualizar: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid planoId))
    {
        Console.WriteLine("⚠️ ID do plano inválido.");
        return;
    }

    Console.WriteLine("Selecione o novo status:");
    Console.WriteLine("1 - Pendente");
    Console.WriteLine("2 - Em Andamento");
    Console.WriteLine("3 - Concluído");

    if (!int.TryParse(Console.ReadLine(), out int statusNum) ||
        !Enum.IsDefined(typeof(EStatusPlanoAcao), statusNum))
    {
        Console.WriteLine("⚠️ Status inválido.");
        return;
    }

    falhaEnergiaService.AtualizarStatusPlano(falhaId, planoId, (EStatusPlanoAcao)statusNum);
    Console.WriteLine("✅ Status atualizado com sucesso!");
}
