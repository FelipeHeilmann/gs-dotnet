using GS.Application.Enums;

namespace GS.Application.Models;

public class PlanoAcao
{
    public Guid Id { get; private set; }
    public Guid FalhaId { get; private set; }
    public string Acao { get; private set; }
    public string Responsavel { get; private set; }
    public DateTime Prazo { get; private set; }
    public bool Concluido { get; private set; } = false;
    public EStatusPlanoAcao Status { get; set; } = EStatusPlanoAcao.Pendente;

    public PlanoAcao(Guid falhaId, string acao, string responsavel, DateTime prazo)
    {
        Id = Guid.NewGuid();
        FalhaId = falhaId;
        Acao = acao;
        Responsavel = responsavel;
        Prazo = prazo;
    }

}
