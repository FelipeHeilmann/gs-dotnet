namespace GS.Application.Models;

public class FalhaEnergia
{
    public Guid Id { get; private set; }
    public DateTime DataHoraInicio { get; private set; }
    public DateTime DataHoraFim { get; private set; }
    public string Local { get; private set; }
    public string Impacto { get; private set; }
    public int Severidade { get; private set; }
    public List<PlanoAcao> PlanosAcao { get; set; } = new();

    public FalhaEnergia(DateTime inicio, DateTime fim, string local, string impacto, int severidade)
    {
        Id = Guid.NewGuid();
        DataHoraInicio = inicio;
        DataHoraFim = fim;
        Local = local;
        Impacto = impacto;
        Severidade = severidade;
    }
    public TimeSpan ObterDuracao() => DataHoraFim - DataHoraInicio;
}