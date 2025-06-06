using System.Text;
using GS.Application.DAO;
using GS.Application.Enums;
using GS.Application.Models;

namespace GS.Application.Service;

public class FalhaEnergiaService
{
    private readonly IFalhaEnergiaDAO _dao;

    public FalhaEnergiaService(IFalhaEnergiaDAO dao)
    {
        _dao = dao;
    }

    public IEnumerable<FalhaEnergia> ObterTodasFalhas()
    {
        return _dao.ObterTodas();
    }

    public void RegistrarFalha(DateTime inicio, DateTime fim, string local, string impacto, int severidade)
    {
        if (string.IsNullOrWhiteSpace(local) || string.IsNullOrWhiteSpace(impacto))
            throw new ArgumentException("Local e impacto devem ser preenchidos.");

        if (severidade < 1 || severidade > 5)
            throw new ArgumentException("Severidade deve ser entre 1 e 5.");

        if (fim <= inicio)
            throw new ArgumentException("Data/hora de fim deve ser maior que a de início.");

        var falha = new FalhaEnergia(inicio, fim, local, impacto, severidade);

        _dao.Adicionar(falha);
    }

    public void GerarRelatorio(string caminhoArquivo)
    {
        var falhas = _dao.ObterTodas();

        var sb = new StringBuilder();
        sb.AppendLine("=== RELATÓRIO DE FALHAS DE ENERGIA ===");
        sb.AppendLine($"Gerado em: {DateTime.Now}\n");

        foreach (var falha in falhas)
        {
            sb.AppendLine($"ID: {falha.Id}");
            sb.AppendLine($"Início: {falha.DataHoraInicio}");
            sb.AppendLine($"Fim: {falha.DataHoraFim}");
            sb.AppendLine($"Duração: {falha.ObterDuracao().TotalMinutes} minutos");
            sb.AppendLine($"Local: {falha.Local}");
            sb.AppendLine($"Impacto: {falha.Impacto}");
            sb.AppendLine($"Severidade: {falha.Severidade}");
            sb.AppendLine("Planos de Ação:");
            if (falha.PlanosAcao.Count == 0)
            {
                sb.AppendLine("Nenhum plano de ação registrado.");
            }
            else
            {
                foreach (var plano in falha.PlanosAcao)
                {
                    sb.AppendLine($"- Ação: {plano.Acao}");
                    sb.AppendLine($"  Responsável: {plano.Responsavel}");
                    sb.AppendLine($"  Prazo: {plano.Prazo.ToShortDateString()}");
                    sb.AppendLine($"  Status: {plano.Status.ToString()}");
                }
            }
            sb.AppendLine("----------------------------------------");
        }

        File.WriteAllText(caminhoArquivo, sb.ToString());
    }

    public void AdicionarPlanoAcao(Guid falhaId, string acao, string responsavel, DateTime prazo)
    {
        var plano = new PlanoAcao
        (
            falhaId,
            acao,
            responsavel,
            prazo
        );

        _dao.AdicionarPlanoAcao(falhaId, plano);
    }
    
    public void AtualizarStatusPlano(Guid falhaId, Guid planoId, EStatusPlanoAcao novoStatus)
    {
        var falha = _dao.ObterPorId(falhaId);
        if (falha == null)
            throw new Exception("Falha não encontrada");

        var plano = falha.PlanosAcao.FirstOrDefault(p => p.Id == planoId);
        if (plano == null)
            throw new Exception("Plano de ação não encontrado");

        plano.Status = novoStatus;
        
        _dao.AtualizarPlanoAcao(falhaId, plano);
    }
}
