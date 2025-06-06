using GS.Application.DAO;
using GS.Application.Models;

namespace GS.Infra.DAO;

public class FalhaEnergiaDAO : IFalhaEnergiaDAO
{
    private readonly List<FalhaEnergia> _falhas = new();

    public void Adicionar(FalhaEnergia falha)
    {
        _falhas.Add(falha);
    }

    public List<FalhaEnergia> ObterTodas()
    {
        return _falhas;
    }

    public FalhaEnergia? ObterPorId(Guid id)
    {
        return _falhas.FirstOrDefault(f => f.Id == id);
    }

    public void AdicionarPlanoAcao(Guid falhaId, PlanoAcao plano)
    {
        var falha = _falhas.FirstOrDefault(f => f.Id == falhaId);
        if (falha == null)
            throw new Exception("Falha não encontrada");

        falha.PlanosAcao.Add(plano);
    }

    public void AtualizarPlanoAcao(Guid falhaId, PlanoAcao plano)
    {
        var falha = _falhas.FirstOrDefault(f => f.Id == falhaId);
        if (falha == null)
            throw new Exception("Falha não encontrada");

        var planoExistente = falha.PlanosAcao.FindIndex(p => p.Id == plano.Id);
        if (planoExistente == -1)
            throw new Exception("Plano de ação não encontrado");

        falha.PlanosAcao[planoExistente] = plano;
    }
}
