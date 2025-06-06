using GS.Application.Models;

namespace GS.Application.DAO;

public interface IFalhaEnergiaDAO
{
    public void Adicionar(FalhaEnergia falha);
    public List<FalhaEnergia> ObterTodas();
    public FalhaEnergia? ObterPorId(Guid id);
    public void AdicionarPlanoAcao(Guid falhaId, PlanoAcao plano);
    public void AtualizarPlanoAcao(Guid falhaId, PlanoAcao plano);
}
