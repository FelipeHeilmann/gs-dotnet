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
        Console.WriteLine("\nUsuário ou senha inválidos. Tente novamente.");
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
    Console.WriteLine("5. Atualiar status de plano de ação");
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
                Console.WriteLine("Relatório gerado com sucesso!");
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
                Console.WriteLine("Opção inválida!");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }

}
static void RegistrarFalha(FalhaEnergiaService servico)
{
    Console.Write("Data e hora de início (yyyy-MM-dd HH:mm): ");
    DateTime inicio = DateTime.Parse(Console.ReadLine());

    Console.Write("Data e hora de fim (yyyy-MM-dd HH:mm): ");
    DateTime fim = DateTime.Parse(Console.ReadLine());

    Console.Write("Local da falha: ");
    var local = Console.ReadLine();

    Console.Write("Descrição do impacto: ");
    var impacto = Console.ReadLine();

    Console.Write("Severidade (1 a 5): ");
    var severidade = int.Parse(Console.ReadLine());

    servico.RegistrarFalha(inicio, fim, local, impacto, severidade);
    Console.WriteLine("Falha registrada com sucesso!");
}

static void ListarFalhas(FalhaEnergiaService falhaEnergiaService)
{
    var falhas = falhaEnergiaService.ObterTodasFalhas();
    Console.WriteLine("\n--- FALHAS REGISTRADAS ---");

    if (!falhas.Any())
    {
        Console.WriteLine("Nenhuma falha registrada.");
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
            Console.WriteLine("Nenhum plano de ação registrado.");
        }
        else
        {
            foreach (var plano in falha.PlanosAcao)
            {
                Console.WriteLine($"- Ação: {plano.Acao}");
                Console.WriteLine($"  Responsável: {plano.Responsavel}");
                Console.WriteLine($"  Prazo: {plano.Prazo.ToShortDateString()}");
                Console.WriteLine($"  Status: {plano.Status.ToString()}");
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
        Console.WriteLine("Nenhuma falha disponível para associar um plano.");
        return;
    }

    Console.WriteLine("Falhas disponíveis:");
    foreach (var falha in falhas)
    {
        Console.WriteLine($"{falha.Id} - {falha.Local} ({falha.DataHoraInicio} a {falha.DataHoraFim})");
    }

    Console.Write("Informe o ID da falha: ");
    Guid falhaId = Guid.Parse(Console.ReadLine());

    Console.Write("Descrição do plano de ação: ");
    var acao = Console.ReadLine();

    Console.Write("Responsável: ");
    var responsavel = Console.ReadLine();

    Console.Write("Prazo para execução (yyyy-MM-dd): ");
    DateTime prazo = DateTime.Parse(Console.ReadLine());

    servico.AdicionarPlanoAcao(falhaId, acao, responsavel, prazo);
    Console.WriteLine("Plano de ação registrado com sucesso!");
}

static void AtualizarStatusPlanoAcao(FalhaEnergiaService falhaEnergiaService)
{
    var falhas = falhaEnergiaService.ObterTodasFalhas();

    if (!falhas.Any())
    {
        Console.WriteLine("Nenhuma falha registrada.");
        return;
    }

    Console.WriteLine("Falhas registradas:");
    foreach (var falha in falhas)
    {
        Console.WriteLine($"ID: {falha.Id}, Local: {falha.Local}, Início: {falha.DataHoraInicio}, Fim: {falha.DataHoraFim}");
        if (falha.PlanosAcao.Count == 0)
        {
            Console.WriteLine("Nenhum plano de ação registrado.");
        }
        else
        {
            foreach (var plano in falha.PlanosAcao)
            {
                Console.WriteLine($"  Plano ID: {plano.Id}, Ação: {plano.Acao}, Responsável: {plano.Responsavel}, Prazo: {plano.Prazo.ToShortDateString()}, Status: {plano.Status.ToString()}");
            }
        }
    }

    Console.WriteLine("Digite o ID da falha que deseja atualizar: ");
    var falhaId = Guid.Parse(Console.ReadLine());

    Console.WriteLine("Digite o ID do plano de ação que deseja atualizar: ");
    var planoId = Guid.Parse(Console.ReadLine());

    Console.WriteLine("Selecione o novo status:");
    Console.WriteLine("1 - Pendente");
    Console.WriteLine("2 - Em Andamento");
    Console.WriteLine("3 - Concluído");

    var statusNum = int.Parse(Console.ReadLine());

    if (!Enum.IsDefined(typeof(EStatusPlanoAcao), statusNum))
    {
        Console.WriteLine("Status inválido.");
        return;
    }

    falhaEnergiaService.AtualizarStatusPlano(falhaId, planoId, (EStatusPlanoAcao)statusNum);
    Console.WriteLine("Status atualizado com sucesso!");
}